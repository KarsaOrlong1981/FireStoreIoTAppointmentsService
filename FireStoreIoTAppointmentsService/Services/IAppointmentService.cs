using IoTAppointmentsService.Models;

namespace  IoTAppointmentsService.Services
{
    public interface IAppointmentService
    {
        IEnumerable<Appointment> GetAllAppointments();
        IEnumerable<Appointment> GetAppointmentsByDay(string member, int year, int month, int day);
        IEnumerable<Appointment> GetAppointmentByMonth(string member, int year, int month);
        IEnumerable<Appointment> GetAppointmentsForMember(string member);
        IEnumerable<Appointment> GetAppointmentsByYear(string member, int year);
        void AddAppointment(Appointment appointment);
        bool UpdateAppointment(Appointment appointment);

        bool DeleteAppointment(string id);
        void DeleteAllAppointments();
    }
}
