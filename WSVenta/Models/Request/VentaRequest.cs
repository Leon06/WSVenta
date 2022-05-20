using System.ComponentModel.DataAnnotations;

namespace WSVenta.Models.Request
{
    public class VentaRequest
    {
        [Required]
        [Range(1, Double.MaxValue, ErrorMessage ="El valor del IdCliente debe ser mayor a 0")]
        [ExisteClienteAttribute(ErrorMessage = "El cliente no existe")]
        public int IdCliente { get; set; }

        [Required]
        [MinLength(1,ErrorMessage ="Deben existir conceptos")]
        public List<Concepto> Conceptos { get; set; }

        public VentaRequest()
        {
            this.Conceptos = new List<Concepto>();
        }
    }

    public class Concepto
    {
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Importe { get; set; }
        public int IdProdcuto { get; set; }
    }

    //Crear nuestra propia validacion 

    #region Validaciones
    public class ExisteClienteAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {   
            int idCliente = (int)value;
            using (var db = new Models.SistemaVentaContext())
            {
                if (db.Clientes.Find(idCliente) == null) return false;
            }
            return true;
        }
    }

    #endregion
}
