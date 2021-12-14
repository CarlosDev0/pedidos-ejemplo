using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ListaReservaViewModel
    {
        public string UsuarioId { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string totalDiasReservados { get; set; }
        public string Alojamiento { get; set; }
        public string Sede { get; set; }
        public string Municipio { get; set; }
        public string Departamento { get; set; }
    }
}