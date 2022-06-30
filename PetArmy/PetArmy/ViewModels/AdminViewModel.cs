using PetArmy.Helpers;
using PetArmy.Interfaces;
using PetArmy.Models;
using Resx;
using System.Windows.Input;
using Xamarin.Forms;

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
                email="eva02@gmail.com",
                firstName= "eva",
                lastName="02",
                password="123456",
                role="admin"
            };

            _i_function.ApproveAdminAccount(Commons.AdminCreationApprovalFunction, _data, (object response, string message) =>
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
                ErrorTitle = AppResources.SomethingWrong;
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
