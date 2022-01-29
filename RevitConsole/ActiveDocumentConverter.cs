﻿using System.Globalization;
using System.Windows.Data;

namespace RevitConsole
{
    class ActiveDocumentConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ( value is DocumentViewModel )
                return value;

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ( value is DocumentViewModel )
                return value;

            return Binding.DoNothing;
        }
    }
}
