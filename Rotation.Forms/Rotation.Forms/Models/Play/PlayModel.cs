using Rotation.Forms.Models.Editor.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rotation.Forms.Models.Play
{
    class PlayModel : INotifyPropertyChanged
    {
        private readonly UfoSaConnectionModel connection = new UfoSaConnectionModel();
        private bool isStopRequested;
        private IDisposable stopper;

        public bool IsConnecting
        {
            get => this._isConnecting;
            set
            {
                if (this._isConnecting != value)
                {
                    this._isConnecting = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool _isConnecting;

        public PlayModel()
        {
            var uiThread = TaskScheduler.FromCurrentSynchronizationContext();
            var fac = new TaskFactory(uiThread);
            this.connection.Connected += (sender, e) =>
            {
                fac.StartNew(() => this.IsConnecting = true);
            };
            this.connection.Disconnected += (sender, e) =>
            {
                fac.StartNew(() => this.IsConnecting = false);
            };
        }

        public async Task PlayAsync(ElementCollection collection)
        {
            this.isStopRequested = false;
            
            if (collection.CollectionData.IsRepeat)
            {
                while (!this.isStopRequested && this.IsConnecting)
                {
                    await this.SendAsync(collection);
                }
            }
            else
            {
                await this.SendAsync(collection);
            }
        }

        private async Task SendAsync(ElementCollection collection)
        {
            var rows = collection.ToEntityTimeline().OrganizedData.GetEnumerator();
            rows.MoveNext();
            var next = rows.Current;

            var locker = new SemaphoreSlim(1);

            this.stopper =
                Observable.Interval(TimeSpan.FromMilliseconds(100))
                            .Where(x => x >= next.Time)
                            .Subscribe(async x =>
                            {
                                if (this.isStopRequested || !this.IsConnecting)
                                {
                                    this.stopper.Dispose();
                                    rows.Dispose();
                                    this.stopper = null;
                                }
                                else
                                {
                                    await locker.WaitAsync();

                                    try
                                    {
                                        await this.connection.WriteAsync(next.ToBluetoothSignal());
                                        if (!rows.MoveNext())
                                        {
                                            this.stopper.Dispose();
                                            rows.Dispose();
                                            this.stopper = null;
                                        }
                                        else
                                        {
                                            next = rows.Current;
                                        }
                                    }
                                    catch
                                    {
                                    }

                                    locker.Release();
                                }
                            });

            while (this.stopper != null)
            {
                await Task.Delay(100);
            }
        }

        public void Stop()
        {
            this.isStopRequested = true;
            this.connection.WriteAsync(new byte[] { 0x02, 0x01, 0x00, });
        }

        #region INotifyProeprtyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
