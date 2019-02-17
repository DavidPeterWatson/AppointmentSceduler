using System;

namespace AppointmentScheduler.Domain
{
    public interface IAppointment<IdType>
    {
        IdType MedicalPractitionerId { get; set; }
        IdType ClientId { get; set; }
        TimeSlot TimeSlot { get; set; }
        String Reason { get; set; }
        String Description { get; set; }

    }
}