using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Municipio")]
    public class Municipio
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Departamento { get; set; }
        public bool Activo { get; set; }
    }
}