using IoTAppointmentsService.Models;
using IoTAppointmentsService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IoTAppointmentsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("Ping")]
        public bool GetPing()
        {
            return true;
        }

        [HttpGet("All")]
        public IEnumerable<Appointment> GetAllAppointments()
        {
            return _appointmentService.GetAllAppointments();
        }

        // GET: api/Appointments/ByMonth
        [HttpGet("ByMonth")]
        public IEnumerable<Appointment> GetAppointmentsByMonth([FromQuery] string member, [FromQuery] int year, [FromQuery] int month)
        {
            return _appointmentService.GetAppointmentByMonth(member, year, month);
        }

        // GET: api/Appointments/ByDay
        [HttpGet("ByDay")]
        public IEnumerable<Appointment> GetAppointmentsByDay([FromQuery] string member, [FromQuery] int year, [FromQuery] int month, [FromQuery] int day)
        {
            return _appointmentService.GetAppointmentsByDay(member, year, month, day);
        }

        // GET: api/Appointments/ByYear
        [HttpGet("ByYear")]
        public IEnumerable<Appointment> GetAppointmentsByYear([FromQuery] string member, [FromQuery] int year)
        {
            return _appointmentService.GetAppointmentsByYear(member, year);
        }

        // GET: api/Appointments/ForMember
        [HttpGet("ForMember")]
        public IEnumerable<Appointment> GetAppointmentsForMember([FromQuery] string member)
        {
            return _appointmentService.GetAppointmentsForMember(member);
        }

        // PUT: api/Appointments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(string id, [FromBody] Appointment appointment)
        {
            if (appointment.Id != id)
            {
                return BadRequest("The ID in the URL does not match the ID in the request body.");
            }

            var updated = await _appointmentService.UpdateAppointment(appointment);
            if (updated)
            {
                return NoContent();
            }

            return NotFound();
        }

        // POST: api/Appointments
        [HttpPost]
        public async Task<IActionResult> AddAppointment([FromBody] Appointment appointment)
        {
            await _appointmentService.AddAppointment(appointment);
            return CreatedAtAction(nameof(AddAppointment), appointment);
        }

        // DELETE: api/Appointments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(string id)
        {
            if (await _appointmentService.DeleteAppointment(id))
            {
                return NoContent();
            }
            return NotFound();
        }

        // DELETE: api/Appointments
        [HttpDelete]
        public IActionResult DeleteAllAppointments()
        {
            _appointmentService.DeleteAllAppointments();
            return NoContent();
        }
    }
}
