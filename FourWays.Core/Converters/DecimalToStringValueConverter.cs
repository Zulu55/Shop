namespace FourWays.Core.Converters
{
    using MvvmCross.Converters;
    using System;
    using System.Globalization;

    public class DecimalToStringValueConverter : MvxValueConverter<decimal, string>
    {
        protected override string Convert(decimal value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value:C2}";
        }
    }
}
