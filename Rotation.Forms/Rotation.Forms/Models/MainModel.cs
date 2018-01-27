using Plugin.NetStandardStorage;
using Rotation.Forms.Models.Editor.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rotation.Forms.Models
{
    class MainModel : INotifyPropertyChanged
    {
        public ObservableCollection<ElementCollection> ElementCollections { get; } = new ObservableCollection<ElementCollection>();

        public ElementCollection SelectedCollection
        {
            get => this._selectedCollection;
            set
            {
                if (this._selectedCollection != value)
                {
                    this._selectedCollection = value;
                    this.OnPropertyChanged();

                    this.CanEdit = value != null;
                }
            }
        }
        private ElementCollection _selectedCollection;

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

        public void Load()
        {
            var folder = CrossStorage.FileSystem.LocalStorage.CreateFolder("data", Plugin.NetStandardStorage.Abstractions.Types.CreationCollisionOption.OpenIfExists);
            int count = 0;
            foreach (var file in folder.GetFiles())
            {
                var ext = file.Name.Split('.').ElementAtOrDefault(1);
                if (ext == "datav1")
                {
                    var c = new ElementCollection();
                    using (var stream = new StreamReader(file.Open(FileAccess.Read)))
                    {
                        c.LoadSerializedText(stream.ReadToEnd());
                    }
                    c.CollectionData.Number = count + 1;
                    this.ElementCollections.Add(c);

                    count++;
                }
            }
        }

        public void Save()
        {
            var folder = CrossStorage.FileSystem.LocalStorage.CreateFolder("data", Plugin.NetStandardStorage.Abstractions.Types.CreationCollisionOption.OpenIfExists);
            int count = 0;
            foreach (var collection in this.ElementCollections)
            {
                var file = folder.CreateFile(count + ".datav1", Plugin.NetStandardStorage.Abstractions.Types.CreationCollisionOption.OpenIfExists);
                using (var stream = new StreamWriter(file.Open(FileAccess.Write)))
                {
                    stream.Write(collection.ToSerializedText());
                }
                count++;
            }
            foreach (var file in folder.GetFiles())
            {
                var fn = file.Name.Split('.');
                if (int.TryParse(fn[0], out int fc))
                {
                    if (fn[1] != ".datav1" || fc >= count)
                    {
                        file.Delete();
                    }
                }
            }
        }

        public void Add()
        {
            this.ElementCollections.Add(new ElementCollection
            {
                CollectionData =
                {
                    Name = "新規アイテム",
                }
            });
        }

        public void Delete()
        {
            if (this.CanEdit)
            {
                this.ElementCollections.Remove(this.SelectedCollection);
                this.SelectedCollection = null;
            }
        }

        #region INotifyProeprtyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
