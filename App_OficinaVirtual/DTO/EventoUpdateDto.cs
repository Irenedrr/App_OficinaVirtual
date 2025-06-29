﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_OficinaVirtual.DTO;

public class EventoUpdateDto
{
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string Tipo { get; set; }
    public List<int> Participantes { get; set; }
}

