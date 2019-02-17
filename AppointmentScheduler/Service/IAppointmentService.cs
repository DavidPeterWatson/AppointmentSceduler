using AppointmentScheduler.Domain;
using System;
using System.Collections.Generic;

namespace AppointmentScheduler.Service
{
    public interface IAppointmentService<IdType>
    {
        IAppointment<IdType> ScheduleAppointment(IAppointment<IdType> Appointment);

        TimeSlot FindNextTimeSlotForMedicalPractitioner(IdType MedicalPractitionerId, DateTime FromDateTime);

        IEnumerable<IAppointment<IdType>> GetAllAppointments(int Skip, int Limit);

        IEnumerable<IAppointment<IdType>> FindAppointmentsForMedicalPractitioner(IdType MedicalPractitionerId, int Skip, int Limit);

        IEnumerable<IAppointment<IdType>> FindAppointmentsForClient(IdType ClientId, int Skip, int Limit);
    }
}