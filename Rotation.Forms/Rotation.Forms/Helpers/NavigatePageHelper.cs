using System;
using System.Collections.Generic;
using System.Text;

namespace Rotation.Forms.Helpers
{
    class NavigatePageHelper
    {
        public void OnNavigate(NavigatePage to)
        {
            this.Navigated?.Invoke(this, new NavigatePageEventArgs
            {
                To = to,
            });
        }

        public event EventHandler<NavigatePageEventArgs> Navigated;
    }

    class NavigatePageEventArgs : EventArgs
    {
        public NavigatePage To { get; set; }
    }

    enum NavigatePage
    {
        MainPage,
        EditPage,
    }
}
