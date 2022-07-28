using System;
using Xamarin.Forms;

namespace PetArmy.CustomControls
{
    public class CustomLabel : Label
    {
        public static readonly BindableProperty JustifytextProperty = (
            BindableProperty.Create(
                propertyName: nameof(JustifyText),
                returnType: typeof(Boolean),
                declaringType: typeof(CustomLabel),
                defaultValue: false,
                defaultBindingMode: BindingMode.OneWay
                )
        );

        public bool JustifyText
        {
            get { return (Boolean)GetValue(JustifytextProperty); }
            set { SetValue(JustifytextProperty, value); }
        }
    }
}
