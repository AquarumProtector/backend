using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        private readonly AquaContext _context;

        public AlertsController(AquaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os alertas.
        /// </summary>
        /// <returns>Lista de Alertas.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Alert>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Alert>>> GetAlerts()
        {
            var alerts = await _context.Alerts.ToListAsync();
            return Ok(alerts);
        }

        /// <summary>
        /// Retorna um alerta especifico por ID.
        /// </summary>
        /// <param name="id">o id especifico do alerta.</param>
        /// <returns>O alerta com o ID fornecido.</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Alert), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Alert>> GetAlert(int id)
        {
            var alert = await _context.Alerts.FindAsync(id);

            if (alert == null)
            {
                return NotFound();
            }

            return Ok(alert);
        }

        /// <summary>
        /// Atualiza um alerta existente.
        /// </summary>
        /// <param name="id">o id do alerta existente.</param>
        /// <param name="alert">O objeto alerta atualizado.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAlert(int id, Alert alert)
        {
            if (id != alert.Id)
            {
                return BadRequest();
            }

            _context.Entry(alert).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlertExists(id))
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

        /// <summary>
        /// Cria um novo alerta.
        /// </summary>
        /// <param name="alert">O Objeto Alerta a ser criado.</param>
        /// <returns>O Alerta Criado.</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Alert), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Alert>> PostAlert(Alert alert)
        {
            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlert), new { id = alert.Id }, alert);
        }

        /// <summary>
        /// Deleta um alerta especifico por ID
        /// </summary>
        /// <param name="id">O id do alerta.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAlert(int id)
        {
            var alert = await _context.Alerts.FindAsync(id);
            if (alert == null)
            {
                return NotFound();
            }

            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlertExists(int id)
        {
            return _context.Alerts.Any(e => e.Id == id);
        }
    }
}
