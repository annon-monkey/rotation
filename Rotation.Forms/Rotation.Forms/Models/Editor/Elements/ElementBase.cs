using rotation.Entities;
using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rotation.Forms.Models.Editor.Elements
{
    abstract class ElementBase : IElement, INotifyPropertyChanged
    {
        public abstract ElementType ElementType { get; }

        public string Description
        {
            get => this._description;
            protected set
            {
                if (this._description != value)
                {
                    this._description = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private string _description;

        public bool CanHaveChildren
        {
            get => this._canHaveChildren;
            set
            {
                if (this._canHaveChildren != value)
                {
                    this._canHaveChildren = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool _canHaveChildren;

        public bool CanNest
        {
            get => this._canNest;
            set
            {
                if (this._canNest != value)
                {
                    this._canNest = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool _canNest;

        public int GenerationLevel
        {
            get => this._generationLevel;
            set
            {
                if (this._generationLevel != value)
                {
                    this._generationLevel = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private int _generationLevel;

        public bool IsError
        {
            get => this._isError;
            set
            {
                if (this._isError != value)
                {
                    this._isError = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool _isError;

        public abstract IEntity ToEntity(IEntity before);

        public virtual string ToSerializedText()
        {
            return $"{(int)this.ElementType}${this.GenerationLevel}";
        }

        public virtual void LoadSerializedText(string text)
        {
            var data = text.Split('$');
            this.GenerationLevel = int.Parse(data[1]);
        }

        #region INotifyProeprtyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
