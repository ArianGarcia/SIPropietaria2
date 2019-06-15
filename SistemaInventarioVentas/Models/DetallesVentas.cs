using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaInventarioVentas.Models
{
    public class DetalleVenta
    {
        [Required(ErrorMessage = "El id del producto es obligatorio")]
        public int ProductoID { get; set; }
        public virtual Producto Producto { get; set; }
        [Required(ErrorMessage = "El id de la venta es obligatorio")]
        public int VentaID { get; set; }
        public virtual Venta Venta { get; set; }
        public float PrecioUnitario { get; set; }
        public float SubTotal { get; set; }
        public int Cantidad { get; set; }
    }
}