using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using PetArmy.Services;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;
using PetArmy.Models;

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
            NewMock = new Command(newMock);
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

        public ICommand NewMock { get; set; }


        public async void newMock()
        {
            try
            {
                MockString mockString = await GraphQLService.GetMockString().ConfigureAwait(false); ;
                
            }
            catch (System.Exception e)
            {
                await App.Current.MainPage.DisplayAlert("xxxxd", e.ToString(), "Ok");
            }

        }

        #endregion

    }
}
