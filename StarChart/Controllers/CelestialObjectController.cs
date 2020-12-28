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
    }
}
