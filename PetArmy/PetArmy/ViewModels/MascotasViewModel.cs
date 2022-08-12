using PetArmy.Helpers;
using PetArmy.Infraestructure;
using PetArmy.Models;
using PetArmy.Services;
using PetArmy.Views;
using Resx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class MascotasViewModel : BaseViewModel
    {

        #region Singleton
        private static MascotasViewModel _instance;
        public static MascotasViewModel GetInstance()
        {
            // esto es tuanis usenlo malditos
            return _instance ??= _instance = new MascotasViewModel();
        }
        #endregion


        private BindingList<Mascota> _mascotas;
        public BindingList<Mascota> Mascotas
        {
            get { return _mascotas; }
            set
            {
                _mascotas = value;
                OnPropertyChanged();
            }
        }

        #region Variables and Commands
        private MascotasViewModel()
        {
            initCommands();
            initClass();
        }

        public void initClass()
        {
            DefaultPetImage = String64Images.petDefault;
        }

        public void initCommands()
        {
            NewMascota = new Command(newMascota);
        }

        public ICommand NewMascota { get; set; }

        public ICommand EditMascota
        {
            get
            {
                return new Command((e) =>
                {
                    openEditMascota(e as Mascota);
                });
            }
        }


        public string DefaultPetImage { get; set; }

        #endregion





        #region Functions

        public async void newMascota()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AddMascotaView());
            }
            catch (System.Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Couldn't open 'Add Mascota'", e.ToString(), "Ok");
            }
        }

        public async Task getData()
        {
            Mascotas = new BindingList<Mascota>(await MascotaService.getAllMascotas(Settings.UID) as List<Mascota>);

            //UI Settings  
            List<Color> gradient = new();
            for (int i = 0; i < 5; i++)
            {
                Application.Current.Resources.TryGetValue($"Grad{i + 1}", out object color);
                gradient.Add((Color)color);
            }

            foreach (var pet in Mascotas)
            {
                var bool_values = new List<PetDbBools>()
                {
                    new PetDbBools() { Bool_Name = AppResources.Disability, Bool_Value = pet.discapacidad, Bool_Color = gradient[0] },
                    new PetDbBools() { Bool_Name = AppResources.Illness, Bool_Value = pet.enfermedad, Bool_Color = gradient[1]  },
                    new PetDbBools() { Bool_Name = AppResources.Allergies, Bool_Value = pet.alergias, Bool_Color = gradient[2]  },
                    new PetDbBools() { Bool_Name = AppResources.VaxxedKey, Bool_Value = pet.vacunado, Bool_Color = gradient[3]  },
                    new PetDbBools() { Bool_Name = AppResources.CastratedKey, Bool_Value = pet.castrado, Bool_Color = gradient[4]  },
                };
                pet.Db_Bools = bool_values.Where(x => x.Bool_Value).ToList();
            }
        }


        public async void openEditMascota(Mascota mascota)
        {
            try
            {
                IsBusy = true;
                App.Current.Resources.TryGetValue("Locator", out object locator);
                ((InstanceLocator)locator).Main.EditMascota.CurrentPet = mascota;
                await Application.Current.MainPage.Navigation.PushAsync(new EditPetView());
                IsBusy = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}