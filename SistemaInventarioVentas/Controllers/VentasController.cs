using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaInventarioVentas.Models;
using SistemaInventarioVentas.Models.viewmodels;
using Newtonsoft.Json;
namespace SistemaInventarioVentas.Controllers
{
    [Authorize]
    public class VentasController : Controller
    {
        private SistemaInventarioDBContext db = new SistemaInventarioDBContext();

        // GET: Ventas
        public ActionResult Index()
        {
            var ventas = db.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetallesVentas)
                .ToList();
            return View(ventas);
        }

        // GET: Ventas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        [Route("Ventas/GetClientes")]
        public ActionResult GetClientes(string param)
        {
            if(string.IsNullOrWhiteSpace(param))
                return Json(db.Clientes.ToList(), JsonRequestBehavior.AllowGet);

            List<Cliente> clientes = db.Clientes
                    .Where(c => c.Nombre.Contains(param) || c.Cedula.Contains(param))
                    .ToList();
            return Json(clientes, JsonRequestBehavior.AllowGet);
        }

        [Route("Ventas/GetProductos")]
        public ActionResult GetProductos(string param)
        {
            List<object> data = null;

            if (string.IsNullOrWhiteSpace(param))
            {
                List<Producto> p = db.Productos.ToList();
                data = ModearData(p);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
                
            List<Producto> productos = db.Productos.Where(p => p.Estado == true && p.Cantidad > 0 &&
                     p.Nombre.ToLower().IndexOf(param.ToLower()) > -1).ToList();

            data = ModearData(productos);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private List<Object> ModearData(List<Producto> productos)
        {
            List<object> data = new List<object>();
            foreach (Producto producto in productos)
            {
                data.Add(new
                {
                    producto.ProductoID,
                    producto.Nombre
                });
            }
            return data;
        }

        [Route("Ventas/GetProducto")]
        public ActionResult GetProducto(int id)
        {
            Producto producto = db.Productos.SingleOrDefault(p => p.ProductoID == id && p.Estado == true);
            if (producto == null)
                return Json("0", JsonRequestBehavior.AllowGet);
            else
                return Json(new {
                    producto.ProductoID,
                    producto.PrecioVenta,
                    producto.Cantidad,
                    producto.Nombre
                }, JsonRequestBehavior.AllowGet);
        }

        // GET: Ventas/Create
        public ActionResult Create()
        {
            VentaViewModel venta = new VentaViewModel()
            {
                Productos = new List<Producto>(),
                Clientes = new List<Cliente>(),
                Comentario = ""
            };

            return View(venta);
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Ventas/Create")]
        public ActionResult Create(string data)
        {
            GuardarVentaViewModel venta = JsonConvert.DeserializeObject<GuardarVentaViewModel>(data);
            Cliente cliente = db.Clientes.Find(venta.ClienteID);
            Producto producto = new Producto();
            Venta nuevaVenta = new Venta() {
                DetallesVentas = new List<DetalleVenta>()
            };
            List<DetalleVenta> detalles = new List<DetalleVenta>();


            nuevaVenta.Comentario = venta.Comentario;
            nuevaVenta.Estado = true;
            nuevaVenta.Cliente = cliente;
            nuevaVenta.FechaVenta = DateTime.UtcNow;
            nuevaVenta.Total = venta.Total;

            foreach (DetalleVenta detalleVenta in venta.Productos)
            {
                producto = db.Productos.Find(detalleVenta.ProductoID);
                producto.Cantidad -= detalleVenta.Cantidad;
                nuevaVenta.DetallesVentas.Add(detalleVenta);
            }

            try
            {
                db.Ventas.Add(nuevaVenta);
                db.SaveChanges();
                return Json(new { success = "1", id = nuevaVenta.VentaID });
            }
            catch (Exception)
            {
                return Json("0");
            }
        }

        // GET: Ventas/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Venta venta = db.Ventas.Find(id);
        //    if (venta == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(venta);
        //}

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "VentaID,FechaVenta,Total,Estado,Comentario,ClientID")] Venta venta)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(venta).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(venta);
        //}

        //// GET: Ventas/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Venta venta = db.Ventas.Find(id);
        //    if (venta == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(venta);
        //}

        //// POST: Ventas/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Venta venta = db.Ventas.Find(id);
        //    db.Ventas.Remove(venta);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
