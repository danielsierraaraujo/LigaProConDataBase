using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaProDB.Models;
using Microsoft.AspNetCore.Hosting;

namespace LigaProDB.Controllers
{
    public class EquiposController : Controller
    {
        private readonly LigaProDataBase _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EquiposController(LigaProDataBase context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Equipos
        public async Task<IActionResult> Index()
        {
            var equiposOrdenados = await _context.Equipo
                .OrderByDescending(e => e.Puntos)
                .ToListAsync();
            return View(equiposOrdenados);
        }

        // GET: Equipos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var equipo = await _context.Equipo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipo == null) return NotFound();

            return View(equipo);
        }

        // GET: Equipos/Create
        public IActionResult Create()
        {
            var logosPath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
            var logos = Directory.Exists(logosPath)
                ? Directory.GetFiles(logosPath).Select(Path.GetFileName).ToList()
                : new List<string>();
            ViewBag.Logos = logos;

            return View();
        }

        // POST: Equipos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Logo,PartidosJugados,PartidosGanados,PartidosEmpatados,PartidosPerdidos")] Equipo equipo)
        {
            int suma = equipo.PartidosGanados + equipo.PartidosEmpatados + equipo.PartidosPerdidos;
            if (suma != equipo.PartidosJugados)
            {
                ModelState.AddModelError(string.Empty, "La suma de ganados, empatados y perdidos debe igualar a los partidos jugados.");
            }

            equipo.Puntos = equipo.PartidosGanados * 3 + equipo.PartidosEmpatados;

            if (ModelState.IsValid)
            {
                _context.Add(equipo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var logosPath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
            ViewBag.Logos = Directory.Exists(logosPath)
                ? Directory.GetFiles(logosPath).Select(Path.GetFileName).ToList()
                : new List<string>();

            return View(equipo);
        }

        // GET: Equipos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var equipo = await _context.Equipo.FindAsync(id);
            if (equipo == null) return NotFound();

            var logosPath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
            ViewBag.Logos = Directory.Exists(logosPath)
                ? Directory.GetFiles(logosPath).Select(Path.GetFileName).ToList()
                : new List<string>();

            return View(equipo);
        }

        // POST: Equipos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Logo,PartidosJugados,PartidosGanados,PartidosEmpatados,PartidosPerdidos")] Equipo equipo)
        {
            if (id != equipo.Id) return NotFound();

            int suma = equipo.PartidosGanados + equipo.PartidosEmpatados + equipo.PartidosPerdidos;
            if (suma != equipo.PartidosJugados)
            {
                ModelState.AddModelError(string.Empty, "La suma de ganados, empatados y perdidos debe igualar a los partidos jugados.");
            }

            equipo.Puntos = equipo.PartidosGanados * 3 + equipo.PartidosEmpatados;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipoExists(equipo.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            var logosPath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
            ViewBag.Logos = Directory.Exists(logosPath)
                ? Directory.GetFiles(logosPath).Select(Path.GetFileName).ToList()
                : new List<string>();

            return View(equipo);
        }

        // GET: Equipos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var equipo = await _context.Equipo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipo == null) return NotFound();

            return View(equipo);
        }

        // POST: Equipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipo = await _context.Equipo.FindAsync(id);
            if (equipo != null)
            {
                _context.Equipo.Remove(equipo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EquipoExists(int id)
        {
            return _context.Equipo.Any(e => e.Id == id);
        }
    }
}
