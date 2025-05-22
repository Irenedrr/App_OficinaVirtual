using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_OficinaVirtual.DTO;
using App_OficinaVirtual.ViewModels;

namespace App_OficinaVirtual.Converters;

public class EmisorNombreConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int emisorId = (int)value;
        int miId = Preferences.Get("usuario_id", -1);

        if (emisorId == miId)
            return "Tú";

        if (parameter is ObservableCollection<UsuarioResponseDto> usuarios)
        {
            var usuario = usuarios.FirstOrDefault(u => u.Id == emisorId);
            return usuario?.Nombre ?? $"Usuario {emisorId}";
        }

        return $"Usuario {emisorId}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

