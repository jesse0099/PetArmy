using PetArmy.Interfaces;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;
using PetArmy.Models;

namespace PetArmy.ViewModels
{
    public class AdminViewModel: BaseViewModel
    {
        IFireFunction _i_function;

        private ICommand _iCreateAdminCommand;

        public ICommand CreateAdminCommand
        {
            get { return _iCreateAdminCommand; }
            set { _iCreateAdminCommand = value; 
                OnPropertyChanged();
            }
        }


        public AdminViewModel()
        {   
            _instance = this;
            _i_function = DependencyService.Get<IFireFunction>();
            _iCreateAdminCommand = new Command(CreateAdminExecute);
        }

        void CreateAdminExecute() {
            var _data = new CreateAdminUserRequest()
            {
                email="radagast_r@gmail.com",
                firstName= "radagastr",
                lastName="01r",
                password="123456",
                role="admin"
            };

            _i_function.CallFunction("createUser", _data, (object response, string message) =>
            {
                FunctionCallChecker(response, message);
            });
        }

        public void FunctionCallChecker(object response, string message)
        {
            if (response != null)
            {

            }
            else
            {
                ErrorTitle = "Something Went Wrong!!!";
                ErrorMessage = message;
                OpenPopUp = true;
            }
        }


        private static AdminViewModel _instance;
        public static AdminViewModel GetInstance()
        {
            if (_instance == null)
                return new AdminViewModel();
            else
                return _instance;

        }
    }
}
