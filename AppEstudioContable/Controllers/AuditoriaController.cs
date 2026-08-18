using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiciosEC.Interfaces.Managers;
using ServiciosEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppEstudioContable.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuditoriaController : Controller
    {
        private readonly ECContext _context;
        private readonly IUsuariosManager _usuarioManger;

        public AuditoriaController(ECContext context, IUsuariosManager usuarioManger)
        {
            _usuarioManger = usuarioManger;
            _context = context;
        }

        // GET: Auditoria
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var actividad = await _usuarioManger.MostrarActividad(cancellationToken);
            return View(actividad);
        }       
    }
}
