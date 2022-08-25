using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class YesNoPopUpViewModel: BaseViewModel
    {

        //Cancel Command
        public ICommand CancelCommand
        {
            get
            {
                return new Command(() => IsConfirmOpen = false);
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



        private string _headerTitle;

        public string HeaderTitle
        {
            get { return _headerTitle; }
            set { _headerTitle = value; 
                OnPropertyChanged();
            }
        }

        private bool _isConfirmOpen;

        public bool IsConfirmOpen
        {
            get { return _isConfirmOpen; }
            set { _isConfirmOpen = value;
                OnPropertyChanged();
            }
        }

        private string _bodyText;

        public string BodyText
        {
            get { return _bodyText; }
            set { _bodyText = value;
                OnPropertyChanged();
            }
        }

        private YesNoPopUpViewModel()
        {
            IsConfirmOpen = false;
        }



        #region Singleton

        private static YesNoPopUpViewModel _instance;
        public static YesNoPopUpViewModel GetInstance()
        {
            return _instance ??= _instance = new YesNoPopUpViewModel();
        }
        #endregion


    }
}
