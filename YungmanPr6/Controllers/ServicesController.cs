using Microsoft.AspNetCore.Mvc;
using YungmanPr6.Models;

namespace YungmanPr6.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly WebApiDatabaseContext _context;

        public ServicesController(WebApiDatabaseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] bool? isActive,
            [FromQuery] int? durationMinutes)//получение всех записей
        {
           IQueryable<Service> services = _context.Services;
           if(durationMinutes.Value<=0)
            {
                return BadRequest("Время должно быть > 0");
            }
            services = services.Where(s => s.DurationMinutes == durationMinutes);
            if(isActive.HasValue)
            {
                services=services.Where(s=>s.IsActive==isActive.Value);
            }
            return Ok(services.ToList());
        }
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)//получение по айди 
        {
            var servis= _context.Services.Find(id);
            if(servis==null)
                return NotFound();
            return Ok(servis);
        }
        [HttpPost]
        public IActionResult Post(Service service)//добавление записи
        {
            if (service == null)
                return BadRequest();
            _context.Services.Add(service);
            _context.SaveChanges();
            return Created();
        }
        [HttpPut("{id:int}")]//полное изменение записи
        public IActionResult Put(int id,[FromBody] Service service)
        {
            if (id != service.Id)
                return BadRequest();

            var n = _context.Services.Find(id);
            if (n == null)
                return NotFound();

            _context.Entry(n).CurrentValues.SetValues(service);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpPatch]//частичное изменение записи
        public IActionResult Patch(int id, [FromBody] DopSer service)
        {
            var ser = _context.Services.Find(id);
            if (ser == null)
            {
                return NotFound();
            }
            if(service.Name!=null)
            {
                ser.Name = service.Name;
            }
            if (service.Description!=null)
            {
                ser.Description = service.Description;
            }
            if (service.DurationMinutes.HasValue)
            {
                ser.DurationMinutes = service.DurationMinutes.Value;
            }
            if (service.Price.HasValue)
            {
                ser.Price = service.Price.Value;
            }
            if (service.IsActive.HasValue)
            {
                ser.IsActive = service.IsActive.Value;
            }
            _context.SaveChanges();
            return Ok(ser);
        }
        [HttpDelete("{id:int}")]//удаление записи
        public IActionResult Delete(int id)
        {
            var serv = _context.Services.Find(id);
            if (serv == null)
            {
                return NotFound();//404
            }
            _context.Services.Remove(serv);
            _context.SaveChanges();
            return NoContent(); //204
        }

    }
}
