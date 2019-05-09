using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(c => c.Name == name).ToList();
            if(!result.Any())
            {
                return NotFound();
            }
            foreach (var s in result)
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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestial)
        {
            _context.CelestialObjects.Add(celestial);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestial.Id }, celestial);
        }

        [HttpPut]
        public IActionResult Update(int id, CelestialObject celestial)
        {
            var oldObject = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);

            if(oldObject is null)
            {
                return NotFound();
            }

            oldObject.Name = celestial.Name;
            oldObject.OrbitalPeriod = celestial.OrbitalPeriod;
            oldObject.OrbitedObjectId = celestial.OrbitedObjectId;

            _context.Update(oldObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch]
        public IActionResult RenameObject(int id, string name)
        {
            var celestial = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);

            if(celestial is null)
            {
                return NotFound();
            }

            celestial.Name = name;
            _context.Update(celestial);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
