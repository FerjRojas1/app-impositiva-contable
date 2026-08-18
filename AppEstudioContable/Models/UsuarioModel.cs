using ServiciosEC.Models; 
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions; 

namespace AppEstudioContable.Models
{
    public class UsuarioModel
    {
        // Persona
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        public string? nombre { get; set; }

        //[Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder los 50 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios.")]
        public string? apellido { get; set; }


        //[Required(ErrorMessage = "El DNI es obligatorio.")]
        [StringLength(8, MinimumLength = 7, ErrorMessage = "El DNI debe tener entre 7 y 8 dígitos.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El DNI solo puede contener números.")]
        public string? dni { get; set; }

        // Usuario
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder los 50 caracteres.")]
        public string nombre_usuario { get; set; } = null!; 

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres.")]
        public string email { get; set; } = null!; 

        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
        public string? telefono { get; set; }

        
        //[Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&\-_])[A-Za-z\d@$!%*?&\-_]{8,}$",
            ErrorMessage = "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial (ej. !@#$%^&*).")]
        public string? NuevaContrasenia { get; set; } 


        //[Required(ErrorMessage = "Debe confirmar la contraseña.")]
        [DataType(DataType.Password)]
        [Compare("NuevaContrasenia", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmarContrasenia { get; set; } 

        
        public string? rol { get; set; } = null!;

        [Required(ErrorMessage = "Debe seleccionar un rol.")]
        [Range(1, 3, ErrorMessage = "Debe seleccionar un rol válido.")]
        public int rolId { get; set; }

        public List<Role>? roles { get; set; } 
    }
}