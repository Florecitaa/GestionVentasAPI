using Microsoft.AspNetCore.Mvc;
using GestionVentasAPI.Services;
using GestionVentasAPI.Models;
using System.Data.SqlClient;

namespace GestionVentasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly VentaService _ventaService;

        public VentaController(VentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
        {
            var ventas = await _ventaService.ObtenerTodasLasVentasAsync();
            return Ok(ventas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
        {
            var venta = await _ventaService.ObtenerVentaPorIdAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            return Ok(venta);
        }

        [HttpPost]
        public async Task<ActionResult> PostVenta(Venta venta)
        {
            int newVentaId = await _ventaService.InsertarVentaAsync(venta);
            venta.IDVenta = newVentaId;
            return CreatedAtAction(nameof(GetVenta), new { id = newVentaId }, venta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenta(int id, Venta venta)
        {
            if (id != venta.IDVenta)
            {
                return BadRequest();
            }

            var result = await _ventaService.ActualizarVentaAsync(id, venta);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenta(int id)
        {
            var result = await _ventaService.EliminarVentaAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
