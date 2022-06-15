using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
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
    public class GoogleLoginActivity : Java.Lang.Object, IGoogleAuth, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        Context _context;
        public Action<UserProfile, string> _onLoginComplete;

        [Obsolete]
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

            _onLoginComplete = onLoginComplete;

            Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(_googleApiClient);

            ((MainActivity)Forms.Context).StartActivityForResult(signInIntent, MainActivity.REQC_GOOGLE_SIGN_IN);
        }
        public void OnAuthCompleted(GoogleSignInResult result)
        {
            //Failed Sign In
            if (!result.IsSuccess)
            {
                _onLoginComplete?.Invoke(null, result.Status.StatusMessage);
            }
            else
            {
                //Successful Sign In
                GoogleSignInAccount accountt = result.SignInAccount;

                //Attempting to register Google Account
                var creation_result = FirebaseAuthentication.GetInstance()
                    .FirebaseAuthRegister(accountt, MainActivity.REQC_GOOGLE_SIGN_IN);

                //Failed Sign Up
                if (creation_result.Exception != null)
                {
                    _onLoginComplete?.Invoke(null, creation_result.Exception.Message);
                }
                else
                {
                    //Successful Sign Up
                    _onLoginComplete?.Invoke(new UserProfile()
                    {
                        Name = accountt.DisplayName,
                        Email = accountt.Email,
                        ProfilePictureUrl = accountt.PhotoUrl.ToString()
                    }, string.Empty);
                }
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