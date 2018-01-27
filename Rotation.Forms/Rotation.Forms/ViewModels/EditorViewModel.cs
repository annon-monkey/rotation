using Rotation.Forms.Models.Editor;
using Rotation.Forms.Models.Editor.Elements;
using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Rotation.Forms.ViewModels
{
    class EditorViewModel : ViewModelBase
    {
        private readonly EditorModel model = new EditorModel();

        public IEnumerable<PickerItem<ElementType>> AddItems => this.model.AddItems;

        public IElement SelectedElement
        {
            get => this.model.SelectedElement;
            set => this.model.SelectedElement = value;
        }

        public PickerItem<ElementType> SelectedAddItem
        {
            get => this.model.SelectedAddItem;
            set => this.model.SelectedAddItem = value;
        }

        public bool IsEditMode => this.model.IsEditMode;

        public ObservableCollection<IElement> Elements => this.model.Elements;

        public EditorViewModel()
        {
            this.model.PropertyChanged += this.RaisePropertyChanged;
            this.model.PropertyChanged += (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(this.model.CanAdd):
                        this.AddCommand.OnCanExecuteChanged();
                        break;
                    case nameof(this.model.CanEdit):
                        this.EditCommand.OnCanExecuteChanged();
                        this.DeleteCommand.OnCanExecuteChanged();
                        break;
                    case nameof(this.model.CanDown):
                        this.DownCommand.OnCanExecuteChanged();
                        break;
                    case nameof(this.model.CanUp):
                        this.UpCommand.OnCanExecuteChanged();
                        break;
                    case nameof(this.model.CanLeft):
                        this.LeftCommand.OnCanExecuteChanged();
                        break;
                    case nameof(this.model.CanRight):
                        this.RightCommand.OnCanExecuteChanged();
                        break;
                }
            };
        }

        public RelayCommand AddCommand =>
            this._addCommand = this._addCommand ?? new RelayCommand(() => this.model.Add(), () => this.model.CanAdd);
        private RelayCommand _addCommand;

        public RelayCommand EditCommand =>
            this._editCommand = this._editCommand ?? new RelayCommand(() => this.model.Edit(), () => this.model.CanEdit);
        private RelayCommand _editCommand;

        public RelayCommand DeleteCommand =>
            this._deleteCommand = this._deleteCommand ?? new RelayCommand(() => this.model.Delete(), () => this.model.CanEdit);
        private RelayCommand _deleteCommand;

        public RelayCommand UpCommand =>
            this._upCommand = this._upCommand ?? new RelayCommand(() => this.model.Up(), () => this.model.CanUp);
        private RelayCommand _upCommand;

        public RelayCommand DownCommand =>
            this._downCommand = this._downCommand ?? new RelayCommand(() => this.model.Down(), () => this.model.CanDown);
        private RelayCommand _downCommand;

        public RelayCommand RightCommand =>
            this._rightCommand = this._rightCommand ?? new RelayCommand(() => this.model.Right(), () => this.model.CanRight);
        private RelayCommand _rightCommand;

        public RelayCommand LeftCommand =>
            this._leftCommand = this._leftCommand ?? new RelayCommand(() => this.model.Left(), () => this.model.CanLeft);
        private RelayCommand _leftCommand;

        public RelayCommand ApplyCommand =>
            this._applyCommand = this._applyCommand ?? new RelayCommand(() => this.model.Apply());
        private RelayCommand _applyCommand;
    }
}
