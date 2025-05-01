using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaProDB.Models;

namespace LigaProDB.Controllers
{
    public class JugadoresController : Controller
    {
        private readonly LigaProDataBase _context;

        public JugadoresController(LigaProDataBase context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var equipos = await _context.Equipo.ToListAsync();
            return View(equipos);
        }

        public IActionResult CrearPorEquipo(int equipoId)
        {
            ViewBag.EquipoId = equipoId;
            ViewBag.NombreEquipo = _context.Equipo.Find(equipoId)?.Nombre;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPorEquipo(Jugador jugador)
        {
            if (ModelState.IsValid)
            {
                _context.Jugador.Add(jugador);
                await _context.SaveChangesAsync();
                return RedirectToAction("ListaPorEquipo", new { equipoId = jugador.EquipoId });
            }

            ViewBag.EquipoId = jugador.EquipoId;
            ViewBag.NombreEquipo = _context.Equipo.Find(jugador.EquipoId)?.Nombre;
            return View(jugador);
        }

        public async Task<IActionResult> ListaPorEquipo(int equipoId)
        {
            var jugadores = await _context.Jugador
                .Where(j => j.EquipoId == equipoId)
                .ToListAsync();

            ViewBag.NombreEquipo = _context.Equipo.Find(equipoId)?.Nombre;
            return View(jugadores);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var jugador = await _context.Jugador.FindAsync(id);
            if (jugador != null)
            {
                int equipoId = jugador.EquipoId;
                _context.Jugador.Remove(jugador);
                await _context.SaveChangesAsync();
                return RedirectToAction("ListaPorEquipo", new { equipoId });
            }

            return NotFound();
        }
    }
}
