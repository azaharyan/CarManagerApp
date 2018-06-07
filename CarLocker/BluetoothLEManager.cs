using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using Android.Bluetooth.LE;

namespace CarLocker
{
    class BluetoothLEManager : Java.Lang.Object, BluetoothAdapter.ILeScanCallback
    {
        public event EventHandler<DeviceDiscoveredEventArgs> DeviceDiscovered = delegate { };
        public event EventHandler<DeviceConnectionEventArgs> DeviceConnected = delegate { };
        public event EventHandler<DeviceConnectionEventArgs> DeviceDisconnected = delegate { };

        protected BluetoothManager _manager;
        protected BluetoothLeScanner _scanner;

        public bool IsScanning
        {
            get { return this._isScanning; }
        }
        protected bool _isScanning = false;

        public List<BluetoothDevice> DiscoveredDevices
        {
            get { return this._discoveredDevices; }
        }
        List<BluetoothDevice> _discoveredDevices = new List<BluetoothDevice>();

        public void Dispose()
        {
            if (this._scanner != null)
                this._scanner.Dispose();
            if (this._manager != null)
                this._manager.Dispose();
        }

        public static BluetoothLEManager Current
        {
            get { return current; }
        }
        private static BluetoothLEManager current;

        static BluetoothLEManager()
        {
            current = new BluetoothLEManager();
        }

        protected BluetoothLEManager()
        {
            var appContext = Android.App.Application.Context;
            this._manager = (BluetoothManager)appContext.GetSystemService("bluetooth");
            this._scanner = this._manager.Adapter.BluetoothLeScanner;
        }

        public async Task  BeginScanningForDevices()
        {
            this._discoveredDevices = new List<BluetoothDevice>();

            this._isScanning = true;
            ScanSettings settings = new ScanSettings.Builder().SetScanMode(Android.Bluetooth.LE.ScanMode.LowPower).SetCallbackType(ScanCallbackType.AllMatches).Build();
            ScanFilter filter = new ScanFilter.Builder().SetDeviceAddress("63:8A:87:FE:F3:76").Build();
            //ScanFilter filter2 = new ScanFilter.Builder().SetServiceUuid(ParcelUuid.fromString(serviceUUIDs[i].toStri‌​ng())).build();
            List<ScanFilter> filters = new List<ScanFilter>();
            filters.Add(filter); 
            this._scanner.StartScan(filters, settings, new BluetoothScanCallback());

            await Task.Delay(10000);

            if(this._isScanning)
            {
                //this._scanner.StopScan(new PendingIntent());
            }

        }

        public void OnLeScan(BluetoothDevice device, int rssi, byte[] scanRecord)
        {
            Console.WriteLine("LeScanCallback: " + device.Name);
            // TODO: for some reason, this doesn't work, even though they have the same pointer,
            // it thinks that the item doesn't exist. so i had to write my own implementation
            //			if(!this._discoveredDevices.Contains(device) ) {
            //				this._discoveredDevices.Add (device );
            //			}		
            //if (!DeviceExistsInDiscoveredList(device))
            //    this._discoveredDevices.Add(device);
            //// TODO: in the cross platform API, cache the RSSI
            //this.DeviceDiscovered(this, new DeviceDiscoveredEventArgs { Device = device, Rssi = rssi, ScanRecord = scanRecord });
        }

        public class BluetoothScanCallback : ScanCallback
        {
            public virtual void OnBatchScanResults(IList<ScanResult> results)
            {
                base.OnBatchScanResults(results);
                Console.WriteLine(results);
            }
            public override void OnScanResult([GeneratedEnum] ScanCallbackType callbackType, ScanResult result)
            {
                base.OnScanResult(callbackType, result);
                BluetoothDevice device = result.Device;
                Console.WriteLine(result);
            }

            public override void OnScanFailed([GeneratedEnum] ScanFailure errorCode)
            {
                base.OnScanFailed(errorCode);
            }
        }

        public class DeviceDiscoveredEventArgs : EventArgs
        {
            public BluetoothDevice Device;
            public int Rssi;
            public byte[] ScanRecord;

            public DeviceDiscoveredEventArgs() : base()
            { }
        }

        public class DeviceConnectionEventArgs : EventArgs
        {
            public BluetoothDevice Device;

            public DeviceConnectionEventArgs() : base()
            { }
        }

        public class ServiceDiscoveredEventArgs : EventArgs
        {
            public BluetoothGatt Gatt;

            public ServiceDiscoveredEventArgs() : base()
            { }
        }
    }
}