using PetArmy.Interfaces;
using PetArmy.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class RegisterViewModel: BaseViewModel
    {
        IFirebaseAuth _i_auth;
        IFireFunction _i_function;

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value;
                   OnPropertyChanged();
            }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value;
                OnPropertyChanged();
            }
        }

        private ICommand _iRequestCreateAdminCommand;

        public ICommand RequestCreateAdminCommand
        {
            get { return _iRequestCreateAdminCommand; }
            set
            {
                _iRequestCreateAdminCommand = value;
                OnPropertyChanged();
            }
        }

        public Command RegisterCommand { get; set; }

        public RegisterViewModel()
        {
            _instance = this;

            _i_auth = DependencyService.Get<IFirebaseAuth>();
            _i_function = DependencyService.Get<IFireFunction>();

            RegisterCommand = new Command(OnRegisterExecute);
            RequestCreateAdminCommand = new Command(OnRequestCreateAdminExecute);

            Email = string.Empty;
            Password = string.Empty;
        }

        private void OnRegisterExecute()
        {
            if (Email == string.Empty || Password == string.Empty)
            {
                RegisterChecker(null, "All Fields Are Required");
                return;
            }

            _i_auth.RegisterWithEmailAndPassword(Email, Password, (UserProfile profile, string message) =>
            {
                RegisterChecker(profile, message);
            });
        }

        private void OnRequestCreateAdminExecute() {
            var _data = new CreateAdminUserRequest()
            {
                email = Email,
                firstName = "",
                lastName = "",
                password = Password,
                role = "admin"
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

        async private void RegisterChecker(UserProfile profile, string message)
        {
            if (profile != null)
            {
                _i_auth.SignOut();
                //Success PopUp ()
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                ErrorTitle = "Something Went Wrong!!!";
                ErrorMessage = message;
                OpenPopUp = true;
            }
        }

        private static RegisterViewModel _instance;
        public static RegisterViewModel GetInstance()
        {
            if (_instance == null)
                return new RegisterViewModel();
            else
                return _instance;

        }
    }
}
