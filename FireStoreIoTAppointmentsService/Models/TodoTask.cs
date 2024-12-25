namespace IoTAppointmentsService.Models
{
    public class TodoTask
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public ECategorieType CategorieType { get; set; }
        public string TodoListId { get; set; }
    }
}
