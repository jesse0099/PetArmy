using PetArmy.Services;
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
            //_ = Shell.Current.GoToAsync("//LoginPage");
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
