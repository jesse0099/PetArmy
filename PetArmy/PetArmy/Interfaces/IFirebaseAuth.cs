using PetArmy.Models;
using System;
using System.Threading.Tasks;

namespace PetArmy.Interfaces
{
    public interface IFirebaseAuth
    {
        Task<string> LoginWithEmailAndPassword(string email, string password);
        bool SignOut();
        bool IsSignIn();
    }
}
