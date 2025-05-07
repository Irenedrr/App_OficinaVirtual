using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_OficinaVirtual.Models;

public class Evento
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public  string Descripcion { get; set; }
    public DateTime Fecha { get; set; }
    public string Tipo { get; set; }

    public List<Usuario> Participantes { get; set; }
}
