using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route(""),ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var result = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if(result is null)
            {
                return NotFound();
            }
            result.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == result.Id).ToList();

            return Ok(result);

        }

        [HttpGet("name:string")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(c => c.Name == name);
            if(result is null)
            {
                return NotFound();
            }
            foreach(var s in result)
            {
                s.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == s.Id).ToList();
            }

            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _context.CelestialObjects.ToList();
            foreach (var s in result)
            {
                s.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == s.Id).ToList();
            }

            return Ok(result);
        }
    }
}
