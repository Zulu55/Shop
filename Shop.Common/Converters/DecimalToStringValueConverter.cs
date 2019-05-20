namespace Shop.Common.Converters
{
    using System;
    using System.Globalization;
    using MvvmCross.Converters;

    public class DecimalToStringValueConverter : MvxValueConverter<decimal, string>
    {
        protected override string Convert(decimal value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value:C2}";
        }
    }
}
