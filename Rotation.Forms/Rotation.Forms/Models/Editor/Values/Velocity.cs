using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rotation.Forms.Models.Editor.Values
{
    public class Velocity : INotifyPropertyChanged
    {
        public int Value
        {
            get => this._value;
            set
            {
                if (this._value != value)
                {
                    this._value = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private int _value;

        public bool IsTakeOver
        {
            get => this._isTakeOver;
            set
            {
                if (this._isTakeOver != value)
                {
                    this._isTakeOver = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool _isTakeOver;

        public bool IsReverse
        {
            get => this._isReverse;
            set
            {
                if (this._isReverse != value)
                {
                    this._isReverse = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool _isReverse;

        public bool IsZero => !this.IsTakeOver && this.Value == 0;

        public string ToListText()
        {
            if (!this.IsTakeOver)
            {
                return $"{this.Value,3} " + (this.IsReverse ? "(逆)" : "(正)");
            }
            else
            {
                return "(継承)";
            }
        }

        #region INotifyProeprtyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
