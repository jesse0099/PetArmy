using PetArmy.Infraestructure;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetArmy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CampCastraView : ContentPage
    {
        public CampCastraView()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Current.Resources.TryGetValue("Locator", out object locator);
            Task.Run(async () => {((InstanceLocator)locator).Main.CampCastracion.getCampCastra(); });
        }
    }
}