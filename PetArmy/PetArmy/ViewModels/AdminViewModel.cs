using PetArmy.Interfaces;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;

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
            var _data = new Dictionary<string,object>();
            _data.Add("email","eva01@gmail.com");
            _data.Add("password","123456");
            _data.Add("role","admin");
            _data.Add("firstName","eva01");
            _data.Add("lastName","01");
            _i_function.CallFunction("createUser", _data, (object response, string message) =>
            {
                
            });
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
