using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace Rotation.Forms.ViewModels
{
    abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected void RaisePropertyChanged(object sender, PropertyChangedEventArgs e)
            => this.PropertyChanged?.Invoke(this, e);

        public class RelayCommand : ICommand
        {
            private readonly Action action;
            private readonly Func<bool> canExecute;

            public RelayCommand(Action act, Func<bool> canExec = null)
            {
                this.action = act;
                this.canExecute = canExec ?? (() => true);
            }

            public void OnCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => this.canExecute();

            public void Execute(object parameter) => this.action();
        }
    }
}
