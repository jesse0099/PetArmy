using PetArmy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Interfaces
{
    public interface IFireFunction
    {
        void ApproveAdminAccount(string function, CreateAdminUserRequest data, Action<Object, string> _onCallComplete);
        void RequestAdminAccount(string function, CreateAdminUserRequest data, Action<Object, string> _onCallComplete);
    }
}
