using Android.App;
using Android.Gms.Tasks;
using Android.OS;
using Org.Json;
using PetArmy.Droid.Implementations;
using PetArmy.Interfaces;
using PetArmy.Models;
using System;
using System.Collections.Generic;
using Xamarin.Facebook;
using Xamarin.Forms;
using static Xamarin.Facebook.GraphRequest;

[assembly: Xamarin.Forms.Dependency(typeof(PetArmy.Droid.FacebookLoginActivity))]
namespace PetArmy.Droid
{
    [Activity(Label = "FacebookLoginActivity")]
    public class FacebookLoginActivity : Java.Lang.Object, IFacebookAuth, IFacebookCallback, IOnFailureListener, IGraphJSONObjectCallback, IOnCompleteListener
    {
        public string _registered_email = "No Email";
        public Action<UserProfile, string> _onLoginComplete;
        public FacebookLoginActivity()
        {
            _instance = this;
        }

        [Obsolete]
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
            _onLoginComplete?.Invoke(null, e.Message);
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
                //Getting user email
                GraphRequest request = GraphRequest.NewMeRequest(AccessToken.CurrentAccessToken, this);
                Bundle parameters = new Bundle();
                parameters.PutString("fields", "id,name,link,email");
                request.Parameters = parameters;
                request.ExecuteAsync();
            }
        }
        #endregion

        #region Eventos disparados por la SDK de facebook
        //IFacebookCallback
        public void OnCancel()
        {
            _onLoginComplete?.Invoke(null, "Canceled");
        }

        //IFacebookCallback
        public void OnError(FacebookException error)
        {
            _onLoginComplete?.Invoke(null, error.Message);
        }

        //IFacebookCallback
        public void OnSuccess(Java.Lang.Object result)
        {
            //Successful Sign In
            //Attempting to register Facebook Account
            FirebaseAuthentication.GetInstance().FirebaseAuthRegister(null, MainActivity.REQC_FACEBOOK_SIGN_IN).AddOnCompleteListener(this);
        }

        //IGraphJSONObjectCallback
        public void OnCompleted(JSONObject @object, GraphResponse response)
        {
            try
            {
                _registered_email = response.JSONObject.Get("email").ToString();
            }
            catch (Exception e)
            {
                _registered_email = e.Message;
            }
            finally
            {
                //Successful Sign Up
                _onLoginComplete?.Invoke(new UserProfile
                {
                    Name = Profile.CurrentProfile.Name,
                    Email = _registered_email,
                    ProfilePictureUrl = string.Empty
                }, string.Empty);
            }
        }
        #endregion

        #region Singleton
        private static FacebookLoginActivity _instance;
        public static FacebookLoginActivity GetInstance()
        {
            if (_instance == null)
                return new FacebookLoginActivity();
            else
                return _instance;

        }
        #endregion
    }
}