namespace AppointmentScheduler.Domain
{
    public interface IAppointment<IdType>
    {
        IdType MedicalPractitionerId { get; set; }
        IdType ClientId { get; set; }
        TimeSlot TimeSlot { get; set; }
        string Reason { get; set; }
        string Description { get; set; }
    }
}