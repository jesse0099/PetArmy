using Android.Gms.Tasks;
using Android.Runtime;
using Firebase.Functions;
using Newtonsoft.Json;
using PetArmy.Interfaces;
using PetArmy.Models;
using PetArmy.Models.CloudFuntionsCalls;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

[assembly: Dependency(typeof(PetArmy.Droid.Implementations.FirebaseFunction))]
namespace PetArmy.Droid.Implementations
{
    public class FirebaseFunction : Java.Lang.Object, IFireFunction, IOnCompleteListener
    {

        Action<Object, string> _onCallComplete;
        
        public void ApproveAdminAccount(string function, AdminAccountRequest data, Action<Object, string> _onCallComplete)
        {
            var json_data = JsonConvert.SerializeObject(data);
            this._onCallComplete = _onCallComplete;
            var lc_fn = FirebaseFunctions.Instance.GetHttpsCallable(function).Call(json_data);
            lc_fn.AddOnCompleteListener(this);
        }
        
        public void RequestAdminAccount(string function, CreateAdminUserFunctionRequest data, Action<object, string> _onCallComplete)
        {
            var json_data = JsonConvert.SerializeObject(data);
            this._onCallComplete = _onCallComplete;
            var lc_fn = FirebaseFunctions.Instance.GetHttpsCallable(function).Call(json_data);
            lc_fn.AddOnCompleteListener(this);
        }

        public void RejectAdminAccount(string function, AdminAccountRequest data, Action<object, string> _onCallComplete)
        {
            var json_data = JsonConvert.SerializeObject(data);
            this._onCallComplete = _onCallComplete;
            var lc_fn = FirebaseFunctions.Instance.GetHttpsCallable(function).Call(json_data);
            lc_fn.AddOnCompleteListener(this);
        }

        public void UpdateAdminAccountAccessState(string function, UpdateAdminAccountAccessRequest data, Action<object, string> _onCallComplete)
        {
            var json_data = JsonConvert.SerializeObject(data);
            this._onCallComplete = _onCallComplete;
            var lc_fn = FirebaseFunctions.Instance.GetHttpsCallable(function).Call(json_data);
            lc_fn.AddOnCompleteListener(this);
        }

        public void GetAdminAccountRequests(string function, Action<object, string> _onCallComplete)
        {
            var lc_fn = FirebaseFunctions.Instance.GetHttpsCallable(function).Call();
            lc_fn.AddOnCompleteListener(GetAdminAccountRequestsListener.GetInstance(_onCallComplete));   
        }

        /// <summary>
        /// Listener para terminacion de llamado a la funcion GetAdminAccountRequestsExecute
        /// </summary>
        internal class GetAdminAccountRequestsListener : Java.Lang.Object, IOnCompleteListener
        {
            public static List<AdminAccountRequest> AdminCreationResponses;
            Action<object, string> _onCallComplete;

            public GetAdminAccountRequestsListener(Action<object, string> onCallComplete)
            {
                _onCallComplete = onCallComplete;
            }

            public void OnComplete(Task task){
                try{
                    if (task.Exception != null){
                        this._onCallComplete?.Invoke(null, task.Exception.Message);
                    }
                    else{
                        DateTime epochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        AdminCreationResponses = new List<AdminAccountRequest>();
                        var result = ((HttpsCallableResult)task.Result).Data;
                        JavaList result_list = ((JavaList)result);
                        object[] info_array = result_list.ToArray();
                        foreach (object document in info_array)
                        {
                            JavaDictionary dictionary = ((JavaDictionary)document);
                            JavaDictionary document_fields = ((JavaDictionary)dictionary["_fieldsProto"]);

                            var json_response = JsonConvert.SerializeObject(document_fields);
                            var creation_request = JsonConvert.DeserializeObject<AdminCreationResponse>(json_response);
                            var creation_request_user_details = creation_request.userDetails;
                            DateTime _createdOnDateTime = epochTime.AddSeconds(Convert.ToInt64(creation_request.createdOn.timestampValue.seconds)).ToLocalTime();
                            AdminCreationResponses.Add(new AdminAccountRequest()
                            {
                                _approvedBy = creation_request.approvedBy.stringValue,
                                _status = creation_request.status.stringValue,
                                _createdOn = _createdOnDateTime,
                                _docId = creation_request.docId.stringValue,
                                _motive = creation_request.motive.stringValue,
                                _accessGrantedBy = creation_request.accessGrantedBy.stringValue,
                                _enabled = creation_request.enabled.booleanValue,
                                _adminAccountDetail = new AdminAccountDetails()
                                {
                                    Email = creation_request_user_details.mapValue.fields.email.stringValue,
                                    FirstName = creation_request_user_details.mapValue.fields.firstName.stringValue,
                                    LastName = creation_request_user_details.mapValue.fields.lastName.stringValue,
                                    Password = creation_request_user_details.mapValue.fields.password.stringValue,
                                    PhoneNumber = creation_request_user_details.mapValue.fields.phoneNumber.stringValue,
                                    Role = creation_request_user_details.mapValue.fields.role.stringValue
                                },
                            });
                        }
                        this._onCallComplete?.Invoke(AdminCreationResponses, string.Empty);
                    }
                }
                catch (Exception e){

                    this._onCallComplete?.Invoke(null, e.Message);
                }
            }

            private static GetAdminAccountRequestsListener _instance;
            public static GetAdminAccountRequestsListener GetInstance(Action<object, string> onCompleteListener)
            {
                return _instance ??= new GetAdminAccountRequestsListener(onCompleteListener);
            }
        }

        /// <summary>
        /// Listener para terminacion de llamado a funciones: ApproveAdminAccount, RequestAdminAccount, RejectAdminAccount
        /// </summary>
        /// <param name="task"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnComplete(Task task){
            //Problemas en la llamada
            if (task.Exception != null) {
                _onCallComplete?.Invoke(null, task.Exception.Message);
            }
            else{
                _onCallComplete?.Invoke(task.Result, string.Empty);
            }
        }
    }
}