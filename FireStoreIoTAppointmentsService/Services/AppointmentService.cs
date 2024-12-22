using FireStoreIoTAppointmentsService.Services;
using IoTAppointmentsService.Database;
using IoTAppointmentsService.Models;

namespace IoTAppointmentsService.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppointmentsDbContext _context;
        private readonly IFirebaseService _firebaseService;
        private readonly IClientService _clientService;

        public AppointmentService(IFirebaseService firebaseService, IClientService clientService)
        {
            _context = new AppointmentsDbContext();
            _firebaseService = firebaseService;
            _clientService = clientService;
            _context.Database.EnsureCreated();
        }

        public IEnumerable<Appointment> GetAllAppointments()
        {
            return _context.Appointments.ToList(); 
        }

        public IEnumerable<Appointment> GetAppointmentByMonth(string member, int year, int month)
        {
            return _context.Appointments
                .Where(a =>
                    (string.IsNullOrEmpty(member) || a.Member == member) &&
                    a.Date.Month == month && a.Date.Year == year)
                .ToList();
        }

        public IEnumerable<Appointment> GetAppointmentsByDay(string member, int year, int month, int day)
        {
            return _context.Appointments
                .Where(a =>
                    (string.IsNullOrEmpty(member) || a.Member == member) &&
                    a.Date.Month == month && a.Date.Year == year && a.Date.Day == day)
                .ToList();
        }

        public IEnumerable<Appointment> GetAppointmentsByYear(string member, int year)
        {
            return _context.Appointments
                .Where(a =>
                (string.IsNullOrEmpty(member) || a.Member == member) &&
                a.Date.Year == year)
                .OrderBy(a => a.Date)
                .ToList();
        }

        public IEnumerable<Appointment> GetAppointmentsForMember(string member)
        {
            return _context.Appointments
                .Where(a => a.Member == member)
                .OrderBy(a => a.Date)
                .ToList();
        }

        public async Task AddAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
            var title = $"{appointment.Member} hat einen neuen Termin eingetragen.";
            var message = $"Am {appointment.Date.Date.ToString("dd.MM.yyyy")}: \n{appointment.Description}";
            await SendNotificationToAllClients(title, message);
        }

        private async Task SendNotificationToAllClients(string title, string message)
        {
            try
            {
                var clients = await _clientService.GetAllClients();
                foreach (var client in clients)
                {
                    await _firebaseService.SendNotificationAsync(client.DeviceToken, title, message);
                }
            }
            catch (Exception ex) { }  
        }

        public async Task<bool> DeleteAppointment(string id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
                var title = $"{appointment.Member} hat einen Termin gelöscht.";
                var message = $"Am {appointment.Date.Date.ToString("dd.MM.yyyy")}: \n{appointment.Description}";
                await SendNotificationToAllClients(title, message);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAppointment(Appointment updatedAppointment)
        {
            var existingAppointment = _context.Appointments.Find(updatedAppointment.Id);
            if (existingAppointment == null)
            {
                return false; 
            }

            // Aktualisiere die Werte des bestehenden Appointments
            existingAppointment.Description = updatedAppointment.Description;
            existingAppointment.Date = updatedAppointment.Date;
            existingAppointment.Member = updatedAppointment.Member;

            // Änderungen speichern
            _context.SaveChanges();
            var title = $"{updatedAppointment.Member} hat Änderungen an einem Termin vorgenommen.";
            var message = $"Am {updatedAppointment.Date.Date.ToString("dd.MM.yyyy")}: \n{updatedAppointment.Description}";
            await SendNotificationToAllClients(title, message);
            return true; // Erfolgreich aktualisiert
        }

        public void DeleteAllAppointments() 
        {
            _context.Appointments.RemoveRange(_context.Appointments);
            _context.SaveChanges();
        }
    }
}
