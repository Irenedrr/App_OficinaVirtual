using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_OficinaVirtual.Models;

public class Rol
{
    public int Id { get; set; }
    public string Nombre { get; set; }

    public List<Usuario> Usuarios { get; set; }
}
