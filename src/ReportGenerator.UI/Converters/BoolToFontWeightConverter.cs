using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ReportGenerator.UI.Converters;

public class BoolToFontWeightConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isBold && isBold)
            return FontWeight.Bold;

        return FontWeight.Normal;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is FontWeight weight)
            return weight == FontWeight.Bold;

        return false;
    }
}
