﻿using PetArmy.Models;
using System;

namespace PetArmy.Interfaces
{
    public interface IFirebaseAuth
    {
        void RegisterWithEmailAndPassword(string email, string password, Action<UserProfile,string> onRegisterComplete);
        void LoginWithEmailAndPassword(string email, string password, Action<UserProfile, string> onLoginComplete);
        UserProfile GetSignedUserProfile();
        bool SignOut();
        bool IsSignIn();
    }
}
