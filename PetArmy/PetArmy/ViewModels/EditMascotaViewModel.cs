using PetArmy.Models;
using PetArmy.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class EditMascotaViewModel : BaseViewModel
    {
        

        #region Singleton and Constructors
        public static EditMascotaViewModel instance = null;

        public static EditMascotaViewModel GetInstance()
        {
            return instance ??= (instance = new EditMascotaViewModel()); 
        }

        public static void DisposeInstance()
        {
            if (instance != null)
            {
                instance = null;
            }
        }

        public EditMascotaViewModel()
        {
            initCommands();
            initClass();
        }

        public void initClass()
        {

        }
        public void initCommands()
        {
            SaveMascota = new Command(updatePet);
            PickImage = new Command(pickImage);
        }


        #endregion


        #region variables and Commands
        private Mascota currPet;

        public Mascota CurrentPet
        {
            get { return currPet; }
            set { currPet = value; OnPropertyChanged(); } 
        }


        public ICommand SaveMascota { get; set; }
        public ICommand PickImage { get; set; }
        #endregion

        #region Functions
        public async void updatePet()
        {
            try
            {
                await MascotaService.updateMascota(currPet);
                await Shell.Current.GoToAsync("//ListMascotasPage");
            }
            catch (Exception)
            {
                throw;
            }

        }


        public async void pickImage()
        {
            try
            {

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


    }
}
