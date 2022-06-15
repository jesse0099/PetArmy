using PetArmy.Models;
using System;
using System.Threading.Tasks;

namespace PetArmy.Interfaces
{
    public interface IFirebaseAuth
    {
        void RegisterWithEmailAndPassword(string email, string password, Action<UserProfile,string> onRegisterComplete);
        Task<string> LoginWithEmailAndPassword(string email, string password);
        bool SignOut();
        bool IsSignIn();
    }
}
