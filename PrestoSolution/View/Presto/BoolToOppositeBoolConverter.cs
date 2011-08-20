using System;
using System.Globalization;
using System.Windows.Data;

namespace Presto
{
    public class BoolToOppositeBoolConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( targetType != typeof( bool ) )
            {
                throw new InvalidOperationException( "The target must be a boolean" );
            }

            // Allow for the byte data type.
            if( value.ToString() == "0" ) { value = false; }
            if( value.ToString() == "1" ) { value = true;  }

            return !(bool)value;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
