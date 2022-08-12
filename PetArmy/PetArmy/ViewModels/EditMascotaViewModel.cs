using PetArmy.Models;
using PetArmy.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class EditMascotaViewModel : BaseViewModel
    {
        #region Singleton
        public static EditMascotaViewModel instance = null;

        public static EditMascotaViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new EditMascotaViewModel();
            }
            return instance;
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
            initClass();
            initCommands();
        }
        public void initClass()
        {

        }

        public void initCommands()
        {
            UpdateMascota = new Command(updateMascota);
            DeleteMascota = new Command(deleteMascota);
            PickImage = new Command(pickImage);
        }

        #endregion


        #region Variables and Commands

        ICommand UpdateMascota;
        ICommand DeleteMascota;
        ICommand PickImage;

        private Mascota currentMascota;
        public Mascota CurrentMascota
        {
            get { return currentMascota; }
            set { currentMascota = value; OnPropertyChanged(); }
        }

        #region privateVars
        private string petName;
        private int id_refugio;
        private bool castrado;
        private bool alergias;
        private bool discapacidad;
        private bool enfermedad;
        private string especie;
        private int id_mascota;
        private bool estado;
        private float peso;
        private float edad;
        private string raza;
        private bool vacunado;
        private string descripcion;
        private List<MascotaTag> mascota_tags;
        private IEnumerable<Imagen_Mascota> imagen_Mascota;

        #endregion

        #region publicVars
        public string PetName
        {
            get { return petName; }
            set { petName = value; OnPropertyChanged(); }
        }

        public int IdRefugio
        {
            get { return id_refugio; }
            set { id_refugio = value; OnPropertyChanged(); }

        }
        public bool Castrado
        {
            get { return castrado; }
            set { castrado = value; OnPropertyChanged(); }
        }


        public bool Alergias
        {
            get { return alergias; }
        }
        public bool Discapacidad 
        {
            get { return ; }
        }
        public bool enfermedad
        {
            get { return; }
        }
        public bool vacunado
        {
            get { return; }
        }
        public string especie
        {
            get { return; }
        }
        public bool estado
        {
            get { return; }
        }
        public float peso
        {
            get { return; }
        }
        public float edad
        {
            get { return; }
        }
        public string raza
        {
            get { return; }
        }
        public string descripcion
        {
            get { return; }
        }
        public List<MascotaTag> mascota_tags
        {
            get { return; }
        }
        public IEnumerable<Imagen_Mascota> imagen_Mascota
        {
            get { return; }
        }

        #endregion




        //not sure if i need it
        //private Refugio refugio { get; set; }




        #endregion


        #region Functions
        public async Task readyEdit(int mascotaID) 
        {
            try 
            {
                CurrentMascota = await MascotaService.getMascotaByPK(mascotaID.ToString());
                PetName = CurrentMascota.nombre;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async void updateMascota()
        {

        }

        public async void pickImage()
        {

        }

        public async void deleteMascota()
        {
            await MascotaService.deleteMascota(CurrentMascota.id_mascota);
            //await Shell
        }

        #endregion
    }
}
