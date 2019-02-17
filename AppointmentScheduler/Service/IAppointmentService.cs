using AppointmentScheduler.Domain;
using System;
using System.Collections.Generic;

namespace AppointmentScheduler.Service
{
    public interface IAppointmentService<IdType>
    {
        IAppointment<IdType> ScheduleAppointment(IAppointment<IdType> appointment);

        TimeSlot FindNextTimeSlotForMedicalPractitioner(IdType medicalPractitionerId, DateTime fromDateTime);

        IEnumerable<IAppointment<IdType>> GetAllAppointments(int skip, int limit);

        IEnumerable<IAppointment<IdType>> FindAppointmentsForMedicalPractitioner(IdType medicalPractitionerId, int skip, int limit);

        IEnumerable<IAppointment<IdType>> FindAppointmentsForClient(IdType clientId, int skip, int limit);
    }
}