using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaInventarioVentas.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }

        [Required(ErrorMessage = "El nombre del producto no puede estar vacio")]
        [MinLength(3, ErrorMessage = "El nombre debe contener al menos 3 caracteres")]
        [MaxLength(20, ErrorMessage = "El nombre es demasiado largo , debe ser 20 caracteres o menos")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe introducir el precio de compra")]
        [Range(1,int.MaxValue,ErrorMessage ="El precio debe ser mayor que 0")]
        public float PrecioCompra { get; set; }

        [Required(ErrorMessage = "Debe introducir el precio de venta")]
        [Range(1, int.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
        public float PrecioVenta { get; set; }

        [Required(ErrorMessage = "Debe introducir la cantidad del inventario")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }

        [Required(ErrorMessage = "Debe de elegir una categoria")]
        public int CategoriaID { get; set; }
        public virtual Categoria Categoria { get; set; }

        public virtual Almacen Almacen { get; set; }
        [Required(ErrorMessage = "Debe de elegir un almacen")]
        public int AlmacenID { get; set; }
    }
}