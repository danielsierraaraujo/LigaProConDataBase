using LigaProDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LigaProDB.Controllers
{
    public class EstadisticasController : Controller
    {
        private readonly LigaProDataBase _context;

        public EstadisticasController(LigaProDataBase context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var goleadores = await _context.Jugador
                .OrderByDescending(j => j.Goles)
                .Take(5)
                .Include(j => j.Equipo)
                .ToListAsync();

            var asistidores = await _context.Jugador
                .OrderByDescending(j => j.Asistencias)
                .Take(5)
                .Include(j => j.Equipo)
                .ToListAsync();

            var presupuestos = await _context.Equipo
                .OrderByDescending(e => _context.Jugador.Where(j => j.EquipoId == e.Id).Sum(j => j.Sueldo))
                .Take(5)
                .ToListAsync();

            ViewBag.Goleadores = goleadores;
            ViewBag.Asistidores = asistidores;
            ViewBag.Presupuestos = presupuestos;

            return View();
        }
    }
}
