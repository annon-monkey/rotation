using Rotation.Forms.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Rotation.Forms.Behaviors
{
    class NavigatePageBehavior : Behavior<Page>
    {
        public NavigatePageHelper Helper
        {
            get => (NavigatePageHelper)GetValue(HelperProperty);
            set => SetValue(HelperProperty, value);
        }
        public static readonly BindableProperty HelperProperty = BindableProperty.Create(nameof(Helper), typeof(NavigatePageHelper), typeof(NavigatePageBehavior), propertyChanged: OnHelperChanged);

        private static void OnHelperChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var b = (NavigatePageBehavior)bindable;

            if (oldValue is NavigatePageHelper oldHelper)
            {
                oldHelper.Navigated -= b.Helper_Navigated;
            }
            if (newValue is NavigatePageHelper newHelper)
            {
                newHelper.Navigated += b.Helper_Navigated;
            }
        }

        private void Helper_Navigated(object sender, NavigatePageEventArgs e)
        {
            Page page = null;
            switch (e.To)
            {
                case NavigatePage.EditPage:
                    page = new EditPage();
                    break;
                case NavigatePage.MainPage:
                    page = new MainPage();
                    break;
            }

            Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
