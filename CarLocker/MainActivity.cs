using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Core;
using System;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.Support.Design.Widget;
using Android.Views;

namespace CarLocker
{
    [Activity(Label = "CarLocker", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EventHandler<BluetoothLEManager.DeviceConnectionEventArgs> deviceConnectedHandler;
        EventHandler<BluetoothLEManager.DeviceDiscoveredEventArgs> deviceDiscoveredHandler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Button btnLock = FindViewById<Button>(Resource.Id.buttonLock);
            Button btnUnlock = FindViewById<Button>(Resource.Id.buttonUnlock);
            RestClientService client = new RestClientService();

            GetLocationPermissionAsync();

            btnLock.Click += (sender, e) =>
            {
                string response = client.SendLockRequest();
                if (!string.IsNullOrEmpty(response))
                    Toast.MakeText(this, response, ToastLength.Short).Show();
            };

            btnUnlock.Click += (sender, e) =>
            {
                string response = client.SendUnlockRequest();
                if (!string.IsNullOrEmpty(response))
                    Toast.MakeText(this, response, ToastLength.Short).Show();

            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (!BluetoothLEManager.Current.IsScanning)
            {
                BluetoothLEManager.Current.BeginScanningForDevices();
            }
            else
            {
            }

            this.WireupExternalHandlers();
        }

        protected void WireupExternalHandlers()
        {
            this.deviceDiscoveredHandler = (object sender, BluetoothLEManager.DeviceDiscoveredEventArgs e) =>
            {
                Toast.MakeText(this, "A BLE was detected", ToastLength.Short).Show();
            };
            BluetoothLEManager.Current.DeviceDiscovered += this.deviceDiscoveredHandler;

        }

        readonly string[] PermissionsLocation =
{
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation
            };

        const int RequestLocationId = 0;

        protected async Task GetLocationPermissionAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;
            if (CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                await GetLocationAsync();
                return;
            }

            if (ShouldShowRequestPermissionRationale(permission))
            {

                //Explain to the user why we need to read the contacts
                Snackbar.Make(FindViewById<View>(Resource.Id.action_bar), "Location access is required to show coffee shops nearby.", Snackbar.LengthIndefinite)
                        .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
                        .Show();
                return;
            }

            RequestPermissions(PermissionsLocation, RequestLocationId);
        }

        async Task TryGetLocationAsync()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                await GetLocationAsync();
                return;
            }

            await GetLocationPermissionAsync();
        }

          public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            //Permission granted
                            var snack = Snackbar.Make(FindViewById<View>(Resource.Id.action_bar), "Location permission is available, getting lat/long.", Snackbar.LengthShort);
                            snack.Show();

                            await GetLocationAsync();
                        }
                        else
                        {
                            //Permission Denied :(
                            //Disabling location functionality
                            //var snack = Snackbar.Make(layout, "Location permission is denied.", Snackbar.LengthShort);
                            //snack.Show();
                        }
                    }
                    break;
            }
        }

        async Task GetLocationAsync()
        {
            //textLocation.Text = "Getting Location";
            try
            {
                //var locator = CrossGeolocator.Current;
                //locator.DesiredAccuracy = 100;
                //var position = await locator.GetPositionAsync(20000);

                //textLocation.Text = string.Format("Lat: {0}  Long: {1}", position.Latitude, position.Longitude);
            }
            catch (Exception ex)
            {
                //textLocation.Text = "Unable to get location: " + ex.ToString();
            }
        }
    }
}

