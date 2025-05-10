using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_OficinaVirtual.DTO;

public class UsuarioUpdateDto
{
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Contrasena { get; set; }
    public int? RolId { get; set; }
    public string ImagenUrl { get; set; }

    public string Personaje { get; set; }

    public string Oficina { get; set; }
    public string Estado { get; set; }
}
