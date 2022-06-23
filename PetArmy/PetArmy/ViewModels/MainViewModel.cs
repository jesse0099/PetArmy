using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PetArmy.ViewModels
{
    public  class MainViewModel: BaseViewModel
    {
        public LoginViewModel Login { get; set; }
        public UserViewModel User { get; set; }
        public RegisterViewModel Register { get; set; }

        public MainViewModel(): base()
        {
            this.Login = LoginViewModel.GetInstance();
            this.Register = RegisterViewModel.GetInstance();
            this.User = new UserViewModel();
        }

    }
}
