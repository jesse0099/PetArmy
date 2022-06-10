using Android.App;
using Android.Gms.Tasks;
using PetArmy.Interfaces;
using PetArmy.Models;
using System;
using System.Collections.Generic;
using Xamarin.Facebook;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(PetArmy.Droid.FacebookLoginActivity))]
namespace PetArmy.Droid
{
    [Activity(Label = "FacebookLoginActivity")]
    public class FacebookLoginActivity : Activity, IFacebookAuth, IFacebookCallback, IOnSuccessListener, IOnFailureListener
    {
        public Action<UserProfile, string> _onLoginComplete;
        public static FacebookLoginActivity Instance { get; set; }
        public FacebookLoginActivity()
        {
            Instance = this;
        }

        public void Login(Action<UserProfile, string> onLoginComplete)
        {
            var activity = Forms.Context as MainActivity;

            _onLoginComplete = onLoginComplete;

            Xamarin.Facebook.Login.LoginManager.Instance.RegisterCallback(activity.CallbackManager, this);

            Xamarin.Facebook.Login.LoginManager.Instance.LogInWithReadPermissions(activity, new List<string> { "public_profile", "email" });
        }

        #region Eventos disparados por la GMS Tasks
        //IOnFailureListener
        public void OnFailure(Java.Lang.Exception e)
        {
            _onLoginComplete.Invoke(null, e.Message);
        }

        //IFacebookCallback
        public void OnSuccess(Java.Lang.Object result)
        {
            MainActivity.fire_impl.FirebaseAuthRegister(null, MainActivity.REQC_FACEBOOK_SIGN_IN);
            _onLoginComplete.Invoke(new UserProfile
            {
                Name="",
                Email="",
                ProfilePictureUrl = ""
            }, string.Empty);
        }
        #endregion

        #region Eventos disparados por la SDK de facebook
        //IFacebookCallback
        public void OnCancel()
        {
            _onLoginComplete.Invoke(null, "Canceled");
        }

        //IFacebookCallback
        public void OnError(FacebookException error)
        {
            _onLoginComplete.Invoke(null, error.Message);
        }
        #endregion

    }
}