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
        }

        private void OnRegisterExecute()
        {
            _i_auth.RegisterWithEmailAndPassword(Email, Password, OnRegisterComplete);
        }

        async private void OnRegisterComplete(UserProfile profile, string message)
        {
            if (profile != null)
                await Shell.Current.GoToAsync("//AboutPage");
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
