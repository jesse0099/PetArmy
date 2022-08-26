﻿namespace PetArmy.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public LoginViewModel Login { get; set; }
        public YesNoPopUpViewModel YesNoPopUp { get; set; }
        public UserViewModel User { get; set; }
        public RegisterViewModel Register { get; set; }
        public AdminViewModel Admin { get; set; }
        public MyServiceViewModel MyService { get; set; }
        public NewShelterViewModel NewShelter { get; set; }
        public NewCasaCunaViewModel NewCasaCuna { get; set; }
        public MascotaViewModel Mascota { get; set; }
        public AddMascotaViewModel AddMascota { get; set; }

        public EditShelterViewModel EditShelter { get; set; }
        public EditMascotaViewModel EditMascota { get; set; }

        public CampCastracionViewModel CampCastracion { get; set; }

        public AddCampCastraViewModel AddCampCastra { get; set; }

        public EditCampCastraViewModel EditCampCastra { get; set; }

        public SearchBarViewModel SearchBar { get; set; }

        public FeedViewModel Feed { get; set; }

        public ConfirmAdoptionRequestViewModel ConfirmAdoptionRequest { get; set; }
        public SolicitudesAprobacionViewModel SolicitudesAprobacion { get; set; }
        public UserProfileViewModel UserProfile { get; set; }

        public MascotasViewModel Mascotas { get; set; }

        public MainViewModel() : base()
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
            this.Feed = FeedViewModel.GetInstance();
            this.ConfirmAdoptionRequest = ConfirmAdoptionRequestViewModel.GetInstance();
            this.EditMascota = EditMascotaViewModel.GetInstance();
            this.CampCastracion = CampCastracionViewModel.GetInstance();
            this.AddCampCastra = AddCampCastraViewModel.GetInstance();
            this.EditCampCastra = EditCampCastraViewModel.GetInstance();
            this.UserProfile = UserProfileViewModel.GetInstance();
            this.User = new UserViewModel();
            this.YesNoPopUp = YesNoPopUpViewModel.GetInstance();
            this.Mascotas = MascotasViewModel.GetInstance();
            this.SolicitudesAprobacion = SolicitudesAprobacionViewModel.GetInstance();

    }
    }
}
