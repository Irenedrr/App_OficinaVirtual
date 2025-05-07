using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App_OficinaVirtual.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string ImagenUrl { get; set; }
    public string Contrasena { get; set; }
    public string Estado { get; set; }

    public int? RolId { get; set; }
    public Rol Rol { get; set; }

    public List<Evento> Eventos { get; set; }
    public List<Mensaje> Mensajes { get; set; }
}
