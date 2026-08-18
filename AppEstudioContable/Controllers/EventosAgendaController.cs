using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ServiciosEC.Models; 

namespace AppEstudioContable.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class EventosAgendaController : ControllerBase
    {
        private readonly ECContext _context; 

        public EventosAgendaController(ECContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetEventos(DateTime start, DateTime end)
        {
           
            var eventos = await _context.EventosAgenda
                .Where(e => e.FechaInicio <= end && (e.FechaFin == null || e.FechaFin >= start))
                .Select(e => new
                {
                    id = e.Id, 
                    title = e.Titulo,
                    start = e.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss"), 
                    end = e.FechaFin.HasValue ? e.FechaFin.Value.ToString("yyyy-MM-ddTHH:mm:ss") : null,
                    allDay = e.TodoElDia,
                    color = e.Color, 
                    description = e.Descripcion 
                })
                .ToListAsync();

            return Ok(eventos);
        }

        // POST: api/EventosAgenda
        // Para crear un nuevo evento
        [HttpPost]
        public async Task<IActionResult> PostEvento([FromBody] EventoAgenda evento)
        {
        
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            
            if (string.IsNullOrEmpty(evento.Color))
            {
                evento.Color = "#007bff"; 
            }

            
            if (!evento.FechaFin.HasValue && !evento.TodoElDia)
            {
                evento.FechaFin = evento.FechaInicio.AddHours(1);
            }

            _context.EventosAgenda.Add(evento); 
            await _context.SaveChangesAsync(); 

           
            return CreatedAtAction(nameof(GetEventos), new { id = evento.Id }, evento);
        }

        // PUT: api/EventosAgenda/{id}
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvento(int id, [FromBody] EventoAgenda evento)
        {
            if (id != evento.Id)
            {
                return BadRequest("ID del evento no coincide.");
            }

           
            _context.Entry(evento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
                {
                    return NotFound(); 
                }
                else
                {
                    throw; 
                }
            }

            return NoContent(); 
        }

        // DELETE: api/EventosAgenda/{id}
        // Para eliminar un evento
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.EventosAgenda.FindAsync(id);
            if (evento == null)
            {
                return NotFound(); 
            }

            _context.EventosAgenda.Remove(evento); 
            await _context.SaveChangesAsync(); 

            return NoContent(); 
        }

        private bool EventoExists(int id)
        {
            return _context.EventosAgenda.Any(e => e.Id == id);
        }
    }
}