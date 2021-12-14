using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ReservaViewModel
    {
        public int Id { get; set; }
        public string Lugar { get; set; }
        public string Alojamiento { get; set; }
        public string TipoAlojamiento { get; set; }
        public string CapacidadMaxima { get; set; }
        public string Valor { get; set; }
    }

}