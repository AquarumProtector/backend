using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Controllers.DTOs;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaterSourcesController : ControllerBase
    {
        private readonly AquaContext _context;

        public WaterSourcesController(AquaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todas as fontes de água.
        /// </summary>
        /// <returns>Lista de WaterSource.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<WaterSource>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WaterSource>>> GetWaterSources()
        {
            var sources = await _context.WaterSources.ToListAsync();
            return Ok(sources);
        }

        /// <summary>
        /// Retorna uma fonte de água pelo ID.
        /// </summary>
        /// <param name="id">ID da fonte de água.</param>
        /// <returns>WaterSource correspondente.</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(WaterSource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WaterSource>> GetWaterSource(int id)
        {
            var waterSource = await _context.WaterSources.FindAsync(id);
            if (waterSource == null)
            {
                return NotFound();
            }
            return Ok(waterSource);
        }

        /// <summary>
        /// Cria uma nova fonte de água.
        /// </summary>
        /// <param name="dto">Dados para criação da fonte de água.</param>
        /// <returns>O objeto criado.</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(WaterSource), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WaterSource>> PostWaterSource(WaterSourceCreateDto dto)
        {
            // Mapear DTO para entidade
            var waterSource = new WaterSource
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Localizacao = dto.Localizacao,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Type = dto.Type,
                CreatedById = dto.CreatedById,
                LastInspected = dto.LastInspected,
                Status = dto.Status,
                // IsActive, CreatedAt e UpdatedAt já vêm com valores padrão
            };

            _context.WaterSources.Add(waterSource);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWaterSource), new { id = waterSource.Id }, waterSource);
        }

        /// <summary>
        /// Atualiza uma fonte de água existente e grava um registro de atualização.
        /// </summary>
        /// <param name="id">ID da fonte de água a ser atualizada.</param>
        /// <param name="dto">Dados para atualização da fonte de água.</param>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutWaterSource(int id, WaterSourceUpdateDto dto)
        {
            var existing = await _context.WaterSources
                                         .Include(ws => ws.WaterSourceUpdates)
                                         .FirstOrDefaultAsync(ws => ws.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            // Verifica inconsistência de ID (não deveria ocorrer porque vem da URL)
            // Mas mantemos para seguir padrão
            // Nota: aqui o DTO não contém Id, pois o endpoint já define qual ID atualizar
            // Caso o usuário inclua um campo Id no JSON, ele será ignorado.

            // Armazenar valores antigos para registro de update
            var oldStatus = existing.Status;
            var oldLatitude = existing.Latitude;
            var oldLongitude = existing.Longitude;

            // Atualizar campos da entidade
            existing.Nome = dto.Nome;
            existing.Descricao = dto.Descricao;
            existing.Localizacao = dto.Localizacao;
            existing.Latitude = dto.Latitude;
            existing.Longitude = dto.Longitude;
            existing.Type = dto.Type;
            existing.LastInspected = dto.LastInspected;
            existing.Status = dto.Status;
            existing.IsActive = dto.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            // Criar novo registro de WaterSourceUpdate
            var updateRecord = new WaterSourceUpdate
            {
                WaterSourceId = existing.Id,
                Descricao = dto.UpdateDescricao,
                OldStatus = oldStatus,
                Status = dto.Status,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
                // UpdateDate será DateTime.UtcNow por padrão
            };
            existing.WaterSourceUpdates.Add(updateRecord);
            _context.WaterSourceUpdates.Add(updateRecord);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WaterSourceExists(id))
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
        /// Exclui uma fonte de água pelo ID.
        /// </summary>
        /// <param name="id">ID da fonte de água a ser excluída.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteWaterSource(int id)
        {
            var waterSource = await _context.WaterSources.FindAsync(id);
            if (waterSource == null)
            {
                return NotFound();
            }

            _context.WaterSources.Remove(waterSource);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WaterSourceExists(int id)
        {
            return _context.WaterSources.Any(e => e.Id == id);
        }
    }
}
