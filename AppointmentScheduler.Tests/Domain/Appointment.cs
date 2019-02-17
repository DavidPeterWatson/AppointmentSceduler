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

        public Appointment(IdType ClientId, IdType MedicalPractitionerId, TimeSlot TimeSlot, string Description, string Reason)
        {
            this.ClientId = ClientId;
            this.MedicalPractitionerId = MedicalPractitionerId;
            this.TimeSlot = TimeSlot;
            this.Description = Description;
            this.Reason = Reason;
        }

    }
}