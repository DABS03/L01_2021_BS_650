using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using L01_2021_BS_650.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2021_BS_650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly blogDBContext _blogDbContexto;

        public usuariosController(blogDBContext blogDbContexto)
        {
            _blogDbContexto = blogDbContexto;
        }

        //CREATE
        [HttpGet]
        [Route("Add")]
        public IActionResult GuardarUsuario([FromBody] usuarios usuario)
        {
            try
            {
                _blogDbContexto.usuarios.Add(usuario);
                _blogDbContexto.SaveChanges();
                return Ok(usuario);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        //READ
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<usuarios> listadoUsuarios = (from e in _blogDbContexto.usuarios
                                              select e).ToList();

            if(listadoUsuarios.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoUsuarios);
        }

        //UPDATE
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarUsuario(int id, [FromBody] usuarios usuarioModificar)
        {
            usuarios? usuarioActual = (from e in _blogDbContexto.usuarios
                                       where e.usuarioId == id
                                       select e).FirstOrDefault();

            if(usuarioActual == null)
            { return NotFound(); }

            usuarioActual.rolId = usuarioModificar.rolId;
            usuarioActual.nombreUsuario = usuarioModificar.nombreUsuario;
            usuarioActual.clave = usuarioModificar.clave;
            usuarioActual.nombre = usuarioModificar.nombre;
            usuarioActual.apellido = usuarioModificar.apellido;

            _blogDbContexto.Entry(usuarioActual).State = EntityState.Modified;

            return Ok(usuarioModificar);
        }

        //DELETE
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            usuarios? usuario = (from e in _blogDbContexto.usuarios
                                 where e.usuarioId == id
                                 select e).FirstOrDefault();
            if(usuario == null) {  return NotFound(); }
            _blogDbContexto.usuarios.Attach(usuario);
            _blogDbContexto.usuarios.Remove(usuario);
            _blogDbContexto.SaveChanges();
            return Ok(usuario);
        }

        //Filtrar por Nombre y por Apellido
        [HttpGet]
        [Route("GetPorNombreyYApellido")]
        public IActionResult GetByNombreApellido(string nombre, string apellido)
        {

            List<usuarios> usuariosFiltrados = _blogDbContexto.usuarios
                .Where(u => u.nombre.ToUpper() == nombre.ToUpper() && u.apellido.ToUpper() == apellido.ToUpper()).ToList();


            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido))
            {
                return BadRequest("El nombre y el apellido deben ser proporcionados.");
            }

            if (usuariosFiltrados.Count == 0)
            {
                return NotFound();
            }

            return Ok(usuariosFiltrados);
        }

        // Filtrar por rol
        [HttpGet]
        [Route("GetByRol")]
        public IActionResult GetByRol(int rolId)
        {
            List<usuarios> usuariosFiltrados = _blogDbContexto.usuarios.Where(e => e.rolId == rolId).ToList();

            if (usuariosFiltrados.Count == 0)
            {
                return NotFound();
            }

            return Ok(usuariosFiltrados);
        }

    }
}
