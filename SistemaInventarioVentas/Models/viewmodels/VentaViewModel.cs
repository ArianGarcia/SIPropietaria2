using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaInventarioVentas.Models.viewmodels
{
    public class VentaViewModel
    {
        public IEnumerable<Cliente> Clientes;
        [Required(ErrorMessage = "Debe de elegir al menos un producto")]
        public IEnumerable<Producto> Productos;
        public string Comentario;
    }
}