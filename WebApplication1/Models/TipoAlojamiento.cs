using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("TipoAlojamiento")]
    public class TipoAlojamiento
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }

}