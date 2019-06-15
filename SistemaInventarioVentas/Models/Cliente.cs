using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SistemaInventarioVentas.Models.validacionesCustomisadas;
namespace SistemaInventarioVentas.Models
{
    public class Cliente
    {
        public int ClienteID { get; set; }

        [Required(ErrorMessage = "Debe introducir el apellido del cliente")]
        [MinLength(3, ErrorMessage = "El apellido debe tener al menos 3 caracteres")]
        [MaxLength(20, ErrorMessage = "El apellido debe ser de 20 o menos caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debe introducir un nombre")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
        [MaxLength(20, ErrorMessage = "El nombre debe ser de 20 o menos caracteres")]
        public string Nombre { get; set; }


        [ValidacionCedula]
        [Required(ErrorMessage = "El campo cedula es obligatorio")]
        [RegularExpression(@"\b\d{3}\-?\d{7}\-?\d{1}\b", ErrorMessage = "la cedula no es valida")]
        public string Cedula { get; set; }

        public ICollection<Venta> Ventas { get; set; }

    }
}