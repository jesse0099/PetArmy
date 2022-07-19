using PetArmy.Models;
using PetArmy.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class AddMascotaViewModel : BaseViewModel
    {
        #region singleton

        private static AddMascotaViewModel Instance;
        public static AddMascotaViewModel GetInstance()
        {
            if (Instance == null)
            {
                Instance = new AddMascotaViewModel();
            }

            return Instance;
        }
        public static void DisposeInstance()
        {
            if (Instance != null)
            {
                Instance = null;
            }
        }
        public AddMascotaViewModel()
        {
            initCommands();
            initClass();

        }
        public void initCommands()
        {
            AddMascota = new Command(addMascota);
        }
        public void initClass()
        {

        }

        #endregion


        #region variable
        private int id_mascota;
        private string especie;
        private string raza;
        private string nombre;
        private float peso;
        private bool castrado;
        private bool vacuando;
        private bool estado;
        private string descripcion;
        private bool discapacidad;
        private bool enfermedad;
        private bool alergias;
        private int id_refugio;

        public int Id_Mascota
        {
            get { return id_mascota; }
            set { id_mascota = value; OnPropertyChanged(); }
        }
        public string Especie
        {
            get { return especie; }
            set { especie = value; OnPropertyChanged(); }
        }
        public string Raza
        {
            get { return raza; }
            set { raza = value; OnPropertyChanged(); }
        }
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; OnPropertyChanged(); }
        }
        public float Peso
        {
            get { return peso; }
            set { peso = value; OnPropertyChanged(); }
        }
        public bool Castrado
        {
            get { return castrado; }
            set { castrado = value; OnPropertyChanged(); }
        }
        public bool Vacuando
        {
            get { return vacuando; }
            set { vacuando = value; OnPropertyChanged(); }
        }
        public bool Estado
        {
            get { return estado; }
            set { estado = value; OnPropertyChanged(); }
        }
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; OnPropertyChanged(); }
        }
        public bool Discapacidad
        {
            get { return discapacidad; }
            set { discapacidad = value; OnPropertyChanged(); }
        }
        public bool Enfermedad
        {
            get { return enfermedad; }
            set { enfermedad = value; OnPropertyChanged(); }
        }
        public bool Alergias
        {
            get { return alergias; }
            set { alergias = value; OnPropertyChanged(); }
        }

        public int Id_Refugio
        {
            get { return id_refugio; }
            set { id_refugio = value; OnPropertyChanged(); }
        }

        #endregion


        #region commands and Functions
        public ICommand AddMascota { get; set; }



        public async void addMascota()
        {
            Mascota pet = new Mascota();
            try
            {
                //pet.id_mascota = 13;
                pet.nombre = Nombre;
                pet.id_refugio = Id_Refugio;
                pet.especie = Especie;
                pet.raza = Raza;
                pet.peso = Peso;
                pet.castrado = Castrado;
                pet.vacunado = Vacuando;
                pet.estado = Estado;
                pet.descripcion = Descripcion;
                pet.discapacidad = Discapacidad;
                pet.enfermedad = Enfermedad;
                pet.alergias = Alergias;

                await GraphQLService.addMascota(pet);
            }
            catch
            {
            }
        }

        #endregion
    }
}
