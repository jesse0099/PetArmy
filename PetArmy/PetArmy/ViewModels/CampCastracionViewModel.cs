using PetArmy.Services;
using PetArmy.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class CampCastracionViewModel : BaseViewModel
    {

        public static CampCastracionViewModel _instance = null;
        public static CampCastracionViewModel GetInstance()
        {
            if (_instance == null)
            {

                _instance = new CampCastracionViewModel();

            }
            return _instance;
        }

        public CampCastracionViewModel()
        {
            initCommands();
            initClass();

        }

        public void initClass()
        {

        }

        public ICommand getAllCampCastra { get; set; }

        public ICommand AddCampCastra { get; set; }

        public void initCommands()
        {
            getAllCampCastra = new Command(getCampCastra);
            AddCampCastra = new Command(addCampCastra);
        }

        public async void getCampCastra()
        {
            try
            {
                await GraphQLService.getAllCampCastra();
            }
            catch
            {
            }
        }

        public async void addCampCastra()
        {
            /*
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AddCampCastraView());
            }
            catch (System.Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Couldn't open 'Add Campaña Castración'", e.ToString(), "Ok");
            }
            */
        }
    }
}