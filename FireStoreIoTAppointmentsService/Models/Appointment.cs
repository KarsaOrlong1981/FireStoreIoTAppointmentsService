using Google.Cloud.Firestore;

namespace IoTAppointmentsService.Models
{
    [FirestoreData]
    public class Appointment
    {
        [FirestoreProperty]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [FirestoreProperty]
        public string Member { get; set; } 
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public DateTime Date { get; set; }
    }
}
