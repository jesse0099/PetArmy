using PetArmy.Infraestructure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Syncfusion.DataSource;

namespace PetArmy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminLandingPage : ContentPage
    {
        public AdminLandingPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Current.Resources.TryGetValue("Locator", out object locator);
            Task.Run(() => { ((InstanceLocator)locator).Main.Admin.GetAdminAccountRequestsExecute(); });
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