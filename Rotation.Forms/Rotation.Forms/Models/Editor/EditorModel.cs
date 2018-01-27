using Rotation.Forms.Models.Editor.Elements;
using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rotation.Forms.Models.Editor
{
    class EditorModel : INotifyPropertyChanged
    {
        public ElementCollection Elements
        {
            get => this._elements;
            set
            {
                if (this._elements != value)
                {
                    this._elements = value;
                    this.OnPropertyChanged();
                    this.SelectedElement = null;
                }
            }
        }
        private ElementCollection _elements = new ElementCollection();

        public IEnumerable<PickerItem<ElementType>> AddItems { get; } = new PickerItem<ElementType>[]
        {
            new PickerItem<ElementType>
            {
                Identity = ElementType.Point,
                Text = "Point",
            },
            new PickerItem<ElementType>
            {
                Identity = ElementType.Line,
                Text = "Line",
            },
            new PickerItem<ElementType>
            {
                Identity = ElementType.Mutual,
                Text = "Mutual",
            },
        };

        public IElement SelectedElement
        {
            get => this._selectedElement;
            set
            {
                if (this._selectedElement != value)
                {
                    this._selectedElement = value;
                    this.OnPropertyChanged();
                    this.UpdateCanDoneStatuses();
                }
            }
        }
        private IElement _selectedElement;

        public PickerItem<ElementType> SelectedAddItem
        {
            get => this._selectedAddItem;
            set
            {
                if (this._selectedAddItem != value)
                {
                    this._selectedAddItem = value;
                    this.OnPropertyChanged();

                    this.CanAdd = value != null;
                }
            }
        }
        private PickerItem<ElementType> _selectedAddItem;

        public bool IsEditMode
        {
            get => this._isEditMode;
            set
            {
                if (this._isEditMode != value)
                {
                    this._isEditMode = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool _isEditMode;

        #region Can hoge

        public bool CanAdd
        {
            get => this._canAdd;
            set
            {
                if (this._canAdd != value)
                {
                    this._canAdd = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool _canAdd;

        public bool CanEdit
        {
            get => this._canEdit;
            set
            {
                if (this._canEdit != value)
                {
                    this._canEdit = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool _canEdit;

        public bool CanUp
        {
            get => this._canUp;
            set
            {
                if (this._canUp != value)
                {
                    this._canUp = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool _canUp;

        public bool CanDown
        {
            get => this._canDown;
            set
            {
                if (this._canDown != value)
                {
                    this._canDown = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool _canDown;

        public bool CanRight
        {
            get => this._canRight;
            set
            {
                if (this._canRight != value)
                {
                    this._canRight = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool _canRight;

        public bool CanLeft
        {
            get => this._canLeft;
            set
            {
                if (this._canLeft != value)
                {
                    this._canLeft = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool _canLeft;

        #endregion

        public EditorModel()
        {
        }

        private void UpdateCanDoneStatuses()
        {
            var value = this.SelectedElement;

            if (value != null)
            {
                this.CanEdit = true;
                this.CanUp = this.Elements.GetPreviousElement(value) != null;
                this.CanDown = this.Elements.GetNextElement(value) != null;
                
                var prev = this.Elements.GetPreviousElement(value);
                var parent = this.Elements.GetParentElement(value);
                this.CanLeft = parent != null;
                this.CanRight = prev != null && prev.CanHaveChildren
                                && (value.CanNest || !this.Elements.GetAncestorElementsWithChild(prev).Skip(1).Any(e => e.GetType() == value.GetType()))
                                && value.GenerationLevel < 4;
            }
            else
            {
                this.CanEdit = this.CanUp = this.CanDown = this.CanLeft = this.CanRight = false;
            }
        }

        public void Add()
        {
            if (this.CanAdd)
            {
                IElement element = null;
                switch (this.SelectedAddItem.Identity)
                {
                    case ElementType.Point:
                        element = new PointElement();
                        break;
                    case ElementType.Line:
                        element = new LineElement();
                        break;
                    case ElementType.Mutual:
                        element = new MutualElement();
                        break;
                }
                
                if (this.SelectedElement != null)
                {
                    element.GenerationLevel = this.SelectedElement.GenerationLevel;
                    this.Elements.Insert(this.Elements.IndexOf(this.SelectedElement), element);
                }
                else
                {
                    this.Elements.Add(element);
                }

                this.UpdateCanDoneStatuses();
            }
        }

        public void Edit()
        {
            this.IsEditMode = this.CanEdit;
        }

        public void Delete()
        {
            if (this.CanEdit)
            {
                foreach (var e in this.Elements.GetDescendantElementsWithRoot(this.SelectedElement).ToArray())
                {
                    this.Elements.Remove(e);
                }
                this.SelectedElement = null;
                this.IsEditMode = false;

                this.UpdateCanDoneStatuses();
            }
        }

        public void Up()
        {
            if (this.CanUp)
            {
                this.Up(this.SelectedElement);
            }
        }

        private void Up(IElement target)
        {
            var block = this.Elements.GetDescendantElementsWithRoot(target).ToArray();
            var index = this.Elements.IndexOf(block.First());
            var afterIndex = this.Elements.IndexOf(this.Elements.GetPreviousElement(block.First()));
            foreach (var e in block)
            {
                this.Elements.Move(index, afterIndex);
                index++;
                afterIndex++;
            }

            this.UpdateCanDoneStatuses();
        }

        public void Down()
        {
            if (this.CanDown)
            {
                this.Up(this.Elements.GetNextElement(this.SelectedElement));
            }
        }

        public void Right()
        {
            if (this.CanRight)
            {
                this.SelectedElement.GenerationLevel++;

                this.UpdateCanDoneStatuses();
            }
        }

        public void Left()
        {
            if (this.CanLeft)
            {
                var next = this.Elements.GetNextElement(this.Elements.GetParentElement(this.SelectedElement));

                this.SelectedElement.GenerationLevel--;
                this.Elements.Move(this.Elements.IndexOf(this.SelectedElement), next == null ? this.Elements.Count - 1 : this.Elements.IndexOf(next));

                this.UpdateCanDoneStatuses();
            }
        }

        public void Apply()
        {
            this.SelectedElement = null;
            this.IsEditMode = false;
        }

        #region INotifyProeprtyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
