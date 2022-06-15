using PetArmy.Interfaces;
using PetArmy.Models;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class RegisterViewModel: BaseViewModel
    {
        IFirebaseAuth _i_auth;

        public string Email { get; set; }
        public string Password { get; set; }
        public Command RegisterCommand { get; set; }

        public RegisterViewModel()
        {
            _instance = this;

            _i_auth = DependencyService.Get<IFirebaseAuth>();

            RegisterCommand = new Command(OnRegisterExecute);

            Email = string.Empty;
            Password = string.Empty;
        }

        private void OnRegisterExecute()
        {
            if (Email.Equals(string.Empty) || Password.Equals(string.Empty))
            {
                RegisterChecker(null, "All Fields Are Required");
                return;
            }

            _i_auth.RegisterWithEmailAndPassword(Email, Password, (UserProfile profile, string message) =>
            {
                RegisterChecker(profile, message);
            });
        }

        async private void RegisterChecker(UserProfile profile, string message)
        {
            if (profile != null)
            {
                //Success PopUp ()
                LoginViewModel.GetInstance().Email = profile.Email;
                LoginViewModel.GetInstance().Password = string.Empty;
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
