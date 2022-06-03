using System.Threading.Tasks;

namespace PetArmy.Interfaces
{
    public interface IFirebaseAut
    {
        Task<string> LoginWithEmailAndPassword(string email, string password);
        bool SignOut();
        bool IsSignIn();
    }
}
