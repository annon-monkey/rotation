using Rotation.Forms.Models.Editor.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rotation.Forms.Models.Play
{
    class PlayModel : INotifyPropertyChanged
    {
        private readonly UfoSaConnectionModel connection = new UfoSaConnectionModel();
        private bool isStopRequested;

        public bool IsConnecting
        {
            //get => this._isConnecting;
            get => true;
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
            var time = 0;
            try
            {
                foreach (var row in collection.ToEntityTimeline().OrganizedData)
                {
                    Task.Delay((row.Time - time) * 100).Wait();
                    time = row.Time;
                    if (this.isStopRequested || !this.IsConnecting) break;
                    await this.connection.WriteAsync(row.ToBluetoothSignal());
                }
            }
            catch (Exception e)
            {

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
