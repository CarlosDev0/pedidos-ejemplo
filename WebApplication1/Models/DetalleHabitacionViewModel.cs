using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class DetalleHabitacionViewModel
    {
        public string id { get; set; }
        public string habitacion { get; set; }
        public string TipoItemId { get; set; }
        public string detalle { get; set; }
        public string Cantidad { get; set; }
        public string numHabitacion { get; set; }
    }
}