using IoTAppointmentsService.Models;

namespace IoTAppointmentsService.Services
{
    public interface IAppointmentService
    {
        IEnumerable<Appointment> GetAllAppointments();
        IEnumerable<Appointment> GetAppointmentsByDay(string member, int year, int month, int day);
        IEnumerable<Appointment> GetAppointmentByMonth(string member, int year, int month);
        IEnumerable<Appointment> GetAppointmentsForMember(string member);
        IEnumerable<Appointment> GetAppointmentsByYear(string member, int year);
        Task AddAppointment(Appointment appointment);
        Task<bool> UpdateAppointment(Appointment appointment);

        Task<bool> DeleteAppointment(string id);
        void DeleteAllAppointments();
    }
}
