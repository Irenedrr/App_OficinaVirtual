using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_OficinaVirtual.Models;

public class Mensaje
{
    public int Id { get; set; }
    public string Contenido { get; set; }
    public DateTime Fecha { get; set; }

    public int EmisorId { get; set; }
    public int ReceptorId { get; set; }

    public Usuario Emisor { get; set; }
    public Usuario Receptor { get; set; }
}

