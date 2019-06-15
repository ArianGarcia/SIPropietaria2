using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaInventarioVentas.Models
{
    public class Almacen
    {
        public int AlmacenID { get; set; }
        [Required(ErrorMessage = "El campo nombre no puede ir vacio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo codigo Almacen no puede ir vacio")]
        public string CodigoAlmacen { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}