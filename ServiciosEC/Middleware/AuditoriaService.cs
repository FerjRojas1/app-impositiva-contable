using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiciosEC.Interfaces;
using ServiciosEC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Middleware
{
    public class AuditoriaService : IAuditoria
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AuditoriaService> _logger;
        public AuditoriaService(IServiceScopeFactory scopeFactory, ILogger<AuditoriaService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        
       
        public async Task RegistrarAuditoriaAsync(string tabla, string accion, int IdPersona, string? datosAntes, string? datosDespues)
        {
            
            using (var scope = _scopeFactory.CreateScope())
            {
                //nuevo contexto solo para auditoria    
                var context = scope.ServiceProvider.GetRequiredService<ECContext>();
                Debug.WriteLine("[AuditoriaService]: Registrar Auditoria");
                context.Auditar = false;

                var auditoria = new Auditoria
                    
                {
                    Tabla = tabla,
                    Accion = accion,
                    IdPersona = IdPersona,
                    Fecha = DateTime.Now,
                    DatosAntes = datosAntes,
                    DatosDespues = datosDespues
                };

                context.Auditorias.Add(auditoria);
                await context.SaveChangesAsync();
            }

        }


    }
}
