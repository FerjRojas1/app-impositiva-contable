using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiciosEC.Interfaces;
using ServiciosEC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiciosEC.Utilidades.Atributos;
using ServiciosEC.Utilidades;
using System.Text.Json;
using System.Reflection;

namespace ServiciosEC.Middleware
{
    public class AuditoriaInterceptor : SaveChangesInterceptor
    {
        private readonly IUserContextService _userContext;
        private readonly IAuditoria _auditoriaService;
        private readonly ILogger<AuditoriaInterceptor> _logger;

        public AuditoriaInterceptor(
            IUserContextService userContext,
            IAuditoria auditoriaService,
            ILogger<AuditoriaInterceptor> logger)
        {
            _userContext = userContext;
            _auditoriaService = auditoriaService;
            _logger = logger;
        }

        //para verificar si la clase tiene atributo ClaseNoAuditable
        private static bool TieneAtributoClaseNoAuditable(Type type)
        {
            while (type != null && type != typeof(object))
            {
                if (Attribute.IsDefined(type, typeof(ClaseNoAuditableAttribute)))
                    return true;

                type = type.BaseType;
            }
            return false;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
                Debug.WriteLine("[Interceptor]: Ejecutando Auditoria - saving");

            var IdPersona = _userContext.IdUsuarioContext;

            var context = eventData.Context as ECContext;

            if (context == null || context.Auditar==false)
            {
                
                return await base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            // Marcando entradas added para auditar
            var addedEntries = context.ChangeTracker.Entries()
                 .Where(e => e.State == EntityState.Added)
                 .ToList();

            foreach (var entry in addedEntries)
            {
                var entityType = entry.Entity.GetType();

                //descartar por clase no auditable
                if (TieneAtributoClaseNoAuditable(entityType))
                {
                    continue;
                }

                // entidad marcada para auditaria posterior en SavedChanges
                context.EntidadesAddedParaAuditar.Add(entry.Entity);
            }


            //Entradas modificadas o borradas
            var entries = context.ChangeTracker.Entries()
                .Where(e =>
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in entries)
            {
                try
                {
                    var entityType = entry.Entity.GetType();

                    //descartar clase no auditable
                    if (TieneAtributoClaseNoAuditable(entityType))
                    {
                        continue;
                    }
                    
                    var dictDatosAntes = new Dictionary<string, object?>();
                    var dictDatosDespues = new Dictionary<string, object?>();

                    foreach (var prop in entry.CurrentValues.Properties)
                    {

                        if (prop.IsShadowProperty()) continue;

                        var propiedadInfo = entityType.GetProperty(prop.Name);

                        if (propiedadInfo != null && !Attribute.IsDefined(propiedadInfo, typeof(PropNoAuditableAttribute)))
                        {
                            dictDatosDespues[prop.Name] = entry.CurrentValues[prop];
                            dictDatosAntes[prop.Name] = entry.OriginalValues[prop];
                        }

                    }

                    //si no hay propiedades para auditar
                    if (dictDatosAntes.Count == 0 && dictDatosDespues.Count == 0)
                    {
                        continue;

                    }


                    //accion
                    var accion = entry.State switch
                    {
                        EntityState.Modified => "Modificación",
                        EntityState.Deleted => "Eliminación",
                        _ => "Desconocida"
                    };

                    
                    //accion para soft delete
                    var entidad = entry.Context.Model.FindEntityType(entry.Entity.GetType());

                    var propiedadEstado = entidad?.FindProperty("EstadoId");

                    if (propiedadEstado != null && entry.Entity is ISoftDeletable)
                    {
                        var valorEstado = entry.CurrentValues[propiedadEstado.Name];

                        if (valorEstado != null && (int)valorEstado == 2)
                        {
                            accion = "Eliminación";
                        }
                    }


                    var tabla = entry.Metadata.GetTableName() ?? entityType.Name;

                    context.AuditoriasPendientes.Add(new AuditoriaTemporal
                    {
                        Tabla = tabla,
                        Accion = accion,
                        IdPersona = IdPersona,
                        DatosAntes = dictDatosAntes != null ? JsonConvert.SerializeObject(dictDatosAntes, Formatting.Indented) : null,
                        DatosDespues = dictDatosDespues != null ? JsonConvert.SerializeObject(dictDatosDespues, Formatting.Indented) : null,
                        
                    });

                    //entidad marcaada para auditar despues
                    context.EntidadesModOBorradasParaAuditar.Add(entry.Entity);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error preparando auditoría para entidad {Entidad}", entry.Entity.GetType().Name);
                }
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        
        
        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {

            Debug.WriteLine("[Interceptor]: Ejecutando Auditoria - saved");

            var IdPersona = _userContext.IdUsuarioContext;

            var context = eventData.Context as ECContext;

            if (context == null || context.Auditar == false)
            {
                return await base.SavedChangesAsync(eventData, result, cancellationToken);
            }

            var addedEntries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Unchanged && e.IsKeySet)
                .ToList();

            //Procesa las entries que tienen estado Unchanged y ademas tienen un id asociado
            foreach (var entry in addedEntries)
            {
                var entityType = entry.Entity.GetType();

                //descartar las modificadas/eliminadas que vienen del changetracker con estado unchanged
                if (context.EntidadesModOBorradasParaAuditar.Contains(entry.Entity))
                {
                    continue;
                }

                //si las entidades son Added
                if (context.EntidadesAddedParaAuditar.Contains(entry.Entity))
                {
                    try
                    {
                        var tabla = entry.Metadata.GetTableName() ?? entityType.Name;

                        var accion = "Creación";

                        object? datosAntes = null;

                        var dictDatosDespues = new Dictionary<string, object?>();

                        foreach (var prop in entry.CurrentValues.Properties)
                        {

                            if (prop.IsShadowProperty()) continue;

                            var propiedadInfo = entityType.GetProperty(prop.Name);

                            if (propiedadInfo != null && !Attribute.IsDefined(propiedadInfo, typeof(PropNoAuditableAttribute)))
                            {
                                dictDatosDespues[prop.Name] = entry.CurrentValues[prop];
                            }

                        }

                        //si no hay propiedades para auditar
                        if (dictDatosDespues.Count == 0)
                        {
                            continue;

                        }


                        string datosDespues = System.Text.Json.JsonSerializer.Serialize(dictDatosDespues, new JsonSerializerOptions
                        {
                            WriteIndented = true,
                        });

                        context.AuditoriasPendientes.Add(new AuditoriaTemporal
                        {
                            Tabla = tabla,
                            Accion = accion,
                            IdPersona = IdPersona,
                            DatosAntes = datosAntes != null ? JsonConvert.SerializeObject(datosAntes) : null,
                            DatosDespues = datosDespues

                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error preparando auditoría para entidad {Entidad}", entry.Entity.GetType().Name);
                    }
                }
                
            }

            //registro de auditorias pendientes
            if (context.Auditar && context.AuditoriasPendientes.Count != 0)
            {
                var auditorias = context.AuditoriasPendientes.ToList();
                context.AuditoriasPendientes.Clear();

                foreach (var item in auditorias)
                {
                    try
                    {
                        await _auditoriaService.RegistrarAuditoriaAsync(
                            item.Tabla,
                            item.Accion,
                            item.IdPersona,
                            item.DatosAntes,
                            item.DatosDespues);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error guardando auditoría diferida para tabla {Tabla}", item.Tabla);
                    }
                }
            }

            //limpiar los registros
            context.EntidadesModOBorradasParaAuditar.Clear();
            context.EntidadesAddedParaAuditar.Clear();
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }


    }


}
