using Plugin.BluetoothLE;
using Plugin.BluetoothLE.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Rotation.Forms
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            CrossBleAdapter.Current.Scan()
                .Where(scanResult => scanResult.Device.Name == "UFOSA")
                .Subscribe(async scanResult =>
                {
                    // Once finding the device/scanresult you want
                    await scanResult.Device.Connect();

                    scanResult.Device.WhenAnyCharacteristicDiscovered()
                        .Subscribe(async characteristic =>
                        {
                            Debug.WriteLine($" -- [LOG] {characteristic.Description} : {characteristic.Properties}");
                            Debug.WriteLine($" --       {characteristic.Service.Uuid}");
                            Debug.WriteLine($" --       {characteristic.Service.Device.PairingStatus}");
                            Debug.WriteLine($" --       {characteristic.Service.Device.Name}");
                            Debug.WriteLine($" --       {characteristic.Service.Device.Status}");
                            Debug.WriteLine($" --       {characteristic.Service.Device.Uuid}");
                            Debug.WriteLine($" --       {characteristic.Service.Device.Features}");
                        });

                    scanResult.Device.WhenAnyCharacteristicDiscovered()
                        .Where(x => x.CanWrite())
                        .Where(x => !x.Service.Uuid.ToString().Contains("-0000-"))
                        .FirstAsync()
                        .Subscribe(async characteristic => {
                        // read, write, or subscribe to notifications here
                            try
                            {
                                //var result = await characteristic.Read(); // use result.Data to see response
                                await characteristic.Write(new byte[] { 0x02, 0x01, 0x00, });
                                await characteristic.Write(new byte[] { 0x02, 0x01, 0x20, });
                                await Task.Delay(1000);
                                await characteristic.Write(new byte[] { 0x02, 0x01, 0x40, });
                                await Task.Delay(1000);
                                await characteristic.Write(new byte[] { 0x02, 0x01, 0x60, });
                                await Task.Delay(1000);
                                await characteristic.Write(new byte[] { 0x02, 0x01, 0xa0, });
                                await Task.Delay(1000);
                                await characteristic.Write(new byte[] { 0x02, 0x01, 0xc0, });
                                await Task.Delay(1000);
                                await characteristic.Write(new byte[] { 0x02, 0x01, 0xe0, });
                            }
                            catch (Exception e)
                            {
                            }
                        });
                });
        }

        private async void StartServer(IScanResult scanResult)
        {
            var uuid = scanResult.Device.Uuid;

            var server = CrossBleAdapter.Current.CreateGattServer();
            var service = server.AddService(Guid.NewGuid(), true);

            var characteristic = service.AddCharacteristic(
                Guid.NewGuid(),
                CharacteristicProperties.Read | CharacteristicProperties.Write,
                GattPermissions.Read | GattPermissions.Write
            );

            characteristic.WhenReadReceived().Subscribe(x =>
            {
                // you must set a reply value
                x.Value = new byte[] { 0x02, 0x01, 0x30, };
                x.Status = GattStatus.Success; // you can optionally set a status, but it defaults to Success
            });
            characteristic.WhenWriteReceived().Subscribe(x =>
            {
                var write = Encoding.UTF8.GetString(x.Value, 0, x.Value.Length);
                // do something value
                Debug.WriteLine("message received: " + write);
            });

            await server.Start(new AdvertisementData
            {
                LocalName = "TestServer",
            });
        }
	}
}
