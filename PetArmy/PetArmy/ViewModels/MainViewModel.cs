using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PetArmy.ViewModels
{
    public  class MainViewModel: BaseViewModel
    {
        public LoginViewModel Login { get; set; }
        public UserViewModel User { get; set; }
        public RegisterViewModel Register { get; set; }
        public AdminViewModel Admin { get; set; }
        public MyServiceViewModel MyService { get; set; }
        public NewShelterViewModel NewShelter { get; set; }
        public NewCasaCunaViewModel NewCasaCuna { get; set; }
        public MascotaViewModel Mascota { get; set; }
        public AddMascotaViewModel AddMascota { get; set; }

        public EditShelterViewModel EditShelter { get; set; }

        public CampCastracionViewModel CampCastracion { get; set; }

        public AddCampCastraViewModel AddCampCastra { get; set; }

        public EditCampCastraViewModel EditCampCastra { get; set; }

        public SearchBarViewModel SearchBar { get; set; }

        public UserProfileViewModel UserProfile { get; set; }

        public MainViewModel(): base()
        {
            this.Login = LoginViewModel.GetInstance();
            this.Admin = AdminViewModel.GetInstance();
            this.Register = RegisterViewModel.GetInstance();
            this.MyService = MyServiceViewModel.GetInstance();
            this.NewShelter = NewShelterViewModel.GetInstance();
            this.NewCasaCuna = NewCasaCunaViewModel.GetInstance();
            this.Mascota = MascotaViewModel.GetInstance();
            this.AddMascota = AddMascotaViewModel.GetInstance();
            this.SearchBar = SearchBarViewModel.GetInstance();
            this.EditShelter = EditShelterViewModel.GetInstance();
            this.CampCastracion = CampCastracionViewModel.GetInstance();
            this.AddCampCastra = AddCampCastraViewModel.GetInstance();
            //this.EditCampCastra = EditCampCastraViewModel.GetInstance();
            this.UserProfile = UserProfileViewModel.GetInstance();
            this.User = new UserViewModel();
         

        }
    }
}
