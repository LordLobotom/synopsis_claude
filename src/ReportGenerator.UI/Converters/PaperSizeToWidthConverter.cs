using System;
using System.Globalization;
using Avalonia.Data.Converters;
using ReportGenerator.Domain.Entities;

namespace ReportGenerator.UI.Converters;

public class PaperSizeToWidthConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not PaperSize paperSize)
            return 794.0; // Default A4 width in pixels (210mm at 96 DPI)

        // Convert mm to pixels at 96 DPI (1 inch = 96 pixels, 1 inch = 25.4mm)
        // pixels = mm * 96 / 25.4 ≈ mm * 3.7795
        return paperSize switch
        {
            PaperSize.A4 => 794.0,        // 210mm width
            PaperSize.A5 => 559.0,        // 148mm width
            PaperSize.Letter => 816.0,    // 8.5 inches (215.9mm)
            PaperSize.Legal => 816.0,     // 8.5 inches (215.9mm)
            PaperSize.Custom => 794.0,    // Default to A4
            _ => 794.0
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
