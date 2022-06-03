using PetArmy.Interfaces;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
           /* _i_auth = DependencyService.Get<IFirebaseAuthentication>();*/
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {

        }
    }
}
