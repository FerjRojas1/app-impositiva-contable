using Microsoft.EntityFrameworkCore;
using ServiciosEC.Interfaces;
using ServiciosEC.Middleware;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Models
{
    public partial class ECContext
    {
        //para configs personalizadas que no se reescriban al hacer scafold

        private IUserContextService _userContextService;

        public bool Auditar { get; set; } = true;

        [NotMapped]
        public List<AuditoriaTemporal> AuditoriasPendientes { get; set; } = new();

        public HashSet<object> EntidadesModOBorradasParaAuditar { get; } = new();

        public HashSet<object> EntidadesAddedParaAuditar { get; } = new();

        public enum EstadosEnum
        {
            Activo = 1,
            Inactivo = 2,
        }
        public enum RolesEnum
        {
            Usuario =1,
            Admin=2,
            Cliente=3
        }
       


        public ECContext(DbContextOptions<ECContext> options, IUserContextService userContextService)
            : this(options)
        {
            _userContextService = userContextService;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            
            var IdUsuario = _userContextService.IdUsuarioContext;
            Debug.WriteLine("Usuario actual: " + IdUsuario);

            foreach (var item in ChangeTracker.Entries<ISoftDeletable>())
            {
                if (item.State == EntityState.Deleted)
                {
                    //aca se puede gestionar el soft delete automaticamente
                    
                    item.State = EntityState.Modified;
                    item.Entity.EstadoId = (int)EstadosEnum.Inactivo;
                }


            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            //filtros para soft delete, solo muestra los activos

            modelBuilder.Entity<Auditoria>()
                .HasOne(a => a.Persona)
                .WithMany(u => u.Auditoria)
                .HasForeignKey(a => a.IdPersona)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.ToTable("Personas");
                //entity.HasQueryFilter(e => e.EstadoId == (int)EstadosEnum.Activo);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios").HasBaseType<Persona>();

                // Propiedades adicionales de Usuario
                entity.Property(e => e.NombreUsuario)
                    .HasColumnName("nombre_usuario")
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Contrasenia)
                    .HasColumnName("contrasenia")
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);


            });

            modelBuilder.Entity<Cliente>(entity => {
                entity.ToTable("Clientes").HasBaseType<Persona>();

                // Propiedades adicionales de Cliente
                entity.Property(e => e.Cuit)
                    .HasColumnName("cuit")
                    .IsRequired()
                    .HasMaxLength(50) 
                    .IsUnicode(false);

                entity.HasIndex(e => e.Cuit, "UQ_Clientes_CUIT").IsUnique();

                entity.Property(e => e.DomFiscal)
                    .HasColumnName("dom_fiscal")
                    .HasMaxLength(100) 
                    .IsUnicode(false);

                entity.Property(e => e.RazonSocial)
                    .HasColumnName("razon_social")
                    .HasMaxLength(150)
                    .IsUnicode(true); 

                
            });

            modelBuilder.Entity<Venta>()
                .HasQueryFilter(e => e.EstadoId == (int)EstadosEnum.Activo);

            modelBuilder.Entity<Compra>()
                .HasQueryFilter(e => e.EstadoId == (int)EstadosEnum.Activo);

           
        }
    }
}
