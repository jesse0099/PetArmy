using PetArmy.Helpers;
using PetArmy.Interfaces;
using PetArmy.Models;
using PetArmy.Models.CloudFuntionsCalls;
using Resx;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private BindingList<AdminAccountRequest> _adminAccountRequestsRaw;
        private BindingList<AdminAccountRequest> _adminAccountRequests;
        public BindingList<AdminAccountRequest> AdminAccountRequests
        {
            get{ return _adminAccountRequests; }
            set{ _adminAccountRequests = value;
                OnPropertyChanged();
            }
        }

        readonly IFireFunction _i_function;

        public ICommand ApproveAdminRequestCommand
        {
            get { 
                return new Command((e) => {
                    ApproveAdminRequestExecute(e as AdminAccountRequest);
                }); 
            }
        }

        public ICommand RejectAdminRequestCommand
        {
            get {
                return new Command((e) => {
                    RejectAdminRequestExecute(e as AdminAccountRequest);
                });
            }
        }

        private ICommand _iGetAdminRequestsCommand;
        public ICommand GetAdminRequestsCommand
        {
            get { return _iGetAdminRequestsCommand; }
            set { _iGetAdminRequestsCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand DisableAdminAccountCommand
        {
            get { 
                return new Command((e) => {
                    UpdateAdminAccountStateExecute(e as AdminAccountRequest, "disable");
                }); 
            }
        }

        public ICommand EnableAdminAccountCommand
        {
            get{
                return new Command((e) => {
                    UpdateAdminAccountStateExecute(e as AdminAccountRequest, "enable");
                });
            }
        }

        public ICommand SortByStatusCommand
        {
            get {
                return new Command<string>(e =>
                {
                    SortByStatusExecute(e);
                }); 
            }
        }

        private string _statusFilter;

        public string StatusFilter
        {
            get { return _statusFilter; }
            set { _statusFilter = value;
                if (value == string.Empty)
                    AdminAccountRequests = _adminAccountRequestsRaw;
                else
                    FilterStatusExecute(value);
            }
        }

        public AdminViewModel()
        {
            _instance = this;
            IsBusy = false;
            _i_function = DependencyService.Get<IFireFunction>();
            GetAdminRequestsCommand = new Command(GetAdminAccountRequestsExecute);
        }

        /// <summary>
        /// Llamado a la funcion ApproveAdminAccount
        /// </summary>
        void ApproveAdminRequestExecute(AdminAccountRequest _data) {
            IsBusy = true;
            _i_function.ApproveAdminAccount(Commons.AdminCreationApprovalFunction, _data, (object response, string message) =>
            {
                ApproveFunctionCallChecker(response, message, _data);
            });
        }
        /// <summary>
        /// Llamado a la funcion RejectAdminAccount
        /// </summary>
        /// <param name="_data"></param>
        void RejectAdminRequestExecute(AdminAccountRequest _data)
        {
            IsBusy = true;
            _i_function.RejectAdminAccount(Commons.AdminCreationRejectionFunction, _data, (object response, string message) =>
            {
                RejectFunctionCallChecker(response, message, _data);
            });
        }
        /// <summary>
        /// Llamado a la funcion GetAdminAccountRequestsExecute
        /// </summary>
        public void GetAdminAccountRequestsExecute()
        {
            IsBusy = true;
            _i_function.GetAdminAccountRequests(Commons.AdminCreationRequestsFunction, (object response, string message) => {
                RequestsFunctionCallChecker(response, message);
            });
        }
        public void UpdateAdminAccountStateExecute(AdminAccountRequest data, string action)
        {
            IsBusy = true;
            _i_function.UpdateAdminAccountAccessState(Commons.AdminAccessStateUpdateFunction, new UpdateAdminAccountAccessRequest() { 
                motive = "Testing",
                docId = data._docId,
                action = action,
                email = data._adminAccountDetail.Email,
            }, 
            (object response, string message) => {
                UpdateStateFunctionCallChecker(response, message, action, data);
            });
        }
        /// <summary>
        ///  Revision de respuesta al llamado de la funcion GetAdminRequests
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public void RequestsFunctionCallChecker(object response, string message)
        {
           IsBusy = false;
           if (response != null){
                AdminAccountRequests = new BindingList<AdminAccountRequest>(((List<AdminAccountRequest>)response));
                _adminAccountRequestsRaw = AdminAccountRequests;
           }
           else {
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
        public void ApproveFunctionCallChecker(object response, string message, AdminAccountRequest updated_item)
        {
            IsBusy = false;
            if (response != null){
                AdminAccountRequests.Where(x => x.Equals(updated_item)).First()._status = Commons.AdminRequestTreatedState;
                return;
            }else {
                // Already Treated Account
                if(message.Equals(Commons.AdminApprovalTreatedExceptionMessage) )
                    AdminAccountRequests.Where(x => x.Equals(updated_item)).First()._status = Commons.AdminRequestTreatedState;

                // Email already in use
                if(message.Equals(Commons.AdminApprovalEmailExceptionMessage))
                    AdminAccountRequests.Where(x => x.Equals(updated_item)).First()._status = Commons.AdminRequestRejectedState;

                ErrorTitle = AppResources.SomethingWrong;
                ErrorMessage = message;
                OpenPopUp = true;
            }
        }
        /// <summary>
        /// Revision de respuesta al llamado de la funcion  RejectAdminAccountRequest
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public void RejectFunctionCallChecker(object response, string message, AdminAccountRequest updated_item)
        {
            IsBusy = false;
            if (response != null){
                AdminAccountRequests.Where(x => x.Equals(updated_item)).First()._status = Commons.AdminRequestRejectedState;
                return;
            }else
            {
                // Already Treated Account
                if (message.Equals(Commons.AdminApprovalTreatedExceptionMessage))
                    AdminAccountRequests.Where(x => x.Equals(updated_item)).First()._status = Commons.AdminRequestRejectedState;
                ErrorTitle = AppResources.SomethingWrong;
                ErrorMessage = message;
                OpenPopUp = true;
            }
        }
        /// <summary>
        /// Revision de respuesta al llamado de la funcion  UpdateAdminAccountState
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public void UpdateStateFunctionCallChecker(object response, string message, string action, AdminAccountRequest updated_item)
        {
            IsBusy = false;
            if (response != null)
            {
                switch (action) {
                    case "enable":
                        {
                            AdminAccountRequests.Where(x => x.Equals(updated_item)).First()._enabled = true;
                            break;
                        }
                    case "disable":
                        {
                            AdminAccountRequests.Where(x => x.Equals(updated_item)).First()._enabled = false;
                            break;
                        }
                }
                return;
            }
            else
            {
                ErrorTitle = AppResources.SomethingWrong;
                ErrorMessage = message;
                OpenPopUp = true;
            }
        }
        /// <summary>
        /// Sort By Status 
        /// </summary>
        /// <param name="status"></param>
        void SortByStatusExecute(string status) {
            IsBusy = true;
            AdminAccountRequests = new BindingList<AdminAccountRequest>(_adminAccountRequestsRaw
                .Where(x => x._status.Equals(status)).ToList());
            IsBusy = false;
        }
        void FilterStatusExecute(string email) { 
            AdminAccountRequests = new BindingList<AdminAccountRequest>(AdminAccountRequests.Where(e => e._adminAccountDetail.Email.Contains(email)).ToList());
        }
        private static AdminViewModel _instance;
        public static AdminViewModel GetInstance()
        {
            return _instance ??= _instance = new AdminViewModel();
        }
    }
}

