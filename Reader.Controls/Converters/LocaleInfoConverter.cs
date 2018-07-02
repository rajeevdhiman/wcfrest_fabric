using FabricWCF.Common.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Reader.Controls.Converters
{
    public class LocaleInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var li = value as LocaleInfo;
            if (li != null)
            {
                return li.Region + (string.IsNullOrWhiteSpace(li.Language) ? "" : $" ({li.Language})");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
