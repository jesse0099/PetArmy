using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using PetArmy.Services;
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

        public void initCommands()
        {
            GetAllMascotas = new Command(getMascotas);
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


    }
}
