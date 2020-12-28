using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name="GetById")]
        public IActionResult GetById(int id)
        {
            var c = _context.CelestialObjects.Find(id);
            if (c == null) return NotFound();
            c.Satellites = _context.CelestialObjects.Where(a => a.OrbitedObjectId == id).ToList<CelestialObject>();
            return Ok(c);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var cs = _context.CelestialObjects.Where(a => a.Name == name);
            if (!cs.Any()) return NotFound();
            foreach (var c in cs)
            {
                c.Satellites = _context.CelestialObjects.Where(a => a.OrbitedObjectId == c.Id).ToList<CelestialObject>();
            }
            return Ok(cs);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var cs = _context.CelestialObjects;
            if (!cs.Any()) return NotFound();
            foreach (var c in cs)
            {
                c.Satellites = _context.CelestialObjects.Where(a => a.OrbitedObjectId == c.Id).ToList<CelestialObject>();
            }
            return Ok(cs);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var c = _context.CelestialObjects.Find(id);
            if (c == null) return NotFound();
            c.Name = celestialObject.Name;
            c.OrbitalPeriod = celestialObject.OrbitalPeriod;
            c.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.Update(c);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var c = _context.CelestialObjects.Find(id);
            if (c == null) return NotFound();
            c.Name = name;
            _context.Update(c);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cs = _context.CelestialObjects.Where(a => a.Id == id || a.OrbitedObjectId == id);
            if (!cs.Any()) return NotFound();
            _context.CelestialObjects.RemoveRange(cs);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
