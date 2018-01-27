using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rotation.Forms.Models.Editor.Elements
{
    static class Element
    {
        public static IElement FromSerializedText(string text)
        {
            var key = (ElementType)int.Parse(new string(text.TakeWhile(c => c != '$').ToArray()));
            IElement element = null;

            switch (key)
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

            element?.LoadSerializedText(text);
            return element;
        }
    }
}
