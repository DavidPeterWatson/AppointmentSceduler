using AppointmentScheduler.Service;
using AppointmentScheduler.Domain;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using AppointmentScheduler.Exception;
using System.Collections.Concurrent;

namespace AppointmentScheduler.Tests.Service
{
    public class InMemoryAppointmentRepository<IdType> : IAppointmentRepository<IdType>
    {

        private ConcurrentDictionary<String, IAppointment<IdType>> appointments = new ConcurrentDictionary<String, IAppointment<IdType>>();
        private ConcurrentDictionary<String, IAppointment<IdType>> clientAppointments = new ConcurrentDictionary<String, IAppointment<IdType>>();
        private ConcurrentDictionary<String, IAppointment<IdType>> medicalPractitionerAppointments = new ConcurrentDictionary<String, IAppointment<IdType>>();

        private Object UniqueKeyLock = new Object();

        public IAppointment<IdType> FindLastAppointmentForMedicalPractitioner(IdType medicalPractitionerId, DateTime fromDateTime)
        {
            var query = from findAppointment in appointments
                        where findAppointment.Value.MedicalPractitionerId.Equals(medicalPractitionerId)
                        orderby findAppointment.Value.TimeSlot.ToDateTime descending
                        select findAppointment.Value;

            return query.FirstOrDefault();
        }

        public IEnumerable<IAppointment<IdType>> GetAllAppointments(int skip, int limit)
        {
            var query = from FindAppointment in appointments
                        orderby FindAppointment.Value.TimeSlot.ToDateTime ascending
                        select FindAppointment.Value;

            return query.Skip(skip).Take(limit);
        }

        public IEnumerable<IAppointment<IdType>> FindAppointmentsForClient(IdType clientId, int skip, int limit)
        {
            var query = from findAppointment in appointments
                        where findAppointment.Value.ClientId.Equals(clientId)
                        orderby findAppointment.Value.TimeSlot.ToDateTime ascending
                        select findAppointment.Value;

            return query.Skip(skip).Take(limit);
        }

        public IEnumerable<IAppointment<IdType>> FindAppointmentsForMedicalPractitioner(IdType medicalPractitionerId, int skip, int limit)
        {
            var query = from findAppointment in appointments
                        where findAppointment.Value.MedicalPractitionerId.Equals(medicalPractitionerId)
                        orderby findAppointment.Value.TimeSlot.ToDateTime ascending
                        select findAppointment.Value;

            return query.Skip(skip).Take(limit);
        }

        public IAppointment<IdType> SaveAppointment(IAppointment<IdType> appointment)
        {
            CheckAllUniqueKeys(appointment);
            AddAllUniqueKeys(appointment);
            return appointment;
        }

        private void CheckAllUniqueKeys(IAppointment<IdType> appointment)
        {
            CheckPrimaryKey(appointment);
            CheckClientTimeSlotUniqueKey(appointment);
            CheckMedicalPractitionerTimeSlotUniqueKey(appointment);
        }

        private void AddAllUniqueKeys(IAppointment<IdType> appointment)
        {
            AddPrimaryKey(appointment);
            AddClientTimeSlotUniqueKey(appointment);
            AddMedicalPractitionerTimeSlotUniqueKey(appointment);
        }

        private void CheckPrimaryKey(IAppointment<IdType> appointment)
        {
            String primaryKey = getPrimaryKey(appointment);
            if (appointments.ContainsKey(primaryKey))
            {
                throw new TimeSlotConflictException();
            }
        }

        private void CheckClientTimeSlotUniqueKey(IAppointment<IdType> appointment)
        {
            String clientUniqueKey = getClientTimeSlotUniqueKey(appointment);
            if (clientAppointments.ContainsKey(clientUniqueKey))
            {
                throw new TimeSlotConflictException();
            }
        }

        private void CheckMedicalPractitionerTimeSlotUniqueKey(IAppointment<IdType> appointment)
        {
            String medicalPractitionerUniqueKey = getMedicalPractitionerTimeSlotUniqueKey(appointment);
            if (medicalPractitionerAppointments.ContainsKey(medicalPractitionerUniqueKey))
            {
                throw new TimeSlotConflictException();
            }
        }

        private void AddPrimaryKey(IAppointment<IdType> appointment)
        {
            String primaryKey = getPrimaryKey(appointment);
            if (!appointments.TryAdd(primaryKey, appointment))
            {
                throw new TimeSlotConflictException();
            }
        }

        private void AddClientTimeSlotUniqueKey(IAppointment<IdType> appointment)
        {
            String clientUniqueKey = getClientTimeSlotUniqueKey(appointment);
            if (!clientAppointments.TryAdd(clientUniqueKey, appointment))
            {
                throw new TimeSlotConflictException();
            }
        }

        private void AddMedicalPractitionerTimeSlotUniqueKey(IAppointment<IdType> appointment)
        {
            String medicalPractitionerUniqueKey = getMedicalPractitionerTimeSlotUniqueKey(appointment);
            if (!medicalPractitionerAppointments.TryAdd(medicalPractitionerUniqueKey, appointment))
            {
                throw new TimeSlotConflictException();
            }
        }

        private String getPrimaryKey(IAppointment<IdType> appointment)
        {
            String primaryKey = String.Format("{0},{1},{2}", appointment.ClientId, appointment.MedicalPractitionerId.ToString(), appointment.TimeSlot.Key());
            return primaryKey;
        }

        private String getClientTimeSlotUniqueKey(IAppointment<IdType> appointment)
        {
            String clientUniqueKey = String.Format("{0},{1}", appointment.ClientId, appointment.TimeSlot.Key());
            return clientUniqueKey;
        }

        private String getMedicalPractitionerTimeSlotUniqueKey(IAppointment<IdType> appointment)
        {
            String medicalPractitionerUniqueKey = String.Format("{0},{1}", appointment.MedicalPractitionerId.ToString(), appointment.TimeSlot.Key());
            return medicalPractitionerUniqueKey;
        }
    }
}