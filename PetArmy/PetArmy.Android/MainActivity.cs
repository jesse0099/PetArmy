using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content;
using Auth0.OidcClient;
using Xamarin.Essentials;

namespace PetArmy.Droid
{
    [Activity(Label = "PetArmy", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]

    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "com.petsdevs.petarmy",
    DataHost = "dev-j8oz2x4w.us.auth0.com",
    DataPathPrefix = "/android/com.petsdevs.petarmy/callback")]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private Auth0Client _auth0Client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            _auth0Client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = "dev-j8oz2x4w.us.auth0.com",
                ClientId = "qxii8E9RbRlrnPqdQBkO1QOodXSXFr9e"
            });

            //SetContentView(Resource.Layout.activity_main);

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}