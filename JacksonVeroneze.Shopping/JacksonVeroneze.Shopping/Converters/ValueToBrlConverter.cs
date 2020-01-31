using System;
using System.Globalization;
using Xamarin.Forms;

namespace JacksonVeroneze.Shopping.Converters
{
    //
    // Summary:
    //     /// Class responsible for converting. ///
    //
    public class ValueToBrlConverter : IValueConverter
    {
        //
        // Summary:
        //     /// Method responsible to perform the conversion. ///
        //
        // Parameters:
        //   value:
        //     The value param.
        //
        //   targetType:
        //     The targetType param.
        //
        //   parameter:
        //     The parameter param.
        //
        //   culture:
        //     The culture param.
        //
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double total = (double)value;

            return total.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
        }

        //
        // Summary:
        //     /// Method responsible to perform the conversion (inverse). ///
        //
        // Parameters:
        //   value:
        //     The value param.
        //
        //   targetType:
        //     The targetType param.
        //
        //   parameter:
        //     The parameter param.
        //
        //   culture:
        //     The culture param.
        //
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
