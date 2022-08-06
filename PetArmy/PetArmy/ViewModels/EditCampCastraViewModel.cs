using PetArmy.Models;
using PetArmy.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class EditCampCastraViewModel : BaseViewModel
    {
        #region Singleton
        public static EditCampCastraViewModel _instance = null;
        public static EditCampCastraViewModel GetInstance()
        {
            if (_instance == null)
            {

                _instance = new EditCampCastraViewModel();

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

        public EditCampCastraViewModel()
        {
            initCommands();
            initClass();
        }

        public void initClass()
        {

        }

        public void initCommands()
        {
            UpdateCampCastra = new Command(updateCampCastra);
            DeleteSCampCastra = new Command(deleteCampCastra);
        }
        #endregion

        #region Varaiables

        private Camp_Castracion curCampCastra;

        public Camp_Castracion CurCampCastra
        {
            get { return curCampCastra; }
            set { curCampCastra = value; OnPropertyChanged(); }
        }

        private bool isEditing = false;

        public bool IsEditing
        {
            get { return isEditing; }
            set { isEditing = value; OnPropertyChanged(); }
        }

        private string campCastraName;

        public string CampCastraName
        {
            get { return campCastraName; }
            set { campCastraName = value; OnPropertyChanged(); }
        }

        private string campCastraDescription;

        public string CampCastraDescription
        {
            get { return campCastraDescription; }
            set { campCastraDescription = value; OnPropertyChanged(); }
        }

        private string campCastraDirection;

        public string CampCastraDirection
        {
            get { return campCastraDirection; }
            set { campCastraDirection = value; OnPropertyChanged(); }
        }

        private string campCastraTelephone;

        public string CampCastraTelephone
        {
            get { return campCastraTelephone; }
            set { campCastraTelephone = value; OnPropertyChanged(); }
        }

        private BindingList<Camp_Castracion> customList;

        public BindingList<Camp_Castracion> CustomList
        {
            get { return customList; }
            set { customList = value; OnPropertyChanged(); }
        }
        #endregion

        #region Commands
        public ICommand UpdateCampCastra { get; set; }
        public ICommand DeleteSCampCastra { get; set; }

        public async Task readyEdit(int campCastraID)
        {
            try
            {

                CurCampCastra = await GraphQLService.getCampCastraByID(campCastraID);
                CampCastraName = CurCampCastra.nombre_camp;
                CampCastraDescription = CurCampCastra.descripcion;
                CampCastraDirection = CurCampCastra.direccion;
                CampCastraTelephone = CurCampCastra.tel_contacto;
                List<Camp_Castracion> temp = new List<Camp_Castracion>();

                Camp_Castracion newItem = new Camp_Castracion();
                temp.Add(newItem);
                CustomList = new BindingList<Camp_Castracion>(temp);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async void updateCampCastra()
        {
            CurCampCastra.nombre_camp = CampCastraName;
            CurCampCastra.descripcion = CampCastraDescription;
            CurCampCastra.direccion = CampCastraDirection;
            CurCampCastra.tel_contacto = CampCastraTelephone;

            await GraphQLService.updateCampCastra(CurCampCastra);
            await Shell.Current.GoToAsync("//MyServicesView");

        }

        public async void deleteCampCastra()
        {
            await GraphQLService.deleteCampCastra(CurCampCastra.id_campana);
            await Shell.Current.GoToAsync("//MyServicesView");
        }
        #endregion
    }
}
