
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiciosEC.Models
{
    public class EventoAgenda
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Titulo { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; } 

        public bool TodoElDia { get; set; } = false; 

        [StringLength(50)] 
        public string Color { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }


    }
}