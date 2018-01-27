using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Rotation.Forms.Converters
{
    class ElementType2BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string param && value is ElementType type)
            {
                switch (type)
                {
                    case ElementType.CollectionSetting:
                        return param == "CollectionSetting";
                    case ElementType.Point:
                        return param == "Point";
                    case ElementType.Line:
                        return param == "Line";
                    case ElementType.Mutual:
                        return param == "Mutual";
                    case ElementType.Loop:
                        return param == "Loop";
                    case ElementType.LinearMutual:
                        return param == "LinearMutual";
                    case ElementType.RandomMutual:
                        return param == "RandomMutual";
                    case ElementType.Random:
                        return param == "Random";
                    case ElementType.Trigonometric:
                        return param == "Trigonometric";
                    case ElementType.From:
                        return param == "From";
                    case ElementType.Stop:
                        return param == "Stop";
                    default:
                        return false;
                }
            }
            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
