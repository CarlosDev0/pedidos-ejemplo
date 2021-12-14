using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class DefaultConnection : DbContext
    {
        public DbSet<TipoAlojamiento> TipoAlojamiento { get; set; }
        public DbSet<Municipio> Municipio { get; set; }
    }
}