using System;
using System.ComponentModel.DataAnnotations;

namespace Turnos.Models
{

    public class Turno
    {
        [Key]
        public int IdTurno { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        [Display(Name = "Fecha hora de inicio")]
        public DateTime FechaHoraInicio { get; set; }
        [Display(Name = "Fecha hora de fin")]
        public DateTime FechaHoraFin { get; set; }
        public Paciente Paciente { get; set; }
        public Medico Medico { get; set; }
    }



}