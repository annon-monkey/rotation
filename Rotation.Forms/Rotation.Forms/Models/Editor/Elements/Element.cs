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
            var k = new string(text.TakeWhile(c => c != '$').ToArray());
            if (int.TryParse(k, out int keyInt))
            {
                var key = (ElementType)keyInt;
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

                try
                {
                    element?.LoadSerializedText(text);
                }
                catch
                {
                    return null;
                }
                return element;
            }

            return null;
        }
    }
}
