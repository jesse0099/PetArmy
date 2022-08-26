using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetArmy.Views.PopUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YesNoPopUp : SfPopupLayout
    {
        public YesNoPopUp()
        {
            InitializeComponent();
        }
    }
}