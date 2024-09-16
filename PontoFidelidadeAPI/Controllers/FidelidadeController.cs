using Microsoft.AspNetCore.Mvc;
using PontoFidelidadeAPI.Models;
using PontoFidelidadeAPI.Services;

namespace PontoFidelidadeAPI.Controllers
{
    [ApiController]
    [Route("api/v1/clientes")]
    public class FidelidadeController : ControllerBase
    {
        private readonly FidelidadeService _service;
        public FidelidadeController(FidelidadeService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Cliente pessoa)
        {
            try
            {
                return Ok(_service.Save(pessoa));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        [HttpDelete("/api/v1/clientes/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/api/v1/clientes/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_service.Get(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("/api/v1/clientes/{id}")]
        public IActionResult Put(int id, [FromBody] Cliente pessoa)
        {
            try
            {
                _service.Update(pessoa);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
