using System;
using PetArmy.Models;
using PetArmy.Models.CloudFuntionsCalls;

namespace PetArmy.Interfaces
{
    public interface IFireFunction
    {
        void ApproveAdminAccount(string function, AdminAccountRequest data, Action<Object, string> _onCallComplete);
        void RejectAdminAccount(string function, AdminAccountRequest data, Action<Object, string> _onCallComplete);
        void RequestAdminAccount(string function, CreateAdminUserFunctionRequest data, Action<Object, string> _onCallComplete);
        void UpdateAdminAccountAccessState(string function, UpdateAdminAccountAccessRequest data, Action<Object, string> _onCallComplete);
        void GetAdminAccountRequests(string function, Action<Object, string> _onCallComplete);
    }
}
