using Android.App;
using Android.Content.PM;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.OS;
using Android.Runtime;
using Firebase.Auth;
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
        public ICallbackManager CallbackManager { get; private set; } 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            CallbackManager = CallbackManagerFactory.Create();
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

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
            base.OnActivityResult(requestCode, resultCode, data);
            CallbackManager.OnActivityResult(requestCode, ((int)resultCode), data);
            if (requestCode == REQC_GOOGLE_SIGN_IN)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                FirebaseAuthWithGoogle(result.SignInAccount);
                GoogleLoginActivity.Instance.OnAuthCompleted(result);
            }
        }
        /// <summary>
        ///     Registro de cuenta logueada en Firebase
        /// </summary>
        /// <param name="acct">Cuenta logueada</param>
        private void FirebaseAuthWithGoogle(GoogleSignInAccount acct)
        {
            AuthCredential credential = GoogleAuthProvider.GetCredential(acct.IdToken, null);
            FirebaseAuth.Instance.SignInWithCredential(credential);
            
        }
    }
}