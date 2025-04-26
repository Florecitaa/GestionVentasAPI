using GestionVentasAPI.Models;
using GestionVentasAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionVentasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleVentaController : ControllerBase
    {
        private readonly DetalleVentaService _detalleVentaService;

        public DetalleVentaController(DetalleVentaService detalleVentaService)
        {
            _detalleVentaService = detalleVentaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleVenta>>> GetDetallesVenta()
        {
            var detallesVenta = await _detalleVentaService.ObtenerTodosLosDetallesDeVentaAsync();
            return Ok(detallesVenta);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleVenta>> GetDetalleVenta(int id)
        {
            var detalleVenta = await _detalleVentaService.ObtenerDetalleVentaPorIdAsync(id);
            if (detalleVenta == null)
            {
                return NotFound();
            }
            return Ok(detalleVenta);
        }

        [HttpPost]
        public async Task<ActionResult> PostDetalleVenta(DetalleVenta detalleVenta)
        {
            await _detalleVentaService.CrearDetalleVentaAsync(detalleVenta);
            return CreatedAtAction(nameof(GetDetalleVenta), new { id = detalleVenta.IdDetalledeventa }, detalleVenta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleVenta(int id, DetalleVenta detalleVenta)
        {
            if (id != detalleVenta.IdDetalledeventa)
            {
                return BadRequest();
            }

            var result = await _detalleVentaService.ActualizarDetalleVentaAsync(id, detalleVenta);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleVenta(int id)
        {
            var result = await _detalleVentaService.EliminarDetalleVentaAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
