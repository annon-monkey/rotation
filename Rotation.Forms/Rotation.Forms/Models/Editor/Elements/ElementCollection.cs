using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Rotation.Forms.Models.Editor.Elements
{
    class ElementCollection : ObservableCollection<IElement>
    {
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
    }
}
