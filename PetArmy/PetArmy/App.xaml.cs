using PetArmy.Interfaces;
using PetArmy.Services;
using PetArmy.ViewModels;
using PetArmy.Views;
using Resx;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace PetArmy
{
    public partial class App : Application
    {
        public App()
        {
            //Register Syncfusion license (Recordar implementar para iOS)
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDQ1OTEyQDMxMzkyZTMxMmUzMGFYUVRyaFV6U1kwWFc4QUhIbWNCOEsxRkpOSjhVSHhsa3dtWDhodDhpY3c9");
            InitializeComponent();
            LocalizationResourceManager.Current.PropertyChanged += (_, _) => AppResources.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init(AppResources.ResourceManager);
            DependencyService.Register<MockDataStore>();
            MainPage = new SplashScreenView();
            
        }

        protected override void OnStart()
        {
           /*_i_auth = DependencyService.Get<IFirebaseAuth>();
            if (_i_auth.IsSignIn())
                LoginViewModel.GetInstance().ProviderLoginChecker(_i_auth.GetSignedUserProfile(), 
                    string.Empty,
                    "user");*/
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
