using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnos.Models;

/*MÉTODOS ASÍNCRONOS

estos se encargan de dar las peticiones de un usuario en distintos canales y al mismo tiempos, pues si 
dejamos un método sin tareas asíncronas, el usuario 2 tendría que esperar a que el usuario 1 termine
de ejecutar su petición para poder usar la app

*/
namespace Turnos.Controllers
{
    public class EspecialidadController : Controller
    {
        private readonly TurnosContext _context; //Inicializamos el objeto _context
        public EspecialidadController(TurnosContext context) //Le pasamos al constructor un objeto llamado Context, de tipo TurnosContext
        {
            _context = context;
        }

        public async Task<IActionResult> Index()//Muestra el resultado en la vista del usuario
        {
            return View(await _context.Especialidad.ToListAsync()); //Pasamos a la vista el parámetro _context, accedemos a la tabla especialidad, 
            //y mediante ToList entramos a Linq, que devuelve un registro de cualquier tabla
        }

        public async Task<IActionResult> Edit(int? id) //Se le añade el signo de pregunta porque puede ser un valor null, el signo permite valores nulos para ese parámetro
        {
            if (id == null) //Se retorna un error, cuando no se encuentra ningún dato
            {
                return NotFound();
            }


            var especialidad = await _context.Especialidad.FindAsync(id); /*var permite que automáticamente se ajuste el tipo de dato
                                                                en relación al valor que se le asigna a ese objeto, lo que esto retorna
                                                                asigna el tipo de dato correcto.
                                                                */

            if (especialidad == null)//Retorna un error porque si el valor no se encuentra en la base, 
                                     // pero si se añadió en index, tira un error
            {
                return NotFound();
            }

            return View(especialidad);//Se le pasa a la vista el objeto para que lo muestre dentro de su contenido
        }

        [HttpPost] //Esto diferencia del método Edit, pues este se encarga de enviar la petición a la DB
        public async Task<IActionResult> Edit(int id, [Bind("IdEspecialidad", "Descripcion")] Especialidad especialidad)
        //Recibimos del formulario los campos de Especialidad y descripción
        {
            if (id != especialidad.IdEspecialidad)
            {
                return NotFound(); //Validamos el caso en el que id y idEspecialidad sean diferentes 
            }

              /*var permite que automáticamente se ajuste el tipo de dato*/

            if (ModelState.IsValid) //Si el enlace se realizó correctamente, quiere decir que estamos en condiciones de grabar nuestros datos
            {
                _context.Update(especialidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); //Le pasamos el action al cual queremos redirigir luego de los cambios
            }
            return View(especialidad);
        }

        //GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) //Se retorna un error, cuando no se encuentra ningún dato
            {
                return NotFound();
            }

            var especialidad = await _context.Especialidad.FirstOrDefaultAsync(e => e.IdEspecialidad == id);
            /*accedemos al modelo especialidad y con Linq accedemos a dicha acción*/

            if (especialidad == null) //Si no encuentra una especialidad, nos lanza un error
            {
                return NotFound();
            }
            return View(especialidad);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var especialidad = await _context.Especialidad.FindAsync(id);//Recibimos ID como parámetro, find encuentra la especialidad, y lo va a redireccionar al objeto especialidad
            _context.Especialidad.Remove(especialidad);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("IdEspecialidad, Descripcion")] Especialidad especialidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(especialidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();

        }
    }
}