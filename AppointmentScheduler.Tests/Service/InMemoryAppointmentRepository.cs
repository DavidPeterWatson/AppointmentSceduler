using AppointmentScheduler.Service;
using AppointmentScheduler.Domain;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using AppointmentScheduler.Exception;

namespace AppointmentScheduler.Tests.Service
{
    public class InMemoryAppointmentRepository<IdType> : IAppointmentRepository<IdType>
    {

        private Dictionary<String, IAppointment<IdType>> Appointments = new Dictionary<String, IAppointment<IdType>>();

        public IAppointment<IdType> FindLastAppointmentForMedicalPractitioner(IdType MedicalPractitionerId, DateTime FromDateTime)
        {
            var Query = from FindAppointment in Appointments
                        where FindAppointment.Value.MedicalPractitionerId.Equals(MedicalPractitionerId)
                        orderby FindAppointment.Value.TimeSlot.ToDateTime descending
                        select FindAppointment.Value;

            return Query.FirstOrDefault();
        }

        public IEnumerable<IAppointment<IdType>> GetAllAppointments(int Skip, int Limit)
        {
            var Query = from FindAppointment in Appointments
                        orderby FindAppointment.Value.TimeSlot.ToDateTime ascending
                        select FindAppointment.Value;

            return Query.Skip(Skip).Take(Limit);
        }

        public IEnumerable<IAppointment<IdType>> GetAppointmentsForClient(IdType ClientId, int Skip, int Limit)
        {
            var Query = from FindAppointment in Appointments
                        where FindAppointment.Value.ClientId.Equals(ClientId)
                        orderby FindAppointment.Value.TimeSlot.ToDateTime ascending
                        select FindAppointment.Value;

            return Query.Skip(Skip).Take(Limit);
        }

        public IEnumerable<IAppointment<IdType>> GetAppointmentsForMedicalPractitioner(IdType MedicalPractitionerId, int Skip, int Limit)
        {
            var Query = from FindAppointment in Appointments
                        where FindAppointment.Value.MedicalPractitionerId.Equals(MedicalPractitionerId)
                        orderby FindAppointment.Value.TimeSlot.ToDateTime ascending
                        select FindAppointment.Value;

            return Query.Skip(Skip).Take(Limit);
        }

        public IAppointment<IdType> SaveAppointment(IAppointment<IdType> Appointment)
        {
            String PrimaryKey = String.Format("{0},{1},{2}", Appointment.ClientId, Appointment.MedicalPractitionerId.ToString(), Appointment.TimeSlot.ToString());
            if (!Appointments.TryAdd(PrimaryKey, Appointment)) {
                throw new TimeSlotConflictException();
            }
            return Appointment;
        }
    }
}