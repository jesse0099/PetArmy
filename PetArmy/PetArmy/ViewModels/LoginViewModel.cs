using PetArmy.Interfaces;
using PetArmy.Models;
using PetArmy.Views;
using Xamarin.Forms;
using PetArmy.Helpers;
using System.Threading.Tasks;
using PetArmy.Infraestructure;

namespace PetArmy.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        //Bound properties
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }    
        const string empty = "";
        private string _loggedMail;
        public string LoggedMail { get { return _loggedMail; }
                                   set { _loggedMail = value; OnPropertyChanged();} 
        }

        //Interfaces para login con proveedores externos
        IFirebaseAuth _i_auth;
        IGoogleAuth _g_auth;
        IFacebookAuth _f_auth;

        //Comandos para iniciar el proceso de login
        public Command LoginEPassCommand { get; }
        public Command LoginGoogleCommand { get; }
        public Command LoginFacebookCommand { get; }
        public Command RegisterCommand { get; }

        public LoginViewModel()
        {
            _instance = this;

            _i_auth = DependencyService.Get<IFirebaseAuth>();
            _g_auth = DependencyService.Get<IGoogleAuth>();
            _f_auth = DependencyService.Get<IFacebookAuth>();
            
            LoginEPassCommand = new Command(OnLoginEPassExecute);
            LoginGoogleCommand = new Command(OnLoginGoogleExecute);
            LoginFacebookCommand = new Command(OnLoginFacebookExecute);
            RegisterCommand = new Command(OnRegisterExecute);

            Email = string.Empty;
            Password = string.Empty;
            IsBusy = false;
        }

        private void OnLoginEPassExecute()
        {
            IsBusy = true;
            if (Email.Equals(string.Empty) || Password.Equals(string.Empty))
            {
                ProviderLoginChecker(null, "All Fields Are Required");
                return;
            }

            _i_auth.LoginWithEmailAndPassword(Email, Password, (UserProfile profile, string message) =>
            {
                ProviderLoginChecker(profile, message);
            });
        }

        private void OnLoginGoogleExecute()
        {   //IsBusy = true;
            _g_auth.Login((UserProfile profile, string message) => {
                ProviderLoginChecker(profile, message);
            });
        }

        private void OnLoginFacebookExecute()
        {
            IsBusy = true;
            _f_auth.Login((UserProfile profile, string message) =>
            {
                ProviderLoginChecker(profile, message);
            });
        }

        async private void OnRegisterExecute()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new RegisterPage());
        } 

        async public void ProviderLoginChecker(UserProfile profile, string message, string role = empty)
        {
            if (profile != null)
            {
                var registered_user = _i_auth.GetSignedUserProfile();
                LoggedMail = registered_user.Email;
                Settings.Email = registered_user.Email;
                Settings.UID = registered_user.Uid;
                Settings.Role = role;
   
                switch (role) {
                    case "admin":
                        {
                            if (Shell.Current == null)
                                Application.Current.MainPage = new AppShell();
                            IsBusy = false;
                            Settings.IsAdmin = true;
                            await Shell.Current.GoToAsync("//AdminLandingPage");
                            break;
                        }
                    case "sa":
                        {
                            if (Shell.Current == null)
                                Application.Current.MainPage = new AppShell();
                            IsBusy = false;
                            Settings.IsAdmin = true;
                            await Shell.Current.GoToAsync("//AdminLandingPage");
                            break;
                        }
                    default:
                        {
                            if (Shell.Current == null)
                                Application.Current.MainPage = new AppShell();
                            IsBusy = false;
                            Settings.IsAdmin = false;
                            await Shell.Current.GoToAsync("//AboutPage");
                            break;
                        }
                }
            }
            else
            {
                IsBusy = false;
                ErrorTitle = "Something Went Wrong!!!";
                ErrorMessage = message;
                OpenPopUp = true;
            }
        }

        private static LoginViewModel _instance;
        public static LoginViewModel GetInstance()
        {
            if (_instance == null)
                return new LoginViewModel();
            else
                return _instance;

        }

    }
}
