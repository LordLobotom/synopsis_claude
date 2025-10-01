using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using ReportGenerator.Domain.Entities;

namespace ReportGenerator.UI.Converters;

public class ElementTypeToCornerRadiusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ElementType elementType)
        {
            return elementType switch
            {
                ElementType.RoundedRectangle => new CornerRadius(5),
                ElementType.Ellipse => new CornerRadius(1000), // Large radius for circular effect
                _ => new CornerRadius(0)
            };
        }

        return new CornerRadius(0);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
