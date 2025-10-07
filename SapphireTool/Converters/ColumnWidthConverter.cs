using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SapphireTool.Converters
{
    public class ColumnWidthConverter : IValueConverter
    {
        // when I was making v2 this sucked 
        public double MinWidth { get; set; } = 50;

        public double MaxWidth { get; set; } = 500;

        public double DefaultProportion { get; set; } = 1;

        public bool UseStarSizing { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double proportion = DefaultProportion;
            if (parameter is string proportionStr && double.TryParse(proportionStr, out double paramProportion))
            {
                proportion = paramProportion;
            }

            if (UseStarSizing)
            {
                return new GridLength(proportion, GridUnitType.Star);
            }
            else
            {
                if (value is double width)
                {
                    return new GridLength(Math.Max(MinWidth, Math.Min(width, MaxWidth)), GridUnitType.Pixel);
                }
                return new GridLength(MinWidth, GridUnitType.Pixel);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GridLength gridLength)
            {
                if (gridLength.IsStar)
                {
                    return gridLength.Value;
                }
                else
                {
                    return gridLength.Value;
                }
            }
            return DefaultProportion;
        }
    }
}