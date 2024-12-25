using FireStoreIoTAppointmentsService.Models;
using IoTAppointmentsService.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTAppointmentsService.Database
{
    public class AppointmentsDbContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoTask> TodoTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=appointments.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoTask>()
                .HasOne<TodoList>() // Optional: Navigationseigenschaft nicht mehr vorhanden
                .WithMany(t => t.Todos) // Wenn `Todos` entfernt, hier anpassen
                .HasForeignKey(t => t.TodoListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
