using PetArmy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Interfaces
{
    public interface IFacebookAuth
    {
        void Login(Action<UserProfile, string> onLoginComplete);
    }
}
