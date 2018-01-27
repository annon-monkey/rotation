using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rotation.Forms.Models.Play
{
    class UfoSaConnectionModel
    {
        private IGattCharacteristic characteristic;
        private readonly SemaphoreSlim locker = new SemaphoreSlim(1);
        private bool isTryingConnect;

        public bool IsConnecting => this.characteristic != null;

        public UfoSaConnectionModel()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    if (this.characteristic == null)
                    {
                        try
                        {
                            this.BeginConnecting();
                        }
                        catch
                        {
                        }
                    }
                    await Task.Delay(10000);
                }
            });
        }

        private void BeginConnecting()
        {
            this.isTryingConnect = false;

            CrossBleAdapter.Current.Scan()
                .Where(scanResult => scanResult.Device.Name == "UFOSA")
                .FirstAsync()
                .Subscribe(async scanResult =>
                {
                    try
                    {
                        // Once finding the device/scanresult you want
                        await scanResult.Device.Connect();

                        scanResult.Device.WhenAnyCharacteristicDiscovered()
                            .Where(x => x.CanWrite() && !x.CanRead())
                            .Where(x => !x.Service.Uuid.ToString().Contains("-0000-"))
                            .FirstAsync()
                            .Subscribe(async characteristic => {
                                try
                                {
                                    if (this.isTryingConnect) return;

                                    this.isTryingConnect = true;
                                    await characteristic.Write(new byte[] { 0x02, 0x01, 0x00, });
                                    this.characteristic = characteristic;
                                    this.Connected?.Invoke(this, EventArgs.Empty);
                                }
                                catch
                                {
                                    this.isTryingConnect = false;
                                    if (this.characteristic != null)
                                    {
                                        this.characteristic = null;
                                        this.Disconnected?.Invoke(this, EventArgs.Empty);
                                    }
                                }
                            });
                        scanResult.Device.WhenStatusChanged().Subscribe(status =>
                        {
                            if (status == ConnectionStatus.Disconnecting || status == ConnectionStatus.Disconnected)
                            {
                                this.characteristic = null;
                                this.Disconnected?.Invoke(this, EventArgs.Empty);
                            }
                        });
                    }
                    catch
                    {
                        this.isTryingConnect = false;
                        if (this.characteristic != null)
                        {
                            this.characteristic = null;
                            this.Disconnected?.Invoke(this, EventArgs.Empty);
                        }
                    }
                });
        }

        public async Task WriteAsync(byte[] bytes)
        {
            await this.locker.WaitAsync();
            Debug.WriteLine(bytes[2]);
            if (this.characteristic != null)
            {
                await this.characteristic.Write(bytes);
            }
            this.locker.Release();
        }

        public event EventHandler Connected;

        public event EventHandler Disconnected;
    }
}
