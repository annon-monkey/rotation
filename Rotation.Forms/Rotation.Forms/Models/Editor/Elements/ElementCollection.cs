using rotation.Entities;
using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rotation.Forms.Models.Editor.Elements
{
    class ElementCollection : ObservableCollection<IElement>
    {
        public Data CollectionData { get; } = new Data();

        public IEnumerable<IElement> GetChildElements(IElement parent)
        {
            foreach (var e in this.GetDescendantElementsWithRoot(parent)
                                  .Skip(1)
                                  .Where(ee => ee.GenerationLevel == parent.GenerationLevel + 1))
            {
                yield return e;
            }
        }

        public IEnumerable<IElement> GetDescendantElementsWithRoot(IElement root)
        {
            yield return root;
            foreach (var e in this.SkipWhile(ee => ee != root)
                                  .Skip(1)
                                  .TakeWhile(ee => ee.GenerationLevel > root.GenerationLevel))
            {
                yield return e;
            }
        }

        public IElement GetParentElement(IElement child)
        {
            return this.TakeWhile(e => e != child)
                       .Reverse()
                       .FirstOrDefault(e => e.GenerationLevel < child.GenerationLevel);
        }

        public IEnumerable<IElement> GetAncestorElementsWithChild(IElement child)
        {
            while (child != null)
            {
                yield return child;
                child = this.GetParentElement(child);
            }
        }

        public IElement GetPreviousElement(IElement element)
        {
            var db = this.TakeWhile(e => e != element)
                         .LastOrDefault();
            if (db != null)
            {
                return this.GetAncestorElementsWithChild(db).FirstOrDefault(e => e.GenerationLevel == element.GenerationLevel);
            }
            return null;
        }

        public IElement GetNextElement(IElement element)
        {
            var da = this.SkipWhile(e => e != element)
                         .Skip(1)
                         .TakeWhile(e => e.GenerationLevel >= element.GenerationLevel)
                         .FirstOrDefault(e => e.GenerationLevel == element.GenerationLevel);
            return da;
        }

        public EntityTimeline ToEntityTimeline()
        {
            var entity = EntityBuilder.Start();
            foreach (var child in this)
            {
                entity = this.ToEntity(entity, child);
            }

            return entity.ToTimeline();
        }

        private IEntity ToEntity(IEntity before, IElement element)
        {
            switch (element)
            {
                case MutualElement mutual:
                    return before.Mutual((children) =>
                    {
                        foreach (var child in this.GetChildElements(element))
                        {
                            children = this.ToEntity(children, child);
                        }
                        return children;
                    }, new RegularReverseFunction
                    {
                        HalfCycleTime = mutual.HalfFrequencyTime,
                    });
                default:
                    return element.ToEntity(before);
            }
        }

        public string ToSerializedText()
        {
            var text = new StringBuilder();
            text.Append($"{this.CollectionData.ToSerializedText()}\n");
            foreach (var e in this)
            {
                text.Append(e.ToSerializedText());
                text.Append("\n");
            }
            return text.ToString();
        }

        public void LoadSerializedText(string text)
        {
            var lines = text.Split('\n');
            this.CollectionData.LoadSerializedText(lines.First());
            foreach (var line in lines.Skip(1))
            {
                var e = Element.FromSerializedText(line);
                if (e != null)
                {
                    this.Add(e);
                }
            }
        }

        public class Data : INotifyPropertyChanged
        {
            public int Number
            {
                get => this._number;
                set
                {
                    if (this._number != value)
                    {
                        this._number = value;
                        this.OnPropertyChanged();
                    }
                }
            }
            private int _number;

            public string Name
            {
                get => this._name;
                set
                {
                    if (this._name != value)
                    {
                        this._name = value;
                        this.OnPropertyChanged();
                    }
                }
            }
            private string _name;

            public bool IsRepeat
            {
                get => this._isRepeat;
                set
                {
                    if (this._isRepeat != value)
                    {
                        this._isRepeat = value;
                        this.OnPropertyChanged();
                    }
                }
            }
            private bool _isRepeat;

            public string ToSerializedText()
            {
                return $"{this.Name}|{(this.IsRepeat ? 1 : 0)}";
            }

            public void LoadSerializedText(string text)
            {
                var data = text.Split('|');
                this.Name = data[0];
                this.IsRepeat = data[1] == "1";
            }

            #region INotifyProeprtyChanged

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
                => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            #endregion
        }
    }
}
