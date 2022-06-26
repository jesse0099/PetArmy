using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Tasks;
using AndroidX.Collection;
using Firebase.Auth;
using PetArmy.Interfaces;
using PetArmy.Models;
using System;
using System.Collections.Generic;
using Xamarin.Facebook;

[assembly: Xamarin.Forms.Dependency(typeof(PetArmy.Droid.Implementations.FirebaseAuthentication))]
namespace PetArmy.Droid.Implementations
{
    public class FirebaseAuthentication : Java.Lang.Object, IFirebaseAuth, FirebaseAuth.IAuthStateListener, IOnSuccessListener, IOnCompleteListener
    {
        string _registered_email = string.Empty;
        public Action<UserProfile, string> _onRegisterComplete;
        public Action<UserProfile, string> _onLoginComplete;
        public FirebaseAuthentication()
        {
            _instance = this;
        }
        public bool IsSignIn()
        {
            var user = Firebase.Auth.FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public async void LoginWithEmailAndPassword(string email, string password, Action<UserProfile, string> onLoginComplete)
        {
            try
            {
                _onLoginComplete = onLoginComplete;

                //Attempting to Sign In
                await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                _onLoginComplete?.Invoke(null, e.Message);
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                _onLoginComplete?.Invoke(null, e.Message);
            }
        }

        public void RegisterWithEmailAndPassword(string email, string password, Action<UserProfile, string> onRegisterComplete)
        {
            _onRegisterComplete = onRegisterComplete;
            _registered_email = email;
            FirebaseAuthRegister(null, MainActivity.REQC_EMAILANDPASS_SIGN_IN, email, password).AddOnCompleteListener(this);
        }

        public bool SignOut()
        {
            try
            {
                FirebaseAuth.Instance.SignOut();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Registro de cuenta de usuario en Firebase Auth
        /// </summary>
        /// <param name="acct">Cuenta logueada</param>
        public Android.Gms.Tasks.Task FirebaseAuthRegister(GoogleSignInAccount acct = null, int provider = 0, string email = "", string password = "")
        {
            switch (provider)
            {
                case MainActivity.REQC_GOOGLE_SIGN_IN:
                    {
                        AuthCredential credential = GoogleAuthProvider.GetCredential(acct.IdToken, null);
                        return FirebaseAuth.Instance.SignInWithCredential(credential);
                    }                                                                                                                                                                                                                             
                case MainActivity.REQC_FACEBOOK_SIGN_IN:
                    {
                        AuthCredential credential = FacebookAuthProvider.GetCredential(AccessToken.CurrentAccessToken.Token);
                        return FirebaseAuth.Instance.SignInWithCredential(credential);
                    }
                case MainActivity.REQC_EMAILANDPASS_SIGN_IN:
                    {
                        return FirebaseAuth.Instance.CreateUserWithEmailAndPassword(email, password);
                    }
                default:
                    return null;
            }
        }

        #region Listeners
        /// <summary>
        /// Listener de cambios en el estado de la sesion    
        /// LLamado al metodo GetIdToken
        /// </summary>
        /// <param name="auth"></param>
        public void OnAuthStateChanged(FirebaseAuth auth)
        {
            if (auth.CurrentUser != null)
            {
                auth.CurrentUser.GetIdToken(true).AddOnSuccessListener(this);
            }
        }

        /// <summary>
        ///  Listener de Exito  para el metodo GetIdToken
        /// </summary>
        /// <param name="result"></param>
        public void OnSuccess(Java.Lang.Object result)
        {
            //Resultados de log in exitoso
            GetTokenResult token_result = ((GetTokenResult)result);
            //Claims - Firebase
            IDictionary<string, Java.Lang.Object> claims;
            //Claims - Hasura
            Java.Lang.Object hasura_claims;
            //Rol a evaluar
            string role = string.Empty;
            try
            {
                claims = token_result.Claims;
                claims.TryGetValue("https://hasura.io/jwt/claims", out hasura_claims);
                
                //Extraccion de rol para super admin (Deberia guardarse en el AppSettings junto al jwt)
                var is_super_admin = ((SimpleArrayMap)hasura_claims).Get("x-hasura-super_admin");
                var hasura_default_role = ((SimpleArrayMap)hasura_claims).Get("x-hasura-default-role");

                if (is_super_admin != null)
                    role = "sa";
                else
                    role = ((string)hasura_default_role);

                //Sucess Log In and Sign Up
                ViewModels.LoginViewModel.GetInstance().ProviderLoginChecker(new UserProfile()
                {
                    Name = FirebaseAuth.Instance.CurrentUser.DisplayName,
                    Email = FirebaseAuth.Instance.CurrentUser.Email,
                    ProfilePictureUrl = FirebaseAuth.Instance.CurrentUser.PhotoUrl != null ?
                                        FirebaseAuth.Instance.CurrentUser.PhotoUrl.ToString() : string.Empty,
                }, string.Empty, role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Listener de terminacion para el metodo RegisterWithEmailAndPassword
        /// </summary>
        /// <param name="task"></param>
        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            //Failed Sign Up
            if (task.Exception != null)
                _onRegisterComplete?.Invoke(null, task.Exception.Message);
            else
                _onRegisterComplete?.Invoke(new UserProfile()
                {
                    Name = _registered_email,
                    Email = _registered_email,
                    ProfilePictureUrl = string.Empty,
                }, string.Empty);
        }
        #endregion

        #region Singleton
        private static FirebaseAuthentication _instance;
        public static FirebaseAuthentication GetInstance()
        {
            if (_instance == null)
                return new FirebaseAuthentication();
            else
                return _instance;

        }

        public UserProfile GetSignedUserProfile()
        {
            if (FirebaseAuth.Instance.CurrentUser == null)
                return null;
            else
                return new UserProfile()
                {
                    Name = FirebaseAuth.Instance.CurrentUser.DisplayName,
                    Email = FirebaseAuth.Instance.CurrentUser.Email,
                    ProfilePictureUrl = FirebaseAuth.Instance.CurrentUser.PhotoUrl != null ?
                                        FirebaseAuth.Instance.CurrentUser.PhotoUrl.ToString() : string.Empty,
                };
        }
        #endregion
    }
}