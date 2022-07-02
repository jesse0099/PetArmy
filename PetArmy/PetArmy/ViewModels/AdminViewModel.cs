using PetArmy.Helpers;
using PetArmy.Interfaces;
using PetArmy.Models;
using PetArmy.Models.CloudFuntionsCalls;
using Resx;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private ObservableCollection<AdminAccountRequest> _adminAccountRequests;
        public ObservableCollection<AdminAccountRequest> AdminAccountRequests
        {
            get{ return _adminAccountRequests; }
            set{ _adminAccountRequests = value; 
                OnPropertyChanged();
            }
        }

        IFireFunction _i_function;

        private ICommand _iApproveAdminRequestCommand;

        public ICommand ApproveAdminRequestCommand
        {
            get { return new Command((e) => {
                ApproveAdminRequestExecute(e as AdminAccountRequest);
            }); }
            set { _iApproveAdminRequestCommand = value;
                OnPropertyChanged();
            }
        }


        public AdminViewModel()
        {
            _instance = this;
            _i_function = DependencyService.Get<IFireFunction>();
            _iApproveAdminRequestCommand = new Command<AdminAccountRequest>(ApproveAdminRequestExecute);
        }
        /// <summary>
        /// Llamado a la funcion ApproveAdminAccount
        /// </summary>
        void ApproveAdminRequestExecute(AdminAccountRequest _data) {
            _i_function.ApproveAdminAccount(Commons.AdminCreationApprovalFunction, _data, (object response, string message) =>
            {
                ApproveFunctionCallChecker(response, message);
            });
        }
        /// <summary>
        /// Llamado a la funcion GetAdminAccountRequests
        /// </summary>
        public void GetAdminAccountRequests()
        {
            _i_function.GetAdminAccountRequests(Commons.AdminCreationRequestsFunction, (object response, string message) => {
                RequestsFunctionCallChecker(response, message);
            });
        }
        /// <summary>
        ///  Revision de respuesta al llamado de la funcion GetAdminRequests
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public void RequestsFunctionCallChecker(object response, string message)
        {
            if (response != null){
                AdminAccountRequests = new ObservableCollection<AdminAccountRequest>(((List<AdminAccountRequest>)response));
            }else
            {
                ErrorTitle = AppResources.SomethingWrong;
                ErrorMessage = message;
                OpenPopUp = true;
            }
        }
        /// <summary>
        /// Revision de respuesta al llamado de la funcion ApproveAdminRequest
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public void ApproveFunctionCallChecker(object response, string message)
        {
            if (response != null){
                return;
            }else
            {
                ErrorTitle = AppResources.SomethingWrong;
                ErrorMessage = message;
                OpenPopUp = true;
            }
        }

        private static AdminViewModel _instance;
        public static AdminViewModel GetInstance()
        {
            return _instance ??= _instance = new AdminViewModel();
        }
    }
}
