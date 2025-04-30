using GestionVentasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using GestionVentasAPI.Models;
using System.Data.SqlClient;
namespace GestionVentasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.ObtenerUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult> PostUsuario(Usuario usuario)
        {
            await _usuarioService.CrearUsuarioAsync(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IDUsuario }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.IDUsuario)
            {
                return BadRequest();
            }

            var result = await _usuarioService.ActualizarUsuarioAsync(id, usuario);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var result = await _usuarioService.EliminarUsuarioAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login([FromQuery] string correo, [FromQuery] string clave)
        {
            var usuario = await _usuarioService.ValidarUsuarioAsync(correo, clave);
            if (usuario == null)
            {
                return Unauthorized();
            }
            return Ok(usuario);
        }

        [HttpGet("validar")]
        public async Task<IActionResult> ValidarUsuario([FromQuery] string correo, [FromQuery] string clave)
        {
            var usuario = await _usuarioService.ValidarUsuarioAsync(correo, clave);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }


    }



}
