using WSVenta.Models;
using WSVenta.Models.Request;

namespace WSVenta.Services
{
    public class VentaService : IVentaService
    {
        public void Add(VentaRequest model)
        {
              //EntityFramework
                using (SistemaVentaContext db = new SistemaVentaContext())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var venta = new Ventum();
                            venta.Total = model.Conceptos.Sum(d => d.Cantidad * d.PrecioUnitario);
                            venta.Fecha = DateTime.Now;
                            venta.IdCliente = model.IdCliente;
                            db.Venta.Add(venta);
                            db.SaveChanges(); // al objeto venta se le asigna un Id

                            foreach (var modelConcepto in model.Conceptos)
                            {
                                var concepto = new Models.Concepto();
                                concepto.Cantidad = modelConcepto.Cantidad;
                                concepto.IdProducto = modelConcepto.Cantidad;
                                concepto.PrecioUnitario = modelConcepto.PrecioUnitario;
                                concepto.Importe = modelConcepto.Importe;
                                concepto.IdVenta = venta.Id;
                                db.Conceptos.Add(concepto);
                                db.SaveChanges();
                            }
                            transaction.Commit();
                            


                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw new Exception("Ocurrio un error en la insercion");
                        }

                    }
                }

            
            
        }
    }
}
