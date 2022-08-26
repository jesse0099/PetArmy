using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetArmy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SolicitudesAdopcion : ContentPage
    {
        public SolicitudesAdopcion()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (Application.Current == null)
                return;
            if (Application.Current.MainPage == null)
                return;
            if (width > 0 && DeviceDisplay.MainDisplayInfo.Width != width)
            {
                var size = Application.Current.MainPage.Width / listView.ItemSize;
                gridLayout.SpanCount = (int)size;
                listView.LayoutManager = gridLayout;
            }
        }
    }
}