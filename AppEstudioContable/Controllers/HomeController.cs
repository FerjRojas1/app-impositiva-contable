using AppEstudioContable.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiciosEC.Interfaces.Managers;
using System.Diagnostics;

namespace AppEstudioContable.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IClienteManager _clienteManager;
        private IVentaManager _ventaManager;
        private ICompraManager _compraManager;
        public HomeController(ILogger<HomeController> logger, IClienteManager clienteManager, ICompraManager compraManager, IVentaManager ventaManager)
        {
            _logger = logger;
            _clienteManager = clienteManager;
            _ventaManager = ventaManager;
            _compraManager = compraManager;
        }

        public async Task<IActionResult> IndexAsync(CancellationToken cancellationToken)
        {
            int cantidadClientes = await _clienteManager.ObtenerCantidad(cancellationToken);
            var cantidadComprobantes = await _compraManager.ObtenerCantidad(cancellationToken) + await _ventaManager.ObtenerCantidad(cancellationToken);

            ViewBag.CantidadClientes = cantidadClientes;
            ViewBag.CantidadComprobantes = cantidadComprobantes;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error403()
        {
            var msg = "No tenés permiso para acceder a esta página.";

            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = msg });
        }
    }
}
