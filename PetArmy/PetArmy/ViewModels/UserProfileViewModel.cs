using PetArmy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class UserProfileViewModel: BaseViewModel
    {
        #region Singleton
        public static UserProfileViewModel instance = null;

        public static UserProfileViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new UserProfileViewModel();
            }
            return instance;
        }

        public static void DisposeInstance()
        {
            if (instance != null)
            {
                instance = null;
            }
        }

        public UserProfileViewModel()
        {
            initClass();
            initCommands();
        }

        public void initClass()
        {
            _i_auth = DependencyService.Get<IFirebaseAuth>();
        }

        public void initCommands()
        {
            
        }
        #endregion

        #region Variables 

        IFirebaseAuth _i_auth;

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }


        #endregion

        #region Commands and Functions


        public async Task setUserInfo()
        {
            var registered_user = _i_auth.GetSignedUserProfile();
            Email = registered_user.Email;
        }

        #endregion

    }
}
