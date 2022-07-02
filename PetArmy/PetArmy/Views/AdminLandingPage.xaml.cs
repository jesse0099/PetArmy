using PetArmy.Infraestructure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace PetArmy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminLandingPage : ContentPage
    {
        Task initRequest;
        public AdminLandingPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            object locator;
            App.Current.Resources.TryGetValue("Locator", out locator);
            Task.Run(() => { ((InstanceLocator)locator).Main.Admin.GetAdminAccountRequests(); });
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > 0 && DeviceDisplay.MainDisplayInfo.Width != width)
            {
                var size = Application.Current.MainPage.Width / lst_requests.ItemSize;
                gridLayout.SpanCount = (int)size;
                lst_requests.LayoutManager = gridLayout;
            }
        }
    }
}