using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Tasks;
using Android.OS;
using PetArmy.Droid.Implementations;
using PetArmy.Interfaces;
using PetArmy.Models;
using System;
using Xamarin.Forms;

#pragma warning disable CS0612 // Type or member is obsolete
[assembly: Dependency(typeof(PetArmy.Droid.GoogleLoginActivity))]
#pragma warning restore CS0612 // Type or member is obsolete
namespace PetArmy.Droid
{
    [Activity(Label = "GoogleLoginActivity")]
    [Obsolete]
    public class GoogleLoginActivity : Java.Lang.Object, IGoogleAuth, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, IOnCompleteListener
    {
        Context _context;
        public Action<UserProfile, string> _onLoginComplete;

        [Obsolete]
        private GoogleSignInAccount _accountt;
        public static GoogleApiClient _googleApiClient { get; set; }
        public static GoogleSignInClient _googleSigInClient { get; set; }
        public GoogleLoginActivity() 
        {
            _instance = this;
            _context = Android.App.Application.Context;
        }
        
        #region Eventos disparados por la SDK de Google
        public void OnConnected(Bundle p0)
        {
        }
        public void OnConnectionFailed(ConnectionResult result)
        {
            if(result.ErrorMessage != null)
                _onLoginComplete?.Invoke(null, result.ErrorMessage);
        }
        public void OnConnectionSuspended(int result)
        {
            _onLoginComplete?.Invoke(null, "Cancelado!!");
        }
        #endregion  
        
        [Obsolete]
        public void Login(Action<UserProfile, string> onLoginComplete)
        {
            _onLoginComplete = onLoginComplete;

            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                                                             .RequestEmail()
                                                             .RequestProfile()
                                                             .RequestIdToken("538291567160-fedlsu153imrurdjnouvghejatjkrjd2.apps.googleusercontent.com")
                                                             .Build();
            _googleApiClient = new GoogleApiClient.Builder((_context).ApplicationContext)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                .AddScope(new Scope(Scopes.Profile))
                .Build();
            _googleApiClient.Connect();

            Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(_googleApiClient);

            ((MainActivity)Forms.Context).StartActivityForResult(signInIntent, MainActivity.REQC_GOOGLE_SIGN_IN);
        }
       
        /// <summary>
        /// Google Sign In result listener
        /// </summary>
        /// <param name="result"></param>
        public void OnSignInCompleted(GoogleSignInResult result)
        {
            //Failed Sign In
            if (!result.IsSuccess)
            {
                _onLoginComplete?.Invoke(null, result.Status.StatusMessage);
            }
            else
            {
                //Successful Sign In
                _accountt = result.SignInAccount;

                //Attempting to register Google Account
                FirebaseAuthentication.GetInstance()
                    .FirebaseAuthRegister(_accountt, MainActivity.REQC_GOOGLE_SIGN_IN)
                    .AddOnCompleteListener(this);
            }
        }

        /// <summary>
        /// GMS Task Completion Listener
        /// </summary>
        /// <param name="task"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnComplete(Task task)
        {
            //Failed Sign Up
            if (task.Exception != null)
            {
                _onLoginComplete?.Invoke(null, task.Exception.Message);
            }
            else
            {
                //Successful Sign Up
                _onLoginComplete?.Invoke(new UserProfile()
                {
                    Name = _accountt.DisplayName,
                    Email = _accountt.Email,
                    ProfilePictureUrl = _accountt.PhotoUrl.ToString()
                }, string.Empty);
            }
        }

        #region Singleton
        private static GoogleLoginActivity _instance;
        public static GoogleLoginActivity GetInstance()
        {
            if (_instance == null)
                return new GoogleLoginActivity();
            else
                return _instance;

        }
        #endregion
    }
}