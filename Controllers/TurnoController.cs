using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Turnos.Models;

namespace Turnos.Controllers
{
    public class TurnoController : Controller
    {

        private readonly TurnosContext _context;

        public IConfiguration _configuration;

        public TurnoController(TurnosContext context, IConfiguration configuration) /*CONSTRUCTOR, le asignamos el contexto y configuración para poder operar a traves de Linq*/
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewData["IdMedico"] = new SelectList((from medico in _context.Medico.ToList() select new { IdMedico = medico.IdMedico, NombreCompleto = medico.Nombre + " " + medico.Apellido }),"IdMedico", "NombreCompleto");
            /*Se llenan 3 parámetros,
            1: Los items que serán mostrados en la lista
            2: El id que estaremos grabando cuando seleccionemos al médico
            3: un string que se mostrará en el ListBox, en este caso, el nombre del médico
            */
            ViewData["IdPaciente"] = new SelectList((from paciente in _context.Paciente.ToList()
                                                     select new { IdPaciente = paciente.IdPaciente, NombreCompleto = paciente.Nombre + " " + paciente.Apellido }),
            "IdPaciente", "NombreCompleto");
            return View();
        }

        public JsonResult ObtenerTurnos(int idMedico)
        {
            var turnos = _context.Turno.Where(t => t.IdMedico == idMedico)
            .Select(t => new{ //subquery sobre nuestra consulta de Linq
                t.IdTurno,
                t.IdMedico,
                t.IdPaciente,
                t.FechaHoraInicio,
                t.FechaHoraFin,
                paciente = t.Paciente.Nombre + ", " + t.Paciente.Apellido,
            })
            .ToList(); 
            //devolveremos un juego de información, definida por la variable turnos

            /*Creamos un método o un EndPoint, recibe un parámetro tipo int, devuelve un JSonResult, nos ayudará para usar
            FullCalendar, este necesita un arhcivo tipo JSon*/
             
            /*Llenamos el objeto turnos, con la lista de turnos que corresponden al médico que se recibe como parámetro*/
            return Json(turnos);
            /*El método JSon convierte el objeto turnos, el cual contiene una colección de datos, lo convierte a una
            colección JSon*/
        }

        [HttpPost]
        public JsonResult GrabarTurno(Turno turno)
        {
            var ok = false;

            try
            {
                _context.Turno.Add(turno);
                _context.SaveChanges();
                ok = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Excepcion encontrada ", e);
                /*Imprimimos la excepción en la consola*/
            }
            var jsonResult = new { ok = ok };
            /*Asignamos al objeto jsonResult un contenido Json {x}, y le colocamos el resultado de la variable ok
            en este caso, el resultado será true*/
            return Json(jsonResult); /*Retornamos el valor que tomó nuestra variable de arriba*/
        }

        [HttpPost]
        public JsonResult EliminarTurno(int idTurno)
        {
            var ok = false;
            try
            {
                var turnoAEliminar = _context.Turno.Where(t => t.IdTurno == idTurno).FirstOrDefault(); 
                /*Encontramos el id*/
                if (turnoAEliminar != null)
                {
                    _context.Turno.Remove(turnoAEliminar);
                    _context.SaveChanges();
                    ok = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Excepcion encontrada ", e);
            }

            var jsonResult = new { ok = ok };
            return Json(jsonResult);
        }

        

    }
}