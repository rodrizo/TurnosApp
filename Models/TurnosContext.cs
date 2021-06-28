using Microsoft.EntityFrameworkCore;
using Turnos.Models;

namespace Turnos.Models
{
    public class TurnosContext : DbContext //Se referencia a DbContext para llamar la BD
    {

        public TurnosContext(DbContextOptions<TurnosContext> opciones)/*El objeto opciones, 
        será de tipo DbContext Options, y a lavez a DbCOntext Options, se le está pasando TurnosContext*/
        : base(opciones)
        { //Se le heredan las opciones base a la clase TurnosContext

        }

        public DbSet<Especialidad> Especialidad { get; set; } //Se define el objeto Especialidad, será de tipo DbSet, y al mismo tiempo ese DbSet será de tipo especialidad
        //Acá se crea la tabla
        public DbSet<Paciente> Paciente { get; set; }
        public DbSet<Medico> Medico { get; set; }
        public DbSet<MedicoEspecialidad> MedicoEspecialidad {get; set; }
        public DbSet<Turno> Turno {get; set; }

        public DbSet<Login> Login { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) //Método protegido, no puede ser modificado desde otro componente
        {
            modelBuilder.Entity<Especialidad>(entidad =>
            {
                entidad.ToTable("Especialidad"); //Nuestra tabla se va a llamar especialidad

                entidad.HasKey(e => e.IdEspecialidad); //El id primario de la tabla será IdEspecialidad

                entidad.Property(e => e.Descripcion)//Agrega un nuevo campo que se llama descripción
                .IsRequired() //El campo descripción será requerido
                .HasMaxLength(200)
                .IsUnicode(false);
            }
            );

            modelBuilder.Entity<Paciente>(entidad =>
            {
                entidad.ToTable("Paciente");

                entidad.HasKey(p => p.IdPaciente);

                entidad.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

                entidad.Property(p => p.Apellido)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

                entidad.Property(p => p.Direccion)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

                entidad.Property(p => p.Telefono)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);


                entidad.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            }
        );

            modelBuilder.Entity<Medico>(entidad =>
            {
                entidad.ToTable("Medico"); //Le damos datos precisos a cada uno de los campos, este trabajo lo realiza
                //EF para tener un mejor control de cada uno

                entidad.HasKey(m => m.IdMedico);

                entidad.Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

                entidad.Property(m => m.Apellido)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
                
                entidad.Property(m => m.Direccion)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);
                
                entidad.Property(m => m.Telefono)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
                
                entidad.Property(m => m.Email)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

                entidad.Property(m => m.HorarioAtencionDesde)
                .IsRequired()
                .IsUnicode(false);

                entidad.Property(m => m.HorarioAtencionHasta)
                .IsRequired()
                .IsUnicode(false);
            }

            );

            modelBuilder.Entity<MedicoEspecialidad>().HasKey(x => new{x.IdMedico, x.IdEspecialidad});
           
            modelBuilder.Entity<MedicoEspecialidad>().HasOne(x => x.Medico)
            .WithMany(p => p.MedicoEspecialidad)
            .HasForeignKey(p => p.IdMedico);

            /*Se definió una restricción entre la tabla médico y la tabla MedicoEspecialidad
            
                Con HasOne (de uno)
                Con WithMany (a muchos)
            */

            modelBuilder.Entity<MedicoEspecialidad>().HasOne(x => x.Especialidad)
            .WithMany(p => p.MedicoEspecialidad)
            .HasForeignKey(p => p.IdEspecialidad);

            modelBuilder.Entity<Turno>(entidad =>
            {
                entidad.ToTable("Turno"); //Le damos datos precisos a cada uno de los campos, este trabajo lo realiza
                //EF para tener un mejor control de cada uno
                /*Se crea la tabla con el nombre TURNO*/

                entidad.HasKey(m => m.IdTurno);

                entidad.Property(m => m.IdPaciente)
                .IsRequired()
                .IsUnicode(false);

                entidad.Property(m => m.IdMedico)
                .IsRequired()
                .IsUnicode(false);
                
                entidad.Property(m => m.FechaHoraInicio)
                .IsRequired()
                .IsUnicode(false);
                
                entidad.Property(m => m.FechaHoraFin)
                .IsRequired()
                .IsUnicode(false);
                
            });
            
            modelBuilder.Entity<Turno>().HasOne(x => x.Paciente)
            .WithMany(p => p.Turno)
            .HasForeignKey(p => p.IdPaciente);

            /*Se crea la restricción, de la relación entre la tabla paciente y la tabla turnos, 
            definimos que la relación será de uno a muchos, un paciente puede tener muchos turnos*/
            
            modelBuilder.Entity<Turno>().HasOne(x => x.Medico)
            .WithMany(p => p.Turno)
            .HasForeignKey(p => p.IdMedico);

            modelBuilder.Entity<Login>(entidad => 
            {
                entidad.ToTable("Login"); //Nuestra tabla en SQL server será TablaLogin

                entidad.HasKey(l => l.LoginId);

                entidad.Property(l => l.Usuario)
                .IsRequired();

                entidad.Property(l => l.Password)
                .IsRequired();
            }

            );

        }
    }



}