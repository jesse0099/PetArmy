using PetArmy.Helpers;
using PetArmy.Interfaces;
using PetArmy.Models;
using PetArmy.Services;
using PetArmy.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
            
            try
            {
                IFirebaseAuth _i_auth = DependencyService.Get<IFirebaseAuth>(); ;
                var registered_user = _i_auth.GetSignedUserProfile();
                Settings.UID = registered_user.Uid;
                Usuario curUser = new Usuario(Settings.UID, 2);

                if (!checkForEmpyValues())
                {
                    Camp_Castracion camp = new Camp_Castracion();
                    camp.id_campana = await generateCampCastraID();
                    camp.nombre_camp = Nombre_Camp;
                    camp.descripcion = Descripcion;
                    camp.direccion = Direccion;
                    camp.tel_contacto = Tel_Contacto;
                    camp.fecha_inicio = Fecha_Inicio;
                    camp.fecha_fin = Fecha_Fin;
                    camp.activo = true;
                    bool chk = await GraphQLService.addCampCastra(camp, curUser);
                }
                await App.Current.MainPage.DisplayAlert("Success", "Campaing Saved!", "Ok");
                Application.Current.MainPage = new NavigationPage(new CampCastraView());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool checkForEmpyValues()
        {
            bool result = false;

            if (String.IsNullOrEmpty(this.Nombre_Camp) || String.IsNullOrEmpty(this.Descripcion) || String.IsNullOrEmpty(this.Tel_Contacto))
            {
                result = true;
            }

            return result;
        }
        #endregion

        public async Task<int> generateCampCastraID()
        {
            int newCampCastraID = 0;

            try
            {
                List<Camp_Castracion> campaings = await GraphQLService.getAllCampCastra();
                int lastID = campaings[campaings.Count - 1].id_campana;
                newCampCastraID = lastID + 1;

            }
            catch (Exception)
            {

                throw;
            }

            return newCampCastraID;
        }

    }
}