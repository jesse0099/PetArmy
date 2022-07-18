using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using PetArmy.Services;
using PetArmy.Views;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class MascotaViewModel : BaseViewModel
    {


        private static MascotaViewModel _instance;
        public static MascotaViewModel GetInstance()
        {
            if (_instance == null)
                return new MascotaViewModel();
            else
                return _instance;

        }

        public MascotaViewModel()
        {
            initCommands();
            initClass();

        }

        public void initClass()
        {

        }

        public ICommand GetAllMascotas { get; set; }

        public ICommand AddMascota { get; set; }

        public void initCommands()
        {
            GetAllMascotas = new Command(getMascotas);
            AddMascota = new Command(addMascota);
        }

        public async void getMascotas()
        {
            try
            {
                await GraphQLService.getAllMascotas();
            }
            catch
            {
            }
        }

        public async void addMascota()
        {
            try
            {

                await Application.Current.MainPage.Navigation.PushAsync(new AddMascotaView());

            }
            catch (System.Exception e)
            {
                await App.Current.MainPage.DisplayAlert("xxxxd", e.ToString(), "Ok");
            }
        }

    }
}
