using Android.Gms.Tasks;
using Firebase.Functions;
using Java.Util;
using Newtonsoft.Json;
using PetArmy.Interfaces;
using PetArmy.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

[assembly: Dependency(typeof(PetArmy.Droid.Implementations.FirebaseFunction))]
namespace PetArmy.Droid.Implementations
{
    public class FirebaseFunction: Java.Lang.Object, IFireFunction, IOnCompleteListener
    {

        Action<Object, string> _onCallComplete;     

        public void CallFunction(string function, CreateAdminUserRequest data,Action<Object, string> _onCallComplete)
        {
            var json_data = JsonConvert.SerializeObject(data);
            this._onCallComplete = _onCallComplete;
            var lc_fn = FirebaseFunctions.Instance.GetHttpsCallable(function).Call(json_data);
            lc_fn.AddOnCompleteListener(this);
        }
        /// <summary>
        /// Listener para terminacion de llamado a funciones
        /// </summary>
        /// <param name="task"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnComplete(Task task)
        {
            //Problemas en la llamada
            if (task.Exception != null) {
                _onCallComplete?.Invoke(null, task.Exception.Message);
            }
            else
            {
                var result = task.Result;
                _onCallComplete?.Invoke(result, string.Empty);
            }
        }
    }
}