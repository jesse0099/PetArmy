using PetArmy.Helpers;
using PetArmy.Interfaces;
using PetArmy.Models;
using PetArmy.Models.CloudFuntionsCalls;
using Resx;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        //Bound properties
        private bool _emailError;
        public bool EmailError { get { return _emailError; } set { _emailError = value; OnPropertyChanged(); } }

        private string _emailErrorLabel;
        public string EmailErrorLabel { get { return _emailErrorLabel; } set { _emailErrorLabel = value; OnPropertyChanged(); } }

        private bool _passwordError;
        public bool PasswordError { get { return _passwordError; } set { _passwordError = value; OnPropertyChanged(); } }

        private string _passwordErrorLabel;
        public string PasswordErrorLabel { get { return _passwordErrorLabel; } set { _passwordErrorLabel = value; OnPropertyChanged(); } }


        private bool _repeatPasswordError;
        public bool RepeatPasswordError { get { return _repeatPasswordError; } set { _repeatPasswordError = value; OnPropertyChanged(); } }

        private string _repeatPasswordErrorLabel;
        public string RepeatPasswordErrorLabel { get { return _repeatPasswordErrorLabel; } set { _repeatPasswordErrorLabel = value; OnPropertyChanged(); } }

        IFirebaseAuth _i_auth;
        IFireFunction _i_function;



        private string _Email;
        public string Email
        {
            get { return _Email; }
            set
            {
                _Email = value;
                OnPropertyChanged();
                ValidateEmail();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
                ValidatePassword();
            }
        }

        private string _repeatPassword;
        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set
            {
                _repeatPassword = value;
                OnPropertyChanged();
                ValidateRepeatPassword();
            }
        }


        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private string _phone_number;
        public string PhoneNumber
        {
            get { return _phone_number; }
            set
            {
                _phone_number = value;
                OnPropertyChanged();
            }
        }


        private bool _isAdminRequest;
        public bool IsAdminRequest
        {
            get { return _isAdminRequest; }
            set
            {
                _isAdminRequest = value;
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

            PhoneNumber = Commons.DefaultPhoneNumber;
            IsBusy = false;
            IsAdminRequest = false;
        }

        private void OnRegisterExecute()
        {
            IsBusy = true;
            if (IsAdminRequest)
            {
                OnRequestCreateAdmin();
            }
            else
            {
                OnUserRegister();
            }
        }

        private void OnUserRegister()
        {
            var isValid = OnUserRegisterValidation();
            if (!isValid)
                return;
            //Final de las validaciones
            _i_auth.RegisterWithEmailAndPassword(Email, Password, (UserProfile profile, string message) =>
            {
                RegisterChecker(profile, message);
            });
        }

        private void OnRequestCreateAdmin()
        {
            var isValid = OnRequestCreateAdminValidation();
            if (!isValid)
                return;
            //Final de las validaciones
            var _data = new CreateAdminUserFunctionRequest()
            {
                email = Email,
                firstName = FirstName,
                lastName = LastName,
                password = Password,
                phoneNumber = PhoneNumber,
                role = "admin"
            };

            _i_function.RequestAdminAccount(Commons.AdminCreationRequestFunction, _data, (object response, string message) =>
            {
                FunctionCallChecker(response, message);
            });
        }

        private void ValidateEmail()
        {
            if (Email != null)
            {
                if (Email == string.Empty)
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

        private void ValidatePassword()
        {
            if (Password != null)
            {
                if (Password == string.Empty)
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

                if (!Password.Equals(RepeatPassword))
                {
                    RepeatPasswordError = true;
                    RepeatPasswordErrorLabel = AppResources.PasswordNoMatch;
                    return;
                }
                else
                {
                    RepeatPasswordError = false;
                    RepeatPasswordErrorLabel = string.Empty;
                }

            }
            else
            {
                PasswordError = false;
                PasswordErrorLabel = string.Empty;
            }
        }

        private void ValidateRepeatPassword()
        {
            if (RepeatPassword != null)
            {
                if (RepeatPassword == string.Empty)
                {
                    RepeatPasswordError = true;
                    RepeatPasswordErrorLabel = AppResources.PasswordIsEmpty;
                    return;
                }

                if (!RepeatPassword.Equals(Password))
                {
                    RepeatPasswordError = true;
                    RepeatPasswordErrorLabel = AppResources.PasswordNoMatch;
                    return;
                }

                if (!Commons.IsValidPassword(RepeatPassword))
                {
                    RepeatPasswordError = true;
                    RepeatPasswordErrorLabel = AppResources.PasswordNonMinimumSize;
                    return;
                }

                RepeatPasswordError = false;
                RepeatPasswordErrorLabel = string.Empty;
            }
            else
            {
                RepeatPasswordError = false;
                RepeatPasswordErrorLabel = string.Empty;
            }
        }

        private bool OnUserRegisterValidation()
        {
            var fields = new List<string> { Email, Password };
            var empty_field = fields.FindAll(x => x == null || x == string.Empty);
            if (empty_field.Count != 0)
            {
                RegisterChecker(null, AppResources.AllFieldsRequired);
                return false;
            }

            //Error con el email
            if (EmailError)
            {
                RegisterChecker(null, EmailErrorLabel);
                return false;
            }

            //Error con el password
            if (PasswordError)
            {
                RegisterChecker(null, PasswordErrorLabel);
                return false;
            }

            //Error con el password repetido
            if (RepeatPasswordError)
            {
                RegisterChecker(null, RepeatPasswordErrorLabel);
                return false;
            }
            return true;
        }

        private bool OnRequestCreateAdminValidation()
        {
            var fields = new List<string> { Email, FirstName, LastName, Password, PhoneNumber };

            var empty_field = fields.FindAll(x => x == null || x == string.Empty);
            if (empty_field.Count != 0)
            {
                FunctionCallChecker(null, AppResources.AllFieldsRequired);
                return false;
            }

            //Error con el email
            if (EmailError)
            {
                FunctionCallChecker(null, EmailErrorLabel);
                return false;
            }

            //Error con el password
            if (PasswordError)
            {
                FunctionCallChecker(null, PasswordErrorLabel);
                return false;
            }

            //Error con el password repetido
            if (RepeatPasswordError)
            {
                FunctionCallChecker(null, RepeatPasswordErrorLabel);
                return false;
            }

            //Error con el telefono
            if (!Commons.IsValidPhone(PhoneNumber))
            {
                FunctionCallChecker(null, AppResources.NonValidPhoneNumber);
                return false;
            }

            return true;
        }

        public void FunctionCallChecker(object response, string message)
        {
            IsBusy = false;
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

        async private void RegisterChecker(UserProfile profile, string message)
        {
            IsBusy = false;
            if (profile != null)
            {
                _i_auth.SignOut();
                //Success PopUp ()
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                ErrorTitle = AppResources.SomethingWrong;
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
