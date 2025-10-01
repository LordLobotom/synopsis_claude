using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ReportGenerator.UI.Converters;

public class BoolToFontStyleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isItalic && isItalic)
            return FontStyle.Italic;

        return FontStyle.Normal;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is FontStyle style)
            return style == FontStyle.Italic;

        return false;
    }
}
