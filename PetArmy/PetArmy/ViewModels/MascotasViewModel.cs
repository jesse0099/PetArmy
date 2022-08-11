using System.ComponentModel;
using PetArmy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using PetArmy.Services;
using PetArmy.Helpers;
using System.Windows.Input;
using Xamarin.Forms;
using PetArmy.Views;

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

            //this.Mascotas = new BindingList<Mascota>()
            //{
            //    new Mascota()
            //    {
            //        nombre = null,
            //        id_refugio = 0,
            //        castrado = false,
            //        alergias = false,
            //        discapacidad = false,
            //        enfermedad = false,
            //        especie = "loool",
            //        id_mascota = 10,
            //        estado = false,
            //        peso = 0,
            //        edad = 0,
            //        raza = null,
            //        vacunado = false,
            //        descripcion = "amazin",
            //        refugio = null,
            //        mascota_tags = null,
            //        imagenes_mascota = null,
            //        Db_Bools = null,
            //        Ubicacion = null
            //    }
            //};

            MascotasList = new BindingList<CstmItemMascota>();
            initCommands();
            initClass();
        }

        public void initClass()
        {

        }

        public void initCommands()
        {
            NewMascota = new Command(newMascota);
            

        }



        private BindingList<CstmItemMascota> mascotasList;

        public BindingList<CstmItemMascota> MascotasList
        {
            get { return mascotasList; }
            set { mascotasList = value; OnPropertyChanged(); }
        }

        private List<Imagen_Mascota> petsImageCollection;

        public List<Imagen_Mascota> PetsImageCollection
        {
            get { return petsImageCollection; }
            set { petsImageCollection = value; OnPropertyChanged(); }
        }

        public ICommand NewMascota { get; set; }
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



        #endregion





        #region Functions

        public async Task getData()
        {
            IsBusy = true;
            List<CstmItemMascota> temp = new List<CstmItemMascota>();

            //Get all Pets
            List<Mascota> allPets = await MascotaService.getAllMascotas();

            if (allPets.Count > 0)
            {
                //get pet's images
                foreach (Mascota pet in allPets)
                {
                    petsImageCollection = await MascotaService.getPet_Images(pet.id_mascota);

                    /*Search for the default picture, if not set by user use default */
                    bool hasDefault = false;
                    byte[] imageData = null;

                    foreach (Imagen_Mascota image in petsImageCollection)
                    {
                        if (image.isDefault)
                        {
                            hasDefault = true;
                            imageData = Convert.FromBase64String(image.imagen);
                        }
                    }

                    /*Add custom item to list*/
                    CstmItemMascota newItem = new CstmItemMascota();
                    newItem.mascota = pet;

                    if (hasDefault)
                    {
                        //Set the pet's image on the item.
                        newItem.Image = imageData;
                    }
                    else
                    {
                        //Sets the default image for the pet
                        imageData = Convert.FromBase64String(String64Images.petDefault);
                        newItem.Image = imageData;
                    }

                    temp.Add(newItem);
                }
            }
            else
            {
                //print something 'There are no pets in the world :('
            }



            MascotasList = new BindingList<CstmItemMascota>(temp);
            IsBusy = false;
        }

        #endregion







    }
}