using FireStoreIoTAppointmentsService.Models;
using FireStoreIoTAppointmentsService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FireStoreIoTAppointmentsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _clientService.GetAllClients();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(string id)
        {
            var client = await _clientService.GetClientByIDAsync(id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterOrUpdateClient([FromBody] Clients client)
        {
            if (client == null || string.IsNullOrEmpty(client.Id) || string.IsNullOrEmpty(client.DeviceToken))
                return BadRequest("Invalid client data.");

            await _clientService.OnTokenRefresh(client);
            return Ok("Client registered/updated successfully.");
        }
    }
}
