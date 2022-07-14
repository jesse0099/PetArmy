using PetArmy.Helpers;
using PetArmy.Interfaces;
using PetArmy.Models;
using PetArmy.Views;
using Resx;
using System.Collections.Generic;
using Xamarin.Forms;
using PetArmy.Helpers;

namespace PetArmy.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        //Bound properties
        private bool _emailError;
        public bool EmailError { get { return _emailError;} set { _emailError = value; OnPropertyChanged(); } }
        
        private string _emailErrorLabel;
        public string EmailErrorLabel { get { return _emailErrorLabel; } set { _emailErrorLabel = value; OnPropertyChanged(); } }
        
        private bool _passwordError;
        public bool PasswordError { get { return _passwordError; } set { _passwordError = value; OnPropertyChanged(); } }
        
        private string _passwordErrorLabel;
        public string PasswordErrorLabel { get { return _passwordErrorLabel; } set { _passwordErrorLabel = value; OnPropertyChanged(); } }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; 
                OnPropertyChanged();
                ValidateEmail();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value;
                OnPropertyChanged();
                ValidatePassword();
            }
        }

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

            IsBusy = false;
            EmailError = false;
            PasswordError = false;
        }

        private void ValidateEmail() { 
            if(Email != null)
            {   if(Email == string.Empty)
                {
                    EmailError = true;
                    EmailErrorLabel = AppResources.EmailIsEmpty;
                    return;
                }

                if (!Commons.IsValidEmail(Email))
                {
                    EmailError = true;
                    EmailErrorLabel = AppResources.BadFormattedEmail;
                    return;
                }
                EmailError = false;
                EmailErrorLabel = string.Empty;
            }
            else
            {
                EmailErrorLabel = string.Empty;
                EmailError = false;
            }
        }

        private void ValidatePassword() { 
            if(Password != null)
            {
                if(Password == string.Empty)
                {
                    PasswordError = true;
                    PasswordErrorLabel = AppResources.PasswordIsEmpty;
                    return;
                }

                if (!Commons.IsValidPassword(Password))
                {
                    PasswordError = true;
                    PasswordErrorLabel = AppResources.PasswordNonMinimumSize;
                    return;
                }
                PasswordError = false;
                PasswordErrorLabel = string.Empty;
            }
            else
            {
                PasswordError = false;
                PasswordErrorLabel = string.Empty;
            }
        }

        public void RefreshUiState()
        {
            Email = null;
            Password = null;
        }

        private void OnLoginEPassExecute()
        {
            IsBusy = true;
            var fields = new List<string> { Email, Password};
            //Email o password nulos o vacios
            var empty_field = fields.FindAll(x => x == null || x == string.Empty);
            if (empty_field.Count != 0)
            {
                ProviderLoginChecker(null, AppResources.AllFieldsRequired);
                return;
            }

            //Error en el Email
            if (EmailError)
            {
                ProviderLoginChecker(null, EmailErrorLabel);
                return;
            }

            //Error en la password
            if (PasswordError)
            {
                ProviderLoginChecker(null, PasswordErrorLabel);
                return;
            }
                
            //Final de las validaciones
            _i_auth.LoginWithEmailAndPassword(Email, Password, (UserProfile profile, string message) =>
            {
                ProviderLoginChecker(profile, message);
            });
        }

        private void OnLoginGoogleExecute()
        {   IsBusy = true;
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
            IsBusy = false;
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

                            Settings.IsAdmin = true;
                            await Shell.Current.GoToAsync("//AdminLandingPage");
                            break;
                        }
                    case "sa":
                        {
                            if (Shell.Current == null)
                                Application.Current.MainPage = new AppShell();

                            Settings.IsAdmin = true;
                            await Shell.Current.GoToAsync("//AdminLandingPage");
                            break;
                        }
                    default:
                        {
                            if (Shell.Current == null)
                                Application.Current.MainPage = new AppShell();

                            Settings.IsAdmin = false;
                            await Shell.Current.GoToAsync("//AboutPage");
                            break;
                        }
                }
            }
            else
            {
                ErrorTitle = AppResources.SomethingWrong;
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
