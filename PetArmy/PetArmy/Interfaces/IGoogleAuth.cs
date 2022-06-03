using PetArmy.Models;
using System;

namespace PetArmy.Interfaces
{
    public interface IGoogleAuth
    {
        void Login(Action<UserProfile, string> onLoginComplete);
    }
}
