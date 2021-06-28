using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Turnos.Models
{
    public class Medico
    {
        [Key]
        public int IdMedico { get; set; }
        [Required(ErrorMessage = "Debe ingresar un nombre.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Debe ingresar un apellido.")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Debe ingresar una dirección")]
        [Display(Name = "Dirección", Prompt = "Ingrese una dirección.")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "Debe ingresar un teléfono.")]
        [Display(Name = "Teléfono", Prompt = "Ingrese una teléfono.")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "Debe ingresar un Email.")]
        [EmailAddress(ErrorMessage = "Debe ingresar una dirección de correo válida.")]
        public string Email { get; set; }
        [Display(Name = "Horario desde")]
        [DataType (DataType.Time)]/* 
        [DisplayFormat (DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)] */
        public DateTime HorarioAtencionDesde { get; set; }
        [Display(Name = "Horario hasta")]
        [DataType(DataType.Time)]/* 
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)] */
        public DateTime HorarioAtencionHasta { get; set; }
        public List<MedicoEspecialidad> MedicoEspecialidad { get; set; }
        public List<Turno> Turno { get; set; } /*Un médico puede estas relacionado con muchos turnos*/


        /*La propiedad List nos sirve para mostrar una lista de especialidades disponibles a un médico
        y así poder elegir una para x médico
        
        Lo que se hace es conectar el modelo con el modelo MedicoEspecialidad
        */
    }
}