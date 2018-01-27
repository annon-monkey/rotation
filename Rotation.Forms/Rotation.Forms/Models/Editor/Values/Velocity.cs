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

        public double ToEntityValue()
        {
            return (double)this.Value / 100 * (this.IsReverse ? -1 : 1);
        }

        public string ToSerializedText()
        {
            return $"{this.Value},{(this.IsReverse ? 1 : 0)},{(this.IsTakeOver ? 1 : 0)}";
        }

        public void LoadSerializedText(string text)
        {
            var data = text.Split(',');
            this.Value = int.Parse(data[0]);
            this.IsReverse = data[1] == "1";
            this.IsTakeOver = data[2] == "1";
        }

        #region INotifyProeprtyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
