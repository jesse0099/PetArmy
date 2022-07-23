using PetArmy.Models;
using PetArmy.Services;
using PetArmy.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class AddCampCastraViewModel : BaseViewModel
    {
        #region singleton
        public static AddCampCastraViewModel _instance = null;

        public static AddCampCastraViewModel GetInstance()
        {
            if (_instance == null)
            {

                _instance = new AddCampCastraViewModel();

            }

            return _instance;
        }

        public static void DisposeInstance()
        {
            if (_instance != null)
            {
                _instance = null;
            }
        }

        public AddCampCastraViewModel()
        {
            initCommands();
            initClass();

        }

        public void initCommands()
        {
            AddCampCastra = new Command(addCampCastra);
        }

        public void initClass()
        {

        }

        #endregion

        #region variable
        private int id_campana;
        private string nombre_camp;
        private string descripcion;
        private string direccion;
        private string tel_contacto;
        private DateTime fecha_inicio;
        private DateTime fecha_fin;
        private bool activo;

        public int Id_Campana
        {
            get { return id_campana; }
            set { id_campana = value; OnPropertyChanged(); }
        }

        public string Nombre_Camp
        {
            get { return nombre_camp; }
            set { nombre_camp = value; OnPropertyChanged(); }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; OnPropertyChanged(); }
        }
        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; OnPropertyChanged(); }
        }
        public string Tel_Contacto
        {
            get { return tel_contacto; }
            set { tel_contacto = value; OnPropertyChanged(); }
        }

        public DateTime Fecha_Inicio
        {
            get { return fecha_inicio; }
            set { fecha_inicio = value; OnPropertyChanged(); }
        }

        public DateTime Fecha_Fin
        {
            get { return fecha_fin; }
            set { fecha_fin = value; OnPropertyChanged(); }
        }

        public bool Activo
        {
            get { return activo; }
            set { activo = value; OnPropertyChanged(); }
        }
        #endregion

        #region commands and Functions
        public ICommand AddCampCastra { get; set; }

        public async void addCampCastra()
        {
            Camp_Castracion camp = new Camp_Castracion();
            try
            {
                camp.id_campana = Id_Campana;
                camp.nombre_camp = Nombre_Camp;
                camp.descripcion = Descripcion;
                camp.direccion = Direccion;
                camp.tel_contacto = Tel_Contacto;
                camp.fecha_inicio = Fecha_Inicio;
                camp.fecha_fin = Fecha_Fin;
                camp.activo = Activo;


                await GraphQLService.addCampCastra(camp);
                await App.Current.MainPage.DisplayAlert("Success", "Campaing Saved!", "Ok");
                Application.Current.MainPage = new NavigationPage(new CampCastraView());
            }
            catch
            {
            }
        }
        #endregion
    }
}