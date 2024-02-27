using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using L01_2021_BS_650.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2021_BS_650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class comentariosController : Controller
    {

        private readonly blogDBContext _blogDbContexto;

        public comentariosController(blogDBContext blogDbContexto)
        {
            _blogDbContexto = blogDbContexto;
        }
        // READ
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<comentarios> listadoComentarios = _blogDbContexto.comentarios.ToList();

            if (listadoComentarios.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoComentarios);
        }

        // CREATE
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarComentario([FromBody] comentarios comentario)
        {
            try
            {
                _blogDbContexto.comentarios.Add(comentario);
                _blogDbContexto.SaveChanges();
                return Ok(comentario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // UPDATE
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarComentario(int id, [FromBody] comentarios comentarioModificar)
        {
            comentarios comentarioActual = _blogDbContexto.comentarios.FirstOrDefault(c => c.cometarioId == id);

            if (comentarioActual == null)
            {
                return NotFound();
            }

            comentarioActual.publicacionId = comentarioModificar.publicacionId;
            comentarioActual.comentario = comentarioModificar.comentario;
            comentarioActual.usuarioId = comentarioModificar.usuarioId;

            _blogDbContexto.Entry(comentarioActual).State = EntityState.Modified;

            _blogDbContexto.SaveChanges();

            return Ok(comentarioModificar);
        }

        // DELETE
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarComentario(int id)
        {
            comentarios comentario = _blogDbContexto.comentarios.FirstOrDefault(c => c.cometarioId == id);

            if (comentario == null)
            {
                return NotFound();
            }

            _blogDbContexto.comentarios.Remove(comentario);
            _blogDbContexto.SaveChanges();

            return Ok(comentario);
        }

        //Filtrar por usuario
        [HttpGet]
        [Route("GetByUsuario")]
        public IActionResult GetByUsuario(int usuarioId)
        {
            List<comentarios> comentariosFiltrados = _blogDbContexto.comentarios.Where(e => e.usuarioId == usuarioId)
                .ToList();

            if (comentariosFiltrados.Count == 0)
            {
                return NotFound();
            }

            return Ok(comentariosFiltrados);
        }
    }
}
