using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Turnos.Models
{
    public class Especialidad
    {
        [Key]
        public int IdEspecialidad { get; set; }
        [StringLength(200, ErrorMessage = "El campo descripción debe tener como máximo 200 caracteres.")] 
        /*Limita la cantidad de caracteres*/
        [Required (ErrorMessage = "Debe ingresar una descripción.")]
        [Display (Name = "Descripción", Prompt = "Ingrese una descripción")]
        public string Descripcion { get; set; }
        public List<MedicoEspecialidad> MedicoEspecialidad { get; set; }

    }
}