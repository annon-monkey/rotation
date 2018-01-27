using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Rotation.Forms.Behaviors
{
    class ConfirmButtonBehavior : Behavior<Button>
    {
        private readonly WeakReference<Button> button = new WeakReference<Button>(null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ConfirmButtonBehavior), propertyChanged: OnCommandChanged);

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        public static readonly BindableProperty MessageProperty = BindableProperty.Create(nameof(Message), typeof(string), typeof(ConfirmButtonBehavior));

        private static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var b = (ConfirmButtonBehavior)bindable;
            if (oldValue is ICommand oldCommand)
            {
                oldCommand.CanExecuteChanged -= b.Command_CanExecuteChanged;
            }
            if (newValue is ICommand newCommand)
            {
                newCommand.CanExecuteChanged += b.Command_CanExecuteChanged;
            }
            b.OnCanExecuteChanged();
        }

        protected override void OnAttachedTo(Button bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.Clicked += this.Bindable_Clicked;
            this.button.SetTarget(bindable);
            this.OnCanExecuteChanged();
        }

        protected override void OnDetachingFrom(Button bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.Clicked -= this.Bindable_Clicked;
            this.button.SetTarget(null);
            this.OnCanExecuteChanged();
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if (this.button.TryGetTarget(out Button b))
            {
                b.IsEnabled = this.Command?.CanExecute(null) ?? true;
            }
        }

        private void OnCanExecuteChanged() => this.Command_CanExecuteChanged(this, EventArgs.Empty);

        private async void Bindable_Clicked(object sender, EventArgs e)
        {
            if (this.button.TryGetTarget(out Button b) && b.IsEnabled)
            {
                // use when using navigation page
                // int index = Application.Current.MainPage.Navigation.NavigationStack.Count - 1;
                // var currPage = Application.Current.MainPage.Navigation.NavigationStack[index];

                if (await Application.Current.MainPage.DisplayAlert("確認", this.Message, "はい", "いいえ"))
                {
                    this.Command?.Execute(null);
                }
            }
        }
    }
}
