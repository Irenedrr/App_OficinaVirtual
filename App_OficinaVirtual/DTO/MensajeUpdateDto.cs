﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_OficinaVirtual.DTO;

public class MensajeUpdateDto
{
    public string Contenido { get; set; }
    public int? ReceptorId { get; set; }
    public DateTime? Fecha { get; set; }
}
