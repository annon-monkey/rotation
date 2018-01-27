﻿using Rotation.Forms.Helpers;
using Rotation.Forms.Models;
using Rotation.Forms.Models.Editor.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Rotation.Forms.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private readonly MainModel model = new MainModel();

        public NavigatePageHelper NavigateHelper { get; } = new NavigatePageHelper();

        public ObservableCollection<ElementCollection> ElementCollections => this.model.ElementCollections;

        public ElementCollection SelectedCollection
        {
            get => this.model.SelectedCollection;
            set => this.model.SelectedCollection = value;
        }

        public MainViewModel()
        {
            this.model.PropertyChanged += this.RaisePropertyChanged;
            this.model.PropertyChanged += (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(this.model.CanEdit):
                        this.EditCommand.OnCanExecuteChanged();
                        this.DeleteCommand.OnCanExecuteChanged();
                        break;
                }
            };
        }

        public RelayCommand AddCommand =>
            this._addCommand = this._addCommand ?? new RelayCommand(() => this.model.Add());
        private RelayCommand _addCommand;

        public RelayCommand EditCommand =>
            this._editCommand = this._editCommand ?? new RelayCommand(() =>
            {
                ViewModelStorage.EditElements = this.SelectedCollection;
                this.NavigateHelper.OnNavigate(NavigatePage.EditPage);
            }, () => this.model.CanEdit);
        private RelayCommand _editCommand;

        public RelayCommand DeleteCommand =>
            this._deleteCommand = this._deleteCommand ?? new RelayCommand(() => this.model.Delete(), () => this.model.CanEdit);
        private RelayCommand _deleteCommand;
    }
}
