using AppEstudioContable.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiciosEC.Interfaces.Managers;
using ServiciosEC.Managers; 
using ServiciosEC.Models;
using ServiciosEC.Utilidades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace AppEstudioContable.Controllers
{
    public class ComprasController : Controller
    {
        private readonly ECContext _context;
        private readonly IClienteManager _clienteManager;
        private readonly ICompraManager _compraManager;
        

        public ComprasController(ECContext context, IClienteManager clienteManager, ICompraManager compraManager)
        {
            _context = context;
            _clienteManager = clienteManager;
            _compraManager = compraManager;
            
        }


        [HttpGet]
        [Route("{controller}/lista/{cuit?}/{anio?}/{mes?}")]
        public async Task<IActionResult> Index(string? cuit = null, int mes = 0, int anio = 0, CancellationToken cancellationToken = default)
        {

            if (string.IsNullOrEmpty(cuit))
                return View("Error", new ErrorViewModel { Message = "Debe elegir un cliente para ver las Compras." });

            var cliente = await _clienteManager.ObtenerClientePorCuitAsync(cuit, cancellationToken);
            if (cliente == null)
                return View("Error", new ErrorViewModel { Message = $"No existe cliente con el CUIT: {cuit}" });


            ViewBag.Cuit = cuit;
            ViewBag.MesSeleccionado = mes;
            ViewBag.AnioSeleccionado = anio;
            ViewBag.Id = cliente.IdPersona;

            var meses = new[]
            {
                "Todos los meses", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
            };

            ViewBag.Meses = meses
                .Select((m, i) => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = m,
                    Selected = (i == mes)
                })
                .ToList();

            var anios = Enumerable.Range(DateTime.Now.Year - 5, 7)
                                 .OrderByDescending(x => x)
                                 .Select(y => new SelectListItem
                                 {
                                     Value = y.ToString(),
                                     Text = y.ToString(),
                                     Selected = (y == anio)
                                 }).ToList();

            anios.Insert(0, new SelectListItem { Value = "0", Text = "Todos los años", Selected = (anio == 0) });
            ViewBag.Anos = anios;

            if (anio != 0)
            {
                var listaCompras = await _compraManager.ObtenerComprasPorClientePeriodoAsync(cliente.IdPersona, mes, anio, cancellationToken);

                return View(listaCompras);
            }
            else
            {
                return View();
            }
        }


        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id, string cuit)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.Estado)
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (compra == null)
            {
                return NotFound();
            }

            ViewBag.Cuit = cuit;

            return View(compra);
        }

        // GET: Compras/Create
        public IActionResult Create(int id)
        {
            ViewBag.id = id;

            var model = new CompraModel
            {
                id = id,
            };

            return View(model);
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompraModel model, CancellationToken cancellationToken)
        {
            try
            {
                Cliente cliente = await _clienteManager.ObtenerPorId(model.id, cancellationToken);

                Compra compraNueva = new Compra
                {
                    IdPersona = model.id,
                    Fecha = model.Fecha,
                    TipoFact = model.TipoFact,
                    PuntoVenta = model.PuntoVenta,
                    NroDesde = model.NroDesde,
                    NroHasta = model.NroHasta,
                    TipoDocVendedor = model.TipoDocVendedor,
                    NroDocVendedor = model.NroDocVendedor,
                    DenomVendedor = model.DenomVendedor,
                    TipoCambio = model.TipoCambio,
                    Moneda = model.Moneda,
                    NetoGravado = model.NetoGravado,
                    NoGravado = model.NoGravado,
                    Exento = model.Exento,
                    Iva = model.Iva,
                    Total = model.Total,
                    Grav0 = model.Grav0,
                    Grav25 = model.Grav25,
                    Grav5 = model.Grav5,
                    Grav105 = model.Grav105,
                    Grav21 = model.Grav21,
                    Grav27 = model.Grav27,
                    Iva0 = model.Iva0,
                    Iva105 = model.Iva105,
                    Iva21 = model.Iva21,
                    Iva25 = model.Iva25,
                    Iva5 = model.Iva5,
                    Iva27 = model.Iva27,

                };

                //if (await _compraManager.ExisteCompraEnFecha(compraNueva.Fecha, cancellationToken))
                //{
                //    ModelState.AddModelError("CompraExistente", "Ya existe una compra con los mismos datos.");
                //    return View(model);
                //}

                if (!await _compraManager.ValidarTotales(compraNueva, cancellationToken))
                {
                    ModelState.AddModelError("Error", "Los totales de la compra no son válidos.");
                    return View(model);
                }
                if (!await _compraManager.ValidacionIvaGravadoDesglosado(compraNueva, cancellationToken))
                {
                    ModelState.AddModelError("Error", "Los montos de Iva y/o Gravados no coinciden con el total.");
                    return View(model);
                }
                if (ModelState.IsValid)
                {
                    await _compraManager.Insertar(compraNueva, cancellationToken);
                    TempData["MensajeExito"] = "Compra creada correctamente.";
                    return RedirectToAction(nameof(Index), new { cuit = cliente.Cuit });
                }
                return View(compraNueva);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", $"Ocurrió un error al crear la compra: {ex.Message}");
                ViewBag.id = model.id;
                return View(model);
            }
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int id, string cuit, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }
            var compra = await _compraManager.ObtenerPorId(id, cancellationToken);

            CompraModel model = new CompraModel
            {
                id = compra.IdPersona,
                IdCompra = compra.IdCompra,
                Fecha = compra.Fecha,
                TipoFact = compra.TipoFact,
                PuntoVenta = compra.PuntoVenta,
                NroDesde = compra.NroDesde,
                NroHasta = compra.NroHasta,
                TipoDocVendedor = compra.TipoDocVendedor,
                NroDocVendedor = compra.NroDocVendedor,
                DenomVendedor = compra.DenomVendedor,
                TipoCambio = compra.TipoCambio,
                Moneda = compra.Moneda,
                NetoGravado = compra.NetoGravado,
                NoGravado = compra.NoGravado,
                Exento = compra.Exento,
                Iva = compra.Iva,
                Total = compra.Total,
                Grav0 = compra.Grav0,
                Grav25 = compra.Grav25,
                Grav5 = compra.Grav5,
                Grav105 = compra.Grav105,
                Grav21 = compra.Grav21,
                Grav27 = compra.Grav27,
                Iva0 = compra.Iva0,
                Iva105 = compra.Iva105,
                Iva21 = compra.Iva21,
                Iva27 = compra.Iva27,
                Iva25 = compra.Iva25,
                Iva5 = compra.Iva5
            };

            ViewBag.cuit = cuit;
            return View(model);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompraModel model,string cuit, CancellationToken cancellationToken)
        {
            try
            {
                Compra compraActual = await _compraManager.ObtenerPorId(model.IdCompra, cancellationToken);

                if (compraActual == null)
                {
                    ModelState.AddModelError("Error", "No se encontró un compraActual con el ID proporcionado.");
                    return View(model);
                }

                compraActual.Fecha = model.Fecha;
                compraActual.TipoFact = model.TipoFact;
                compraActual.PuntoVenta = model.PuntoVenta;
                compraActual.NroDesde = model.NroDesde;
                compraActual.NroHasta = model.NroHasta;
                compraActual.TipoDocVendedor = model.TipoDocVendedor;
                compraActual.NroDocVendedor = model.NroDocVendedor;
                compraActual.DenomVendedor = model.DenomVendedor;
                compraActual.TipoCambio = model.TipoCambio;
                compraActual.Moneda = model.Moneda;
                compraActual.NetoGravado = model.NetoGravado;
                compraActual.NoGravado = model.NoGravado;
                compraActual.Exento = model.Exento;
                compraActual.Iva = model.Iva;
                compraActual.Total = model.Total;
                compraActual.Iva0 = model.Iva0;
                compraActual.Iva105 = model.Iva105;
                compraActual.Iva21 = model.Iva21;
                compraActual.Iva25 = model.Iva25;
                compraActual.Iva5 = model.Iva5;
                compraActual.Iva27 = model.Iva27;
                compraActual.Grav0 = model.Grav0;
                compraActual.Grav25 = model.Grav25;
                compraActual.Grav5 = model.Grav5;
                compraActual.Grav105 = model.Grav105;
                compraActual.Grav21 = model.Grav21;
                compraActual.Grav27 = model.Grav27;


                Cliente cliente = await _clienteManager.ObtenerPorId(compraActual.IdPersona, cancellationToken);

                ViewBag.id = model.id;
                ViewBag.cuit = cuit;
                if (!await _compraManager.ValidarTotales(compraActual, cancellationToken))
                {
                    ModelState.AddModelError("Error", "Los totales de la compra no son válidos.");
                    return View(model);
                }

                if (!await _compraManager.ValidacionIvaGravadoDesglosado(compraActual, cancellationToken))
                {
                    ModelState.AddModelError("Error", "Los montos de Iva y/o Gravados no coinciden con el total.");
                    return View(model);
                }



                await _compraManager.Editar(compraActual, cancellationToken);
                TempData["MensajeExito"] = "Compra editada correctamente.";
                return RedirectToAction(nameof(Index), new { cuit = cuit });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", $"Ocurrió un error al editar la compra: {ex.Message}");
                ViewBag.id = model.id;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit2(CompraModel model, CancellationToken cancellationToken)
        {
            try
            {
                var compra = await _compraManager.ObtenerPorId(model.IdCompra, cancellationToken);
                if (compra == null)
                {
                    return NotFound();
                }

                compra.IdCompra = model.IdCompra;
                compra.Fecha = model.Fecha;
                compra.TipoFact = model.TipoFact;
                compra.PuntoVenta = model.PuntoVenta;
                compra.NroDesde = model.NroDesde;
                compra.NroHasta = model.NroHasta;
                compra.TipoDocVendedor = model.TipoDocVendedor;
                compra.NroDocVendedor = model.NroDocVendedor;
                compra.DenomVendedor = model.DenomVendedor;
                compra.TipoCambio = model.TipoCambio;
                compra.Moneda = model.Moneda;
                compra.NetoGravado = model.NetoGravado;
                compra.NoGravado = model.NoGravado;
                compra.Exento = model.Exento;
                compra.Iva = model.Iva;
                compra.Iva0 = model.Iva0;
                compra.Iva25 = model.Iva25;
                compra.Iva5 = model.Iva5;
                compra.Iva105 = model.Iva105;
                compra.Iva21 = model.Iva21;
                compra.Iva27 = model.Iva27;
                compra.Grav0 = model.Grav0;
                compra.Grav25 = model.Grav25;
                compra.Grav5 = model.Grav5;
                compra.Grav105 = model.Grav105;
                compra.Grav21 = model.Grav21;
                compra.Grav27 = model.Grav27;
                compra.Total = model.Total;

                await _compraManager.Editar(compra, cancellationToken);

                TempData["MensajeExito"] = "Venta editada correctamente.";
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al editar la compra: {ex.Message}" });
            }

        }



        

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int id, string cuit, CancellationToken cancellationToken)
        {
            var compra = await _compraManager.ObtenerPorId(id, cancellationToken);
            if (compra == null)
            {
                return NotFound();
            }
            ViewBag.Cuit = cuit;
            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string cuit, CancellationToken cancellationToken)
        {
            try
            {
                ViewBag.Cuit = cuit;
                await _compraManager.Borrar(id, cancellationToken);
                TempData["MensajeExito"] = "Venta eliminada correctamente.";
                return RedirectToAction(nameof(Index), new { cuit = cuit });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al eliminar la compra: {ex.Message}");
                ViewBag.Cuit = cuit;
                var compra = await _compraManager.ObtenerPorId(id, cancellationToken);
                return View("Delete", compra);
            }
        }

        private bool CompraExists(int id)
        {
            return _context.Compras.Any(e => e.IdCompra == id);
        }


        [HttpGet]
        public async Task<IActionResult> VerTotales(DateOnly fechaDesde, DateOnly fechaHasta, CancellationToken cancellationToken)
        {
            
            if (fechaDesde > fechaHasta)
            {
                ModelState.AddModelError("FechaInvalida", "La fecha 'Desde' no puede ser posterior a la fecha 'Hasta'.");
             
                return View("~/Views/Compras/Index.cshtml", await _context.Compras.Include(c => c.Estado).Where(c => c.EstadoId == (int)ECContext.EstadosEnum.Activo).ToListAsync());
            }

            try
            {
                
                var compras = await _compraManager.ObtenerTodasLasComprasPorFechas(fechaDesde, fechaHasta, cancellationToken);

                var totalesIVA = Calculadora.CalcularTotalesDict(compras);

                return View("~/Views/Compras/VerTotales.cshtml", totalesIVA);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ErrorTotales", $"Error al calcular totales: {ex.Message}");
                
                return View("~/Views/Compras/Index.cshtml", await _context.Compras.Include(c => c.Estado).Where(c => c.EstadoId == (int)ECContext.EstadosEnum.Activo).ToListAsync());
            }
        }

        // GET: Compras: Altas
        public async Task<IActionResult> Altas(int id, CancellationToken cancellationToken)
        {

            CompraViewModel model = new CompraViewModel
            {
                id = id,
            };

            var cliente = await _clienteManager.ObtenerPorId(id, cancellationToken);
            if (cliente == null)
            {
                TempData["ErrorMessage"] = $"No se encontró cliente con cuit: {id}";
                return View(model);
            }


            return View(model);
        }

        // POST: Compras: Altas
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Altas(CompraViewModel model, List<string> tiposFacturaExcluir, IFormFile file, [FromServices] IWebHostEnvironment hostingEnvironment, CancellationToken cancellationToken)
        {

            var id = model.id;

            var cliente = await _clienteManager.ObtenerPorId(model.id, cancellationToken);

            if (cliente == null)
            {
                ModelState.AddModelError("", $"No se encontró cliente con cuit: {id}");
                return View(model);
            }

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Por favor, selecciona un archivo para cargar.");
                return View(model);
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var extensionesPermitidas = new[] { ".xlsx", ".xls", ".csv" };

            if (!extensionesPermitidas.Contains(extension))
            {
                ModelState.AddModelError("", "Solo se permiten archivos con extensión .xlsx, .xls o .csv");
                return View(model);
            }

            try
            {
                

                using var stream = file.OpenReadStream();

                var tiposSeparados = new List<string>();

                if (tiposFacturaExcluir != null)
                {
                    foreach (var tipoFactura in tiposFacturaExcluir)
                    {
                        tiposSeparados.AddRange(tipoFactura.Split(','));
                    }
                }

                Debug.WriteLine(tiposSeparados);

                
                var (comprasCorrectas, comprasParaRevisar, comprasFallidas, excelValido) = await _compraManager.AgregarCompras(stream, id, cliente.Cuit, tiposSeparados, cancellationToken);


                if (!excelValido)
                {
                    TempData["ErrorMessage"] = "Error: Cuit incorrecto o excel no corresponde a Compras";
                    return View(model);
                }
                model.comprasCorrectas = comprasCorrectas.ToList();
                model.comprasParaRevisar = comprasParaRevisar.ToList();
                model.comprasFallidas = comprasFallidas.ToList();

                if (!comprasCorrectas.Any() && !comprasParaRevisar.Any())
                {
                    TempData["ErrorMessage"] = "No hay Compras para agregar.";
                    return View(model);
                }

                
                TempData["SuccessMessage"] = "Compras cargadas exitosamente.";
                return View(model);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error al procesar el archivo: {ex.Message}";
                return View(model);
            }

        }



    }
}
