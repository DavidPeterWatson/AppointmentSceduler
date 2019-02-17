using AppointmentScheduler.Domain;

namespace AppointmentScheduler.Tests.Domain
{
    public class Appointment<IdType> : IAppointment<IdType>
    {
        public IdType ClientId { get; set; }
        public IdType MedicalPractitionerId { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }

        public Appointment() { }

        public Appointment(IdType clientId, IdType medicalPractitionerId, TimeSlot timeSlot, string description, string reason)
        {
            this.ClientId = clientId;
            this.MedicalPractitionerId = medicalPractitionerId;
            this.TimeSlot = timeSlot;
            this.Description = description;
            this.Reason = reason;
        }

    }
}