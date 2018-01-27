using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rotation.Forms
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VelocityEditor : ContentView
	{
        public Velocity Velocity
        {
            get => (Velocity)GetValue(VelocityProperty);
            set => SetValue(VelocityProperty, value);
        }
        public static readonly BindableProperty VelocityProperty = BindableProperty.Create("Velocity", typeof(Velocity), typeof(VelocityEditor));

        public bool CanTakeOver
        {
            get => (bool)GetValue(CanTakeOverProperty);
            set => SetValue(CanTakeOverProperty, value);
        }
        public static readonly BindableProperty CanTakeOverProperty = BindableProperty.Create("CanTakeOver", typeof(bool), typeof(VelocityEditor), true);

		public VelocityEditor ()
		{
			InitializeComponent ();
		}
	}
}