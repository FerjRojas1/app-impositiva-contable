using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiciosEC.Managers;
using AppEstudioContable.Models;
using Newtonsoft.Json;
using ServiciosEC.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Linq;
using ServiciosEC.Interfaces.Managers;

namespace AppEstudioContable.Controllers
{

    [Authorize(Roles = "Admin")]

    public class UsuariosController : Controller
    {
        private readonly IUsuariosManager _usuarioManager;

        public UsuariosController(IUsuariosManager usuarioManager)
        {
            _usuarioManager = usuarioManager;
        }


        // GET: UsuariosController
        [Authorize(Roles = "Admin, Usuario")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var listaUsuarios = await _usuarioManager.ObtenerTodos(cancellationToken);
            return View(listaUsuarios);
        }

        // GET: UsuariosController/Details/5
        [Authorize(Roles = "Admin, Empleado")]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {

            var usuario = await _usuarioManager.ObtenerPorId(id, cancellationToken);

            if (usuario == null)
            {
                return NotFound();
            }

            
            long.TryParse(usuario.Dni, out long dni);

            UsuarioModel usuarioModel = new UsuarioModel
            {
                idUsuario = usuario.IdPersona,
                nombre = usuario.Nombre,
                apellido = usuario.Apellido,
                dni = usuario.Dni,
                nombre_usuario = usuario.NombreUsuario,
                email = usuario.Email,
                telefono = usuario.Telefono,
                rol = usuario.Rol?.Descripcion,
            };

            return View(usuarioModel);
        }

        // GET: UsuariosController/Create
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var model = new UsuarioModel
            {
                roles = await _usuarioManager.ObtenerRoleUsuarioAsync(cancellationToken)
            };

            try
            {
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                
                model.roles = await _usuarioManager.ObtenerRoleUsuarioAsync(cancellationToken);
                return View(model);
            }
        }

        // POST: UsuariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioModel model, CancellationToken cancellationToken)
        {
            try
            {
               
                if (!ModelState.IsValid)
                {
                    
                    model.roles = await _usuarioManager.ObtenerRoleUsuarioAsync(cancellationToken);
                    return View(model);
                }

                if(string.IsNullOrWhiteSpace(model.NuevaContrasenia))
                {

                    ModelState.AddModelError("NuevaContrasenia", "La nueva contraseña es obligatoria.");

                    model.roles = await _usuarioManager.ObtenerRoleUsuarioAsync(cancellationToken);
                    return View(model);
                }

                Usuario usuario = new Usuario
                {
                    Nombre = model.nombre,
                    Apellido = model.apellido,
                    Dni = model.dni?.ToString(),
                    Email = model.email,
                    NombreUsuario = model.nombre_usuario,
                    Telefono = model.telefono,
                    Contrasenia = HashPassword(model.NuevaContrasenia), 
                    RolId = model.rolId,
                };

               
                var resultado = await _usuarioManager.DatosExistentesAsync(usuario, cancellationToken);

                if (resultado.Existe)
                {
                   
                    ModelState.AddModelError(resultado.Campo, $"Ya existe un usuario con ese {resultado.Campo}.");
                    
                    model.roles = await _usuarioManager.ObtenerRoleUsuarioAsync(cancellationToken);
                    return View(model);
                }

                await _usuarioManager.Insertar(usuario, cancellationToken);
                TempData["SuccessMessage"] = "Usuario creado exitosamente.";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                
                model.roles = await _usuarioManager.ObtenerRoleUsuarioAsync(cancellationToken);
                return View(model);
            }
        }

        // GET: UsuariosController/Edit/5
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioManager.ObtenerPorId(id, cancellationToken);

            if (usuario == null)
            {
                return NotFound();
            }

            
            long.TryParse(usuario.Dni, out long dni);

            UsuarioModel usuarioModel = new UsuarioModel
            {
                idUsuario = usuario.IdPersona,
                nombre = usuario.Nombre,
                apellido = usuario.Apellido,
                
                dni = dni == 0 ? null : usuario.Dni, 
                nombre_usuario = usuario.NombreUsuario,
                email = usuario.Email,
                telefono = usuario.Telefono,
                rolId = usuario.RolId,
                rol = usuario.Rol?.Descripcion, 
                roles = await _usuarioManager.ObtenerRoleUsuarioAsync(cancellationToken) 
            };

            return View(usuarioModel);
        }

        // POST: UsuariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioModel model, CancellationToken cancellationToken)
        {
            try
            {
               
                if (!ModelState.IsValid)
                {
                   
                    model.roles = await _usuarioManager.ObtenerRoleUsuarioAsync(cancellationToken);
                    return View(model);
                }

                var usuario = await _usuarioManager.ObtenerPorId(id, cancellationToken);

                if (usuario == null)
                    return NotFound();

               
                usuario.Nombre = model.nombre;
                usuario.Apellido = model.apellido;
                usuario.Dni = model.dni?.ToString();
                usuario.Email = string.IsNullOrWhiteSpace(model.email) ? usuario.Email : model.email;
                usuario.NombreUsuario = string.IsNullOrWhiteSpace(model.nombre_usuario) ? usuario.NombreUsuario : model.nombre_usuario;
                usuario.Telefono = model.telefono;
                usuario.RolId = model.rolId;


               
                var resultado = await _usuarioManager.DatosExistentesAsync(usuario, cancellationToken);
               
                
                if (resultado.Existe && resultado.IdExistente != id) 
                {
                    ModelState.AddModelError(resultado.Campo, $"Ya existe un usuario con ese {resultado.Campo}.");
                    
                    model.roles = await _usuarioManager.ObtenerRoleUsuarioAsync(cancellationToken);
                    return View(model);
                }


               
                if (!string.IsNullOrWhiteSpace(model.NuevaContrasenia))
                {
                   
                    if (model.NuevaContrasenia != model.ConfirmarContrasenia)
                    {
                        ModelState.AddModelError("ConfirmarContrasenia", "La nueva contraseña y la confirmación no coinciden.");
                        model.roles = await _usuarioManager.ObtenerRoleUsuarioAsync(cancellationToken);
                        return View(model);
                    }
                    
                    usuario.Contrasenia = HashPassword(model.NuevaContrasenia); 
                    await _usuarioManager.Editar(usuario, cancellationToken);
                }
                else
                {
                   
                    await _usuarioManager.EditarSinContrasenia(usuario, cancellationToken); 
                }

                // Si el usuario editado es el mismo que está logueado, actualizar las claims
                //if (User.Identity.GetUserId() == id.ToString())
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == id.ToString())
                {
                    
                    await HttpContext.SignOutAsync();

                   
                    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(
                            new ClaimsIdentity(
                                new[] 
                                {
                                    new Claim(ClaimTypes.NameIdentifier, usuario.IdPersona.ToString()),
                                    new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                                    new Claim(ClaimTypes.Email, usuario.Email),
                                    new Claim(ClaimTypes.Role, usuario.Rol?.Descripcion ?? "Usuario"), 
                                }, CookieAuthenticationDefaults.AuthenticationScheme)
                            )
                        );
                }

                TempData["SuccessMessage"] = "Usuario editado correctamente.";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

               
                model.roles = await _usuarioManager.ObtenerRoleUsuarioAsync(cancellationToken);
                return View(model);
            }
        }

        // GET: UsuariosController/Delete/5
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            
            var usuario = await _usuarioManager.ObtenerPorId(id, cancellationToken);
            if (usuario == null)
            {
                return NotFound();
            }
            
            return View(usuario);
        }

        // POST: UsuariosController/Delete/5
        [HttpPost, ActionName("Delete")] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken) 
        {
            try
            {
                //if (User.Identity.GetUserId() == id.ToString())
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == id.ToString())
                {
                    TempData["Error"] = "No podés eliminar tu propio usuario.";
                    return RedirectToAction(nameof(Index));
                }

                await _usuarioManager.Borrar(id, cancellationToken);
                TempData["SuccessMessage"] = "Usuario eliminado correctamente."; 
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = $"Error al eliminar el usuario: {ex.Message}"; 
                return RedirectToAction(nameof(Index));
            }
        }


        public async Task<IActionResult> Buscar(string filtro, CancellationToken cancellationToken)
        {
            var usuarios = await _usuarioManager
                .Buscar(filtro, cancellationToken);


            return Json(usuarios);
        }


       
        private string HashPassword(string password)
        {
          
            return password; 
        }

        // GET: ClienteController
        public async Task<IActionResult> VerInactivos(CancellationToken cancellationToken)
        {

            var usuarios = await _usuarioManager.ObtenerInactivosAsync(cancellationToken);

            return View(usuarios);
        }
    }
}