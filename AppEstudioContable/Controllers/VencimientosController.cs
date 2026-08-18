using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace AppEstudioContable.Controllers
{
    public class VencimientosController : Controller
    {
        // GET: VencimientosController
        public ActionResult Index()
        {
            ViewBag.Meses = Enumerable.Range(1, 12).Select(i => new SelectListItem
            {
                Value = i.ToString("00"),
                Text = new DateTime(2000, i, 1).ToString("MMMM", new CultureInfo("es-ES")).ToUpper()
            }).ToList();
            ViewBag.Impuestos = new List<SelectListItem>
            {
                new SelectListItem { Value = "IVA", Text = "IVA" },
                new SelectListItem { Value = "IIBB", Text = "Ingresos Brutos" },
        
            };




            return View();
        }

        // GET: VencimientosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VencimientosController/Create
        public ActionResult Create()
        {
            ViewBag.Impuestos = new List<SelectListItem>
            {
                new SelectListItem { Value = "IVA", Text = "IVA" },
                new SelectListItem { Value = "IIBB", Text = "Ingresos Brutos" },
                new SelectListItem { Value = "GAN", Text = "Ganancias" },                
            };



            return View();
        }

        // POST: VencimientosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VencimientosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VencimientosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VencimientosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VencimientosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
