using System.Diagnostics;
using System.Text;
using Android.Opengl;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace BeaconFun;

public partial class MainPage : ContentPage
{
    IBluetoothLE ble;
    IAdapter adapter;
    IDevice device;

    public MainPage()
    {
        InitializeComponent();

        ble = CrossBluetoothLE.Current;
        adapter = CrossBluetoothLE.Current.Adapter;

        var state = ble.State;
        //You can also listen for State changes. So you can react if the user turns on / off bluetooth on your smartphone.
        ble.StateChanged += (s, e) =>
        {
            Debug.WriteLine($"The bluetooth state changed to {e.NewState}");
        };
        adapter.DeviceDiscovered += (s, e) =>
        {
            if (!string.IsNullOrEmpty(e.Device.Name))
            {
                Debug.WriteLine($"---");
                Debug.WriteLine($"Discovered {e.Device.Name} {e.Device.Id} {e.Device.Rssi}");
                foreach (var a in e.Device.AdvertisementRecords)
                {
                    Debug.WriteLine($"{a.Type} {ByteArrayToHexString(a.Data)}");

                    if (IsIBeaconAdvertisement(a.Data))
                    {
                        Debug.WriteLine("Its a beacon!");
                    }


                }
            }
            device = e.Device;
        };

        getBLEPermission();
    }

    //TODO: add another for the eddystone beacon specs
    //https://github.com/google/eddystone/blob/master/protocol-specification.md

    //about Tx Power
    //https://docs.silabs.com/bluetooth/2.13/general/system-and-performance/bluetooth-tx-power-settings#tx-power-for-data-packets

    public bool IsIBeaconAdvertisement(byte[] data)
    {
        if (data == null)
            return false;

        if (data.Length != 25)
            return false;

        if (data[2] != 0x02)
            return false;

        return true;
    }

    private static async void getBLEPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                //well kill the app because it's no use if bluetooth not enabled
            }
        }

        PermissionStatus statusBLE = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
        if (statusBLE != PermissionStatus.Granted)
        {
            statusBLE = await Permissions.RequestAsync<Permissions.Bluetooth>();
            if (statusBLE != PermissionStatus.Granted)
            {
                //well kill the app because it's no use if bluetooth not enabled
            }
        }
    }

    private void OnScanButtonClicked(object sender, EventArgs e)
    {
        adapter.ScanMode = ScanMode.LowLatency;
        adapter.ScanMatchMode = ScanMatchMode.AGRESSIVE;//https://developer.android.com/reference/android/bluetooth/le/ScanSettings#MATCH_MODE_AGGRESSIVE

        Task.Run(async () =>
        {
            adapter.ScanTimeout = 3000;
            //var scanFilterOptions = new ScanFilterOptions();
            //scanFilterOptions.ServiceUuids = new[] { guid1, guid2, etc }; // cross platform filter
            //scanFilterOptions.ManufacturerDataFilters = new[] { new ManufacturerDataFilter(1), new ManufacturerDataFilter(2) }; // android only filter
            //scanFilterOptions.DeviceAddresses = new[] { "80:6F:B0:43:8D:3B", "80:6F:B0:25:C3:15", etc }; // android only filter
            //scanFilterOptions.HasManufacturerIds();
            await adapter.StartScanningForDevicesAsync();
            await Task.Delay(1000);
            //await adapter.StopScanningForDevicesAsync();
            //await Task.Delay(1000);

            await adapter.StartScanningForDevicesAsync();
            await Task.Delay(1000);
            //await adapter.StopScanningForDevicesAsync();
            //await Task.Delay(1000);

            await adapter.StartScanningForDevicesAsync();
            await Task.Delay(1000);
            //await adapter.StopScanningForDevicesAsync();
            //await Task.Delay(1000);


        });

        Debug.WriteLine("Done Scanning");

    }

    public string  ByteArrayToHexString(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "");
        
    }
}


