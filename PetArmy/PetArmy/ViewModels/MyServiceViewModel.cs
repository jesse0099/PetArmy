using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using PetArmy.Services;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;
using PetArmy.Models;
using PetArmy.Views;

namespace PetArmy.ViewModels
{
    public class MyServiceViewModel:BaseViewModel
    {

        #region Singleton
        public static MyServiceViewModel instance = null;

        public static MyServiceViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MyServiceViewModel();
            }
            return instance;
        }

        public void DeleteInstance()
        {
            if(instance != null)
            {
                instance = null;
            }
        }

        public MyServiceViewModel()
        {
            initCommands();
            initClass();
        }

        public void initClass()
        {

        }

        public void initCommands()
        {
            NewShelter = new Command(newShelter);
        }

        #endregion

        #region Variables

        private string _test = "Añadir lista de servicios";

        public string Test
        {
            get { return _test; }

            set
            {
                _test = value;
                OnPropertyChanged("Test");
            }
        }
        #endregion


        #region Commands and Funtions

        public ICommand NewShelter { get; set; }


        public async void newShelter()
        {
            try
            {
                /*await GraphQLService.GetMockString().ConfigureAwait(false);
                /* await GraphQLService.Insert_MockString("insertados").ConfigureAwait(false);*/
                /* await GraphQLService.Get_MockString_ById("s1").ConfigureAwait(false);*/

                await Application.Current.MainPage.Navigation.PushAsync(new NewShelterView());

            }
            catch (System.Exception e)
            {
                await App.Current.MainPage.DisplayAlert("xxxxd", e.ToString(), "Ok");
            }

        }

        #endregion

    }
}
