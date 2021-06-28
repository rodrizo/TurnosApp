using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnos.Models;

namespace Turnos.Controllers
{
    public class PacienteController : Controller
    {
        private readonly TurnosContext _context;
        public PacienteController(TurnosContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Paciente.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Paciente.FirstOrDefaultAsync(p => p.IdPaciente == id);
            if (paciente == null)
            {
                return NotFound();
            }
            return View(paciente);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Se encarga de validar que nuestro método ha sido ejecutado a traves del formulario, previene ataques que provienen fuera de la app
        public async Task<IActionResult> Create([Bind("IdPaciente, Nombre, Apellido, Direccion, Telefono, Email")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Paciente.FindAsync(id);

            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPaciente, Nombre, Apellido, Direccion, Telefono, Email")] Paciente paciente)
        {
            if (id != paciente.IdPaciente)
            {
                return NotFound();
            }

            if (ModelState.IsValid) //Si el enlace se realizó correctamente, quiere decir que estamos en condiciones de grabar nuestros datos
            {
                _context.Update(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); //Le pasamos el action al cual queremos redirigir luego de los cambios
            }
            return View(paciente); //En el caso que el modelstate sea falso, retorna la vista del paciente
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Paciente.FirstOrDefaultAsync(e => e.IdPaciente == id);


            if (paciente == null) //Si no encuentra una especialidad, nos lanza un error
            {
                return NotFound();
            }
            return View(paciente);
        }

        [HttpPost, ActionName("Delete")] //Usamos el action name para renombrar el método desde la vista, si lo dejamos igual
                                        // al Delete Get, nos dará error
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {

            if(id == null)
            {
                return NotFound();
            }
            var paciente = await _context.Paciente.FindAsync(id);//Recibimos ID como parámetro, find encuentra el paciente, y lo va a redireccionar al objeto paciente
            if(paciente == null)
            {
                return NotFound();
            }

            _context.Paciente.Remove(paciente);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}