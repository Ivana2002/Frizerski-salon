using System;
using System.Globalization;
using System.Windows.Data;
using System.Resources;

namespace IvanaDrugi.Resources
{
    public class LocalizationConverter : IValueConverter
    {
        private static readonly ResourceManager _resourceManager =
            new ResourceManager("IvanaDrugi.Resources.Strings", typeof(LocalizationConverter).Assembly);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string key)
            {
                var lang = LocalizationManager.CurrentLanguage;
                return _resourceManager.GetString(key, new CultureInfo(lang)) ?? $"[{key}]";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}