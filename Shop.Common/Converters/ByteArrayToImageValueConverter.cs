namespace Shop.Common.Converters
{
    using global::Android.Graphics;
    using MvvmCross.Converters;
    using System;
    using System.Globalization;

    public class ByteArrayToImageValueConverter : MvxValueConverter<byte[], Bitmap>
    {
        protected override Bitmap Convert(byte[] value, Type targetType, object parameter, CultureInfo culture)
        {
            return BitmapFactory.DecodeByteArray(value, 0, value.Length); ;
        }
    }
}