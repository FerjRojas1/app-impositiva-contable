using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServiciosEC.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions; 

namespace AppEstudioContable.Models
{
    public class ClienteModel
    {
        [Key]
        public int id { get; set; }

        
        [StringLength(100, ErrorMessage = "El Nombre no puede exceder los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El Nombre solo puede contener letras y espacios.")]
        public string? Nombre { get; set; }

        
        [StringLength(100, ErrorMessage = "El Apellido no puede exceder los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El Apellido solo puede contener letras y espacios.")]
        public string? Apellido { get; set; }

        
        [StringLength(10, MinimumLength = 7, ErrorMessage = "El DNI debe tener entre 7 y 10 dígitos.")]
        [RegularExpression(@"^\d{7,10}$", ErrorMessage = "El DNI debe contener solo números.")]
        public string? Dni { get; set; }

        [Required(ErrorMessage = "El CUIT es obligatorio.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "El CUIT debe tener exactamente 11 dígitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "El CUIT debe contener solo números y tener 11 dígitos.")]
        public string Cuit { get; set; }

        [Display(Name = "Razón Social")]
        [StringLength(255, ErrorMessage = "La Razón Social no puede exceder los 255 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9\s.,&'-]+$", ErrorMessage = "La Razón Social contiene caracteres inválidos.")]
        public string? RazonSocial { get; set; }

        [Display(Name = "Domicilio Fiscal")]
        [Required(ErrorMessage = "El Domicilio Fiscal es obligatorio.")]
        [StringLength(255, ErrorMessage = "El Domicilio Fiscal no puede exceder los 255 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9\s.,#/\-]+$", ErrorMessage = "El Domicilio Fiscal contiene caracteres inválidos.")]
        public string? DomFiscal { get; set; }

        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
        [StringLength(255, ErrorMessage = "El Email no puede exceder los 255 caracteres.")]
        public string? Email { get; set; }

        public DateTime? Fecha { get; set; }

        public int EstadoId { get; set; }

        [BindNever]
        public string? Estado { get; set; }
        public List<Estado>? Estados { get; set; }
    }
}