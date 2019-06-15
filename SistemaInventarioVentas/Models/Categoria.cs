using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaInventarioVentas.Models
{
    public class Categoria
    {
        public int CategoriaID { get; set; }

        [Required(ErrorMessage = "Debe introducir un nombre para la categoria")]
        public string Nombre { get; set; }

        public bool Estado { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}