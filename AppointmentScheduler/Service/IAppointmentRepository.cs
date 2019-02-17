using AppointmentScheduler.Domain;
using System;
using System.Collections.Generic;

namespace AppointmentScheduler.Service
{
    public interface IAppointmentRepository<IdType>
    {
        IAppointment<IdType> SaveAppointment(IAppointment<IdType> Appointment);

        IAppointment<IdType> FindLastAppointmentForMedicalPractitioner(IdType MedicalPractitionerId, DateTime FromDateTime);

        IEnumerable<IAppointment<IdType>> GetAllAppointments(int Skip, int Limit);

        IEnumerable<IAppointment<IdType>> GetAppointmentsForMedicalPractitioner(IdType MedicalPractitionerId, int Skip, int Limit);

        IEnumerable<IAppointment<IdType>> GetAppointmentsForClient(IdType ClientId, int Skip, int Limit);
    }
}