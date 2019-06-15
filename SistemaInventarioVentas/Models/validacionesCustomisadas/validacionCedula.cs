using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaInventarioVentas.Models.validacionesCustomisadas
{
    public class ValidacionCedula : ValidationAttribute
    {
        private readonly SistemaInventarioDBContext _context;
        public ValidacionCedula()
        {
            _context = new SistemaInventarioDBContext();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var cliente = (Cliente)validationContext.ObjectInstance;
            var clienteEnDB = _context.Clientes.SingleOrDefault(c => c.Cedula == cliente.Cedula);


            if (clienteEnDB != null && clienteEnDB.ClienteID != cliente.ClienteID)
                return new ValidationResult("Ya existe un cliente con esta cedula");

            return ValidationResult.Success;
        }
    }
}