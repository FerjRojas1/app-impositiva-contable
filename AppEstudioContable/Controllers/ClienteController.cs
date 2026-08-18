//using AspNetCoreGeneratedDocument;
using AppEstudioContable.Models; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiciosEC.Managers;
using ServiciosEC.Models; 
using static ServiciosEC.Models.ECContext;
using System.Threading;
using System;
using ServiciosEC.Interfaces.Managers;

namespace AppEstudioContable.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteManager _clienteManager;
        private readonly IVentaManager _ventaManager; 
        private readonly ICompraManager _compraManager;
        private readonly EstadoManager _estadoManager;
        private readonly PersonaManager _personaManager;

        
        public ClienteController(IClienteManager clienteManager, IVentaManager ventaManager, ICompraManager compraManager, EstadoManager estadoManager, PersonaManager personaManager)
        {
            _clienteManager = clienteManager;
            _ventaManager = ventaManager; 
            _compraManager = compraManager; 
            _estadoManager = estadoManager;
            _personaManager = personaManager;
        }

        // GET: ClienteController
        public async Task<IActionResult> Index(string filtro, CancellationToken cancellationToken)
        {
            
            var clientes = await _clienteManager.ObtenerTodos(cancellationToken);
            
            ViewBag.Filtro = filtro;
            return View(clientes);
        }

        // GET: ClienteController/Details/5
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var cliente = await _clienteManager.ObtenerPorId(id, cancellationToken);
            if (cliente == null)
            {
                return NotFound();
            }

            // Mapeo de ServiciosEC.Models.Cliente a AppEstudioContable.Models.ClienteModel
            ClienteModel clienteModel = new ClienteModel
            {
                id = cliente.IdPersona,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Dni = cliente.Dni,
                Cuit = cliente.Cuit, 
                DomFiscal = cliente.DomFiscal,
                RazonSocial = cliente.RazonSocial,
                Fecha = cliente.FechaAlta, 
                Email = cliente.Email,
                EstadoId = cliente.EstadoId,
                Estado = cliente.Estado?.Descripcion,
                Estados = await _clienteManager.ObtenerEstadosAsync(cancellationToken)
            };
            return View(clienteModel);
        }

        // GET: ClienteController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,Dni,Cuit,DomFiscal,RazonSocial,Email")] ClienteModel model, CancellationToken cancellationToken)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    
                    ServiciosEC.Models.Cliente cliente = new ServiciosEC.Models.Cliente
                    {
                        Nombre = model.Nombre,
                        Apellido = model.Apellido,
                        Dni = model.Dni,
                        Cuit = model.Cuit,
                        DomFiscal = model.DomFiscal,
                        RazonSocial = model.RazonSocial,
                        Email = model.Email, 
                        RolId = (int)RolesEnum.Cliente,
                    };

                    if (await _clienteManager.ExisteCliente(cliente, cancellationToken))
                    {
                        ModelState.AddModelError("", "El cliente ya existe (DNI o CUIT duplicado)."); 
                        return View(model); 
                    }

                    await _clienteManager.Insertar(cliente, cancellationToken);
                    TempData["SuccessMessage"] = "Cliente agregado exitosamente."; 
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al guardar el cliente: {ex.Message}");
                   
                    Console.WriteLine($"Error al crear cliente: {ex.ToString()}");
                }
            }
           
            return View(model); 
        }

        // GET: ClienteController/Edit/5
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var cliente = await _clienteManager.ObtenerPorId(id, cancellationToken);
            if (cliente == null)
            {
                return NotFound();
            }

            
            ClienteModel clienteModel = new ClienteModel
            {
                id = cliente.IdPersona,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Dni = cliente.Dni,
                Cuit = cliente.Cuit,
                DomFiscal = cliente.DomFiscal,
                RazonSocial = cliente.RazonSocial,
                Email = cliente.Email,
                EstadoId = cliente.EstadoId,
                Estado = cliente.Estado.Descripcion,
                Estados = await _clienteManager.ObtenerEstadosAsync(cancellationToken)
            };
            return View(clienteModel);
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteModel model, CancellationToken cancellationToken)
        {
            if (id != model.id) 
            {
                return NotFound();
            }

            
            if (ModelState.IsValid)
            {
                try
                {
                    var cliente = await _clienteManager.ObtenerPorId(id, cancellationToken);
                    if (cliente == null)
                    {
                        return NotFound();
                    }

                   
                    bool cambioDni = cliente.Dni != model.Dni;
                    bool cambioCuit = cliente.Cuit != model.Cuit; 

                    cliente.Nombre = model.Nombre;
                    cliente.Apellido = model.Apellido;
                    cliente.Dni = model.Dni;
                    cliente.Cuit = model.Cuit;
                    cliente.DomFiscal = model.DomFiscal;
                    cliente.RazonSocial = model.RazonSocial;
                    cliente.Email = model.Email; 
                    cliente.EstadoId = model.EstadoId;


                    
                    if ((cambioDni || cambioCuit) && await _clienteManager.ExisteCliente(cliente, cancellationToken))
                    {
                        ModelState.AddModelError("", "Ya existe un cliente con el mismo DNI o CUIT.");
                        return View(model); 
                    }

                    await _clienteManager.Editar(cliente, cancellationToken);
                    TempData["SuccessMessage"] = "Cliente actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException) 
                {
                   
                    if (!await _clienteManager.Existe(model.id, cancellationToken)) 
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Ocurrió un error: {ex.Message}");
                    Console.WriteLine($"Error al editar cliente: {ex.ToString()}"); 
                }
            }
            
            
            return View(model);
        }

        // GET: ClienteController/Delete/5
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var cliente = await _clienteManager.ObtenerPorId(id, cancellationToken);
            if (cliente == null)
            {
                return NotFound();
            }

           
            ClienteModel clienteModel = new ClienteModel
            {
                id = cliente.IdPersona,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                RazonSocial = cliente.RazonSocial,
                DomFiscal = cliente.DomFiscal,
                Cuit= cliente.Cuit,
                Email= cliente.Email,   
                Dni= cliente.Dni,
               
            };
            return View(clienteModel); 
        }

        // POST: ClienteController/Delete/5
        [HttpPost, ActionName("Delete")] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken) 
        {
            try
            {
                await _clienteManager.Borrar(id, cancellationToken);
                TempData["SuccessMessage"] = "Cliente eliminado exitosamente."; 
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", $"Error al eliminar el cliente: {ex.Message}");
                
                var cliente = await _clienteManager.ObtenerPorId(id, cancellationToken);
                ClienteModel clienteModel = new ClienteModel
                {
                    id = cliente.IdPersona,
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    
                };
                return View(clienteModel); 
            }
        }


        [HttpGet]
        public async Task<JsonResult> ObtenerDatosGraficaVentasCompras(int idCliente, int ano, CancellationToken cancellationToken)
        {
       
            var cliente = await _clienteManager.ObtenerPorId(idCliente, cancellationToken);
            if (cliente == null)
            {
                return Json(new { success = false, message = "Cliente no encontrado." });
            }

        
            var resumenVentas = await _ventaManager.ObtenerIVAMensualPorCliente(cliente, ano);

          
            var resumenCompras = await _compraManager.ObtenerIVAMensualPorCliente(cliente, ano);


            var datosVentas = resumenVentas.PorMes.Select(t => t.TotalGeneral).ToArray();
            var datosCompras = resumenCompras.PorMes.Select(t => t.TotalGeneral).ToArray();


            return Json(new
            {
                success = true,
                ventas = datosVentas,
                compras = datosCompras
            });
        }



        // GET: ClienteController
        public async Task<IActionResult> VerInactivos(CancellationToken cancellationToken)
        {

            var clientes = await _clienteManager.ObtenerInactivosAsync(cancellationToken);

            return View(clientes);
        }
    }
}