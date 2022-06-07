using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using PetArmy.Interfaces;
using PetArmy.Models;
using System;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(PetArmy.Droid.GoogleLoginActivity))]
namespace PetArmy.Droid
{
    public class GoogleLoginActivity : Activity, IGoogleAuth, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        Context _context;
        public Action<UserProfile, string> _onLoginComplete;
        public static GoogleApiClient _googleApiClient { get; set; }
        public static GoogleLoginActivity Instance { get; private set; }
        public GoogleLoginActivity() 
        {
            _context = Android.App.Application.Context;
            Instance = this;
        }
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
            if (result.IsSuccess)
            {
                GoogleSignInAccount accountt = result.SignInAccount;

                _onLoginComplete?.Invoke(new UserProfile()
                {
                    Name = accountt.DisplayName,
                    Email = accountt.Email,
                    ProfilePictureUrl = accountt.PhotoUrl.ToString()
                }, string.Empty);
            }
            else
            {
                _onLoginComplete?.Invoke(null, "An error occured!");
            }
        }
    }
}