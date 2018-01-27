using Plugin.BluetoothLE;
using Plugin.BluetoothLE.Server;
using Rotation.Forms.ViewModels;
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
            this.BindingContext = new MainViewModel();
            return;

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
                                await characteristic.Write(new byte[] { 0x02, 0x01, 0x64, });
                                await Task.Delay(2000);
                                await characteristic.Write(new byte[] { 0x02, 0x01, 0x7f, });
                                await Task.Delay(2000);
                            }
                            catch (Exception e)
                            {
                            }
                        });
                });
        }
	}
}
