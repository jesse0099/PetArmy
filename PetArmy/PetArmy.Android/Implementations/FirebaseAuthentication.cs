using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Tasks;
using Firebase.Auth;
using PetArmy.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Facebook;

[assembly: Xamarin.Forms.Dependency(typeof(PetArmy.Droid.Implementations.FirebaseAuthentication))]
namespace PetArmy.Droid.Implementations
{
    public class FirebaseAuthentication : Java.Lang.Object, IFirebaseAuth, FirebaseAuth.IAuthStateListener, IOnSuccessListener, IOnCompleteListener
    {

        public bool IsSignIn()
        {
            var user = Firebase.Auth.FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = user.User.GetIdToken(false);
                //return user.User.GetIdToken(false).Result.ToString();
                return user.User.Uid;
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
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
        ///     Registro de cuenta Google en Firebase Auth
        /// </summary>
        /// <param name="acct">Cuenta logueada</param>
        public void FirebaseAuthRegister(GoogleSignInAccount acct = null, int provider = MainActivity.REQC_GOOGLE_SIGN_IN)
        {
            if (provider == MainActivity.REQC_GOOGLE_SIGN_IN)
            {
                AuthCredential credential = GoogleAuthProvider.GetCredential(acct.IdToken, null);
                FirebaseAuth.Instance.SignInWithCredential(credential).AddOnCompleteListener(this);
            }
            else
            {
                AuthCredential credential = FacebookAuthProvider.GetCredential(AccessToken.CurrentAccessToken.Token);
                FirebaseAuth.Instance.SignInWithCredential(credential).AddOnCompleteListener(this);
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
        ///  Listener de terminacion para el metodo SignInWithCredential
        /// </summary>
        /// <param name="task"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (!task.IsSuccessful)
                Console.WriteLine(task.Result.ToString());
        }

        /// <summary>
        ///  Listener de Exito  para el metodo GetIdToken
        /// </summary>
        /// <param name="result"></param>
        public void OnSuccess(Java.Lang.Object result)
        {
            Java.Lang.Object hasura_claims;
            IDictionary<string, Java.Lang.Object> claims;
            try
            {

                claims = ((GetTokenResult)result).Claims;
                claims.TryGetValue("https://hasura.io/jwt/claims", out hasura_claims);
                //Logica de guardado local de tokens
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

    }
}