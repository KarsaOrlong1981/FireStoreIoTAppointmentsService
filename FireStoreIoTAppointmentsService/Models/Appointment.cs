

namespace IoTAppointmentsService.Models
{
    public class Appointment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
       
        public string Member { get; set; } 
       
        public string Description { get; set; }
        
        public DateTime Date { get; set; }
    }
}
