using System.Diagnostics;
using System.Text;
using Android.Opengl;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace BeaconFun;

public partial class MainPage : ContentPage
{
    IBluetoothLE ble;
    IAdapter adapter;

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
        adapter.DeviceDiscovered += OnDeviceDiscovered;

        getBLEPermission();
    }

    private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
    {
        var dev = e.Device;

        if (!string.IsNullOrEmpty(dev.Name))
        {
            Debug.WriteLine($"---");
            Debug.WriteLine($"Discovered {dev.Name} {dev.Id} {dev.Rssi}");
            foreach (var a in dev.AdvertisementRecords)
            {
                Debug.WriteLine($"{a.Type} {BeaconByteHelper.ByteArrayToHexString(a.Data)}");

                if (BeaconByteHelper.IsIBeaconAdvertisement(a.Data))
                {
                    var iBeacon = BeaconByteHelper.ParseiBeaconBytes(a.Data, dev.Rssi);
                    UpdateBeaconSquare(iBeacon);
                }
            }
        }
    }

    private void UpdateBeaconSquare(iBeacon iBeacon)
    {
        switch (iBeacon.Minor)
        {
            case 4949:
                SQ1.BackgroundColor = iBeacon.RssiColor;
                break;
            case 26049 :
                SQ2.BackgroundColor = iBeacon.RssiColor;
                break;
            case 37728:
                SQ3.BackgroundColor = iBeacon.RssiColor;
                break;
        }

    }

    //TODO: add another for the eddystone beacon specs
    //https://github.com/google/eddystone/blob/master/protocol-specification.md

    //about Tx Power
    //https://docs.silabs.com/bluetooth/2.13/general/system-and-performance/bluetooth-tx-power-settings#tx-power-for-data-packets

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

}


