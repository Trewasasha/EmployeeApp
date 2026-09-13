using System;
using System.Globalization;
using Xamarin.Forms;

namespace EmployeeApp.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrEmpty(str))
            {
                return Color.FromHex("#FFEBEE"); // Светло-красный для ошибки
            }
            return Color.Transparent; // Без фона
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}