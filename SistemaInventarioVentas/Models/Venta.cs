using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaInventarioVentas.Models
{
    public class Venta
    {
        public int VentaID { get; set; }
        public DateTime FechaVenta { get; set; }
        public float Total { get; set; }
        public bool Estado { get; set; }
        public string Comentario { get; set; }

        [Required(ErrorMessage ="El id del cliente es obligatorio")]
        public int ClientID { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<DetalleVenta> DetallesVentas { get; set; }
    }
}