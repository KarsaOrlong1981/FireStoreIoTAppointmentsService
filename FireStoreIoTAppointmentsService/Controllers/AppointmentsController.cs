using IoTAppointmentsService.Models;
using IoTAppointmentsService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IoTAppointmentsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public AppointmentsController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Appointment>> GetAllAppointments()
        {
            return await _firebaseService.GetAllAppointments();
        }

        [HttpGet("ByMonth")]
        public async Task<IEnumerable<Appointment>> GetAppointmentsByMonth([FromQuery] string member, [FromQuery] int year, [FromQuery] int month)
        {
            return await _firebaseService.GetAppointmentByMonth(member, year, month);
        }

        [HttpGet("ByDay")]
        public async Task<IEnumerable<Appointment>> GetAppointmentsByDay([FromQuery] string member, [FromQuery] int year, [FromQuery] int month, [FromQuery] int day)
        {
            return await _firebaseService.GetAppointmentsByDay(member, year, month, day);
        }

        [HttpGet("ByYear")]
        public async Task<IEnumerable<Appointment>> GetAppointmentsByYear([FromQuery] string member, [FromQuery] int year)
        {
            return await _firebaseService.GetAppointmentsByYear(member, year);
        }

        [HttpGet("ForMember")]
        public async Task<IEnumerable<Appointment>> GetAppointmentsForMember([FromQuery] string member)
        {
            return await _firebaseService.GetAppointmentsForMember(member);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(string id, [FromBody] Appointment appointment)
        {
            if (appointment.Id != id)
            {
                return BadRequest("The ID in the URL does not match the ID in the request body.");
            }

            var updated = await _firebaseService.UpdateAppointment(appointment);
            if (updated)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddAppointment([FromBody] Appointment appointment)
        {
            await _firebaseService.AddAppointment(appointment);
            return CreatedAtAction(nameof(AddAppointment), appointment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(string id)
        {
            var deleted = await _firebaseService.DeleteAppointment(id);
            if (deleted)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllAppointments()
        {
            await _firebaseService.DeleteAllAppointments();
            return NoContent();
        }
    }
}
