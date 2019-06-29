using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaInventarioVentas.Models.viewmodels
{
    public class GuardarVentaViewModel
    {
        public IEnumerable<DetalleVenta> Productos;
        public string Comentario;
        public float Total;
        public int ClienteID;
    }
}