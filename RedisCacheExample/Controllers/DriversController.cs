using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisCacheExample.Data;
using RedisCacheExample.Models;
using RedisCacheExample.Services;

namespace RedisCacheExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class DriversController : ControllerBase
    {
        private readonly ILogger<DriversController> _logger;
        private readonly ICacheService _cacheService;
        private readonly AppDbContext _context;
        public DriversController(ILogger<DriversController> logger, ICacheService cacheService, AppDbContext context)
        {
            _logger = logger;
            _cacheService = cacheService;
            _context = context;
        }
        [HttpGet("GetDrivers")]
        public async Task<IActionResult> GetDrivers()
        {
            var cacheData = _cacheService.GetData<IEnumerable<Driver>>("drivers");

            if (cacheData != null && cacheData.Count() > 0)
                return Ok(cacheData);

            cacheData = await _context.Drivers.ToListAsync();

            var expiryTime = DateTimeOffset.Now.AddSeconds(30);
            _cacheService.SetData<IEnumerable<Driver>>("drivers", cacheData, expiryTime);
            return Ok(cacheData);
        }
        [HttpPost("AddDriver")]
        public async Task<IActionResult> AddDriver(Driver value)
        {
            var addedObj = await _context.Drivers.AddAsync(value);

            var expiryTime = DateTimeOffset.Now.AddSeconds(30);

            _cacheService.SetData<Driver>($"driver{value.Id}", addedObj.Entity, expiryTime);

            await _context.SaveChangesAsync();
            return Ok(addedObj.Entity);
        }
        [HttpDelete("DeleteDriver")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var exist = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == id);

            if (exist != null)
            {
                _context.Remove(exist);
                _cacheService.RemoveData($"driver{id}");
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }
    }
}
