using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_OficinaVirtual.Converters;

public class EmisorColorConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int emisorId = (int)value;
        int miId = Preferences.Get("usuario_id", -1);
        return emisorId == miId ? Colors.LightGreen : Colors.White;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
