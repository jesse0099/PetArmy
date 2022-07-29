using PetArmy.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class ConfirmAdoptionRequestViewModel : BaseViewModel
    {
        private IFirebaseAuth _i_auth;

        private bool _openConfirmationPopUp;

        public bool OpenConfirmationPopUp
        {
            get { return _openConfirmationPopUp; }
            set { _openConfirmationPopUp = value; 
                OnPropertyChanged();
            }
        }

        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value;
                OnPropertyChanged();
            }
        }

        private string _phoneNumber;

        public  string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; 
                OnPropertyChanged();
            }
        }

        private string  _email;

        public string  Email
        {
            get { return _email; }
            set { _email = value;
                OnPropertyChanged();
            }
        }

        private ICommand _cancelCommand;

        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
            set { _cancelCommand = value;
                OnPropertyChanged();   
            }
        }

        private ICommand _confirmCommand;

        public ICommand ConfirmCommand
        {
            get { return _confirmCommand; }
            set { _confirmCommand = value;
                OnPropertyChanged();
            }
        }


        public ConfirmAdoptionRequestViewModel()
        {
            _instance = this;
            _i_auth = DependencyService.Get<IFirebaseAuth>();
            CancelCommand = new Command(() => {
                FeedViewModel.GetInstance().OpenConfirmationPopUp = false;
            });
            ConfirmCommand = new Command(SendAdoptionRequest);
            OpenConfirmationPopUp  = false;
            GetCurrentUserInfo();
        }

        async public void SendAdoptionRequest()
        {
            IsBusy = true;
            try
            {
                //Insert en BD
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void GetCurrentUserInfo() {
            var current_user_info = _i_auth.GetSignedUserProfile();
            Email = current_user_info.Email;
            PhoneNumber = current_user_info.PhoneNumber;
            FullName = current_user_info.Name;
        }

        private static ConfirmAdoptionRequestViewModel _instance;
        public static ConfirmAdoptionRequestViewModel GetInstance()
        {
            return _instance ??= _instance = new ConfirmAdoptionRequestViewModel();    
        }
    }
}
