using Android.Gms.Tasks;
using Firebase.Auth;
using PetArmy.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(PetArmy.Droid.Implementations.FirebaseAuthentication))]
namespace PetArmy.Droid.Implementations
{
    public class FirebaseAuthentication : Java.Lang.Object, IFirebaseAuth, FirebaseAuth.IAuthStateListener, IOnSuccessListener
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

        public void OnAuthStateChanged(FirebaseAuth auth)
        {
            if(auth.CurrentUser != null)
            {
                var token = auth.CurrentUser.GetIdToken(true).AddOnSuccessListener(this);
                
            }
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            Java.Lang.Object hasura_claims;
            var claims = ((GetTokenResult)result).Claims;

            claims.TryGetValue("https://hasura.io/jwt/claims", out hasura_claims);
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

        
    }
}