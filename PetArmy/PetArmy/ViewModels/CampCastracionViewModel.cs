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

        private static CampCastracionViewModel _instance;
        public static CampCastracionViewModel GetInstance()
        {
            if (_instance == null)
                return new CampCastracionViewModel();
            else
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
        public ICommand UpdateCampCastra { get; set; }
        public ICommand DeleteCampCastra { get; set; }

        //I have to uncomment that shit below
        public void initCommands()
        {
            getAllCampCastra = new Command(getCampCastra);
            AddCampCastra = new Command(addCampCastra);
            //UpdateCampCastra = new Command(updateCampCastra);
            //DeleteCampCastra = new Command(deleteCampCastra);

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
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AddCampCastraView());
            }
            catch (System.Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Couldn't open 'Add Campaña Castración'", e.ToString(), "Ok");
            }
        }
    }
}
