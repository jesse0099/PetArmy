using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.ViewModels
{
    public  class MainViewModel: BaseViewModel
    {
        public LoginViewModel Login { get; set; }
        public UserViewModel User { get; set; }

        public MainViewModel()
        {
            this.Login = LoginViewModel.GetInstance();
            this.User = new UserViewModel();
        }

    }
}
