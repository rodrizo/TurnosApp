namespace Turnos.Models
{
    public class MedicoEspecialidad{

        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public Medico Medico  { get; set; }
        public Especialidad Especialidad  { get; set; }

    }
  
    /*Este modelo tendrá una relación directa con el modelo Médico y Especialidad
    a través de IdMedico y IdEspecialidad*/

}
