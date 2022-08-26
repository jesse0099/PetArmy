using Android.App;
using Android.Content.PM;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.OS;
using Android.Runtime;
using Firebase.Auth;
using Firebase.Functions;
using PetArmy.Droid.Implementations;
using Plugin.CurrentActivity;
using Xamarin.Essentials;
using Xamarin.Facebook;

namespace PetArmy.Droid
{
    [Activity(Label = "PetArmy", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //rivate Auth0Client _auth0Client;
        public const int REQC_GOOGLE_SIGN_IN = 1;
        public const int REQC_FACEBOOK_SIGN_IN = 2;
        public const int REQC_EMAILANDPASS_SIGN_IN = 3;
        public ICallbackManager CallbackManager { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            CallbackManager = CallbackManagerFactory.Create(); 
            //FirebaseFunctions functions = FirebaseFunctions.Instance;
            //functions.UseFunctionsEmulator("http://10.0.2.2:5001");    //Implementar para IOS
            //functions.UseFunctionsEmulator("http://192.168.100.207:5001");    //Implementar para IOS
            //FirebaseAuth.Instance.UseEmulator("10.0.2.2",5001);

            //Register Syncfusion license  (Recordar implementar para iOS)
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDQ1OTEyQDMxMzkyZTMxMmUzMGFYUVRyaFV6U1kwWFc4QUhIbWNCOEsxRkpOSjhVSHhsa3dtWDhodDhpY3c9");
            
            base.OnCreate(savedInstanceState);
            
            Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            //Local renderer for PopUps (Recordar implementar para iOS)
            Syncfusion.XForms.Android.PopupLayout.SfPopupLayoutRenderer.Init();

            LoadApplication(new App());

            FirebaseAuth.Instance.AddAuthStateListener(FirebaseAuthentication.GetInstance());

            _instance = this;

            //Set status bar color | Theme bounded
            OnAppThemeChange(null,new Xamarin.Forms.AppThemeChangedEventArgs(Xamarin.Forms.Application.Current.RequestedTheme));
     
            Xamarin.Forms.Application.Current.RequestedThemeChanged += OnAppThemeChange;
        }

        void OnAppThemeChange(object s, Xamarin.Forms.AppThemeChangedEventArgs a)
        {
            object output;
            switch (a.RequestedTheme)
            {
                case Xamarin.Forms.OSAppTheme.Dark:
                    {
                        Xamarin.Forms.Application.Current.Resources.TryGetValue("JacksonPurple", out output);
                        Window.SetStatusBarColor(Android.Graphics.Color.ParseColor(((Xamarin.Forms.Color)output).ToHex()));
                        break;
                    }
                case Xamarin.Forms.OSAppTheme.Light:
                    {
                        Xamarin.Forms.Application.Current.Resources.TryGetValue("Grad3", out output);
                        Window.SetStatusBarColor(Android.Graphics.Color.ParseColor(((Xamarin.Forms.Color)output).ToHex()));
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// Listener de operaciones (Intent)
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {

            CallbackManager.OnActivityResult(requestCode, ((int)resultCode), data);
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == REQC_GOOGLE_SIGN_IN)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                #pragma warning disable CS0612 // Type or member is obsolete
                GoogleLoginActivity.GetInstance().OnSignInCompleted(result);
                #pragma warning restore CS0612 // Type or member is obsolete
            }
        }

        private static MainActivity _instance;
        public static MainActivity GetInstance()
        {
            if (_instance == null)
                return new MainActivity();
            else
                return _instance;

        }

    }
}