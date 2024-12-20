using IoTAppointmentsService.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTAppointmentsService.Database
{
    public class AppointmentsDbContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=appointments.db");
        }
    }
}
