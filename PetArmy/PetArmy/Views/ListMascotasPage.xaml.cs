using PetArmy.Infraestructure;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetArmy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListMascotasPage : ContentPage
    {
        public ListMascotasPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Current.Resources.TryGetValue("Locator", out object locator);
            InstanceLocator local_locator = locator as InstanceLocator;
            Task.Run(() => local_locator.Main.Mascotas.getData());
        }
    }
}