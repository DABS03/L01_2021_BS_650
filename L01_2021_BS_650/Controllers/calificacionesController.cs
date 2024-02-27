using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using L01_2021_BS_650.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2021_BS_650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class calificacionesController : ControllerBase
    {
        private readonly blogDBContext _blogDbContexto;

        public calificacionesController(blogDBContext blogDbContexto)
        {
            _blogDbContexto = blogDbContexto;
        }

        // CREATE
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarCalificacion([FromBody] calificaciones calificacion)
        {
            try
            {
                _blogDbContexto.calificaciones.Add(calificacion);
                _blogDbContexto.SaveChanges();
                return Ok(calificacion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //READ
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<calificaciones> listadoCalificaciones = (from e in _blogDbContexto.calificaciones
                                                          select e).ToList();

            if(listadoCalificaciones.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoCalificaciones);
        }

        // UPDATE
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarCalificacion(int id, [FromBody] calificaciones calificacionModificar)
        {
            calificaciones calificacionActual = _blogDbContexto.calificaciones.FirstOrDefault(c => c.calificacionId == id);

            if (calificacionActual == null)
            {
                return NotFound();
            }

            calificacionActual.publicacionId = calificacionModificar.publicacionId;
            calificacionActual.usuarioId = calificacionModificar.usuarioId;
            calificacionActual.calificacion = calificacionModificar.calificacion;

            _blogDbContexto.Entry(calificacionActual).State = EntityState.Modified;

            _blogDbContexto.SaveChanges();

            return Ok(calificacionModificar);
        }

        // DELETE
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarCalificacion(int id)
        {
            calificaciones calificacion = _blogDbContexto.calificaciones.FirstOrDefault(e => e.calificacionId == id);

            if (calificacion == null)
            {
                return NotFound();
            }

            _blogDbContexto.calificaciones.Remove(calificacion);
            _blogDbContexto.SaveChanges();

            return Ok(calificacion);
        }

        //Filtrar por publicacion
        [HttpGet]
        [Route("GetPorPublicacion")]
        public IActionResult GetByPublicacion(int publicacionId)
        {
            List<calificaciones> calificacionesFiltradas = _blogDbContexto.calificaciones
                .Where(e => e.publicacionId == publicacionId)
                .ToList();

            if (calificacionesFiltradas.Count == 0)
            {
                return NotFound();
            }

            return Ok(calificacionesFiltradas);
        }
    }
}
