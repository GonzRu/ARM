using System;

namespace NormalModeLibrary
{
    [System.Windows.Data.ValueConversion( typeof( String ), typeof( Object ) )]
    internal class AnalogSignalValueConverter : System.Windows.Data.IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            var val = double.Parse( value.ToString() );
            return ( val < 0 ) ?
                string.Format( "{0}", val.ToString( "00.00" ) ) :
                string.Format( " {0}", val.ToString( "00.00" ) );
        }
        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }

    [System.Windows.Data.ValueConversion( typeof( String[] ), typeof( Object ) )]
    internal class AnalogCaptionMultiValueConverter : System.Windows.Data.IMultiValueConverter
    {
        public object Convert( object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            var str1 = values[0].ToString();
            var str2 = values[1].ToString();

            return string.IsNullOrEmpty( str2 ) ? str1 : string.Format( "{0} ( {1} )", str1, str2 );
        }
        public object[] ConvertBack( object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }

    [System.Windows.Data.ValueConversion( typeof( Boolean ), typeof( System.Windows.Media.Brush ) )]
    internal class AnalogSignalColorValueConverter : System.Windows.Data.IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            return ( System.Convert.ToBoolean( value ) ) ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.Black;
        }
        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
