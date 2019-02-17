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

        private ConcurrentDictionary<String, IAppointment<IdType>> Appointments = new ConcurrentDictionary<String, IAppointment<IdType>>();
        private ConcurrentDictionary<String, IAppointment<IdType>> ClientAppointments = new ConcurrentDictionary<String, IAppointment<IdType>>();
        private ConcurrentDictionary<String, IAppointment<IdType>> MedicalPractitionerAppointments = new ConcurrentDictionary<String, IAppointment<IdType>>();

        private Object UniqueKeyLock = new Object();

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

        public IEnumerable<IAppointment<IdType>> FindAppointmentsForClient(IdType ClientId, int Skip, int Limit)
        {
            var Query = from FindAppointment in Appointments
                        where FindAppointment.Value.ClientId.Equals(ClientId)
                        orderby FindAppointment.Value.TimeSlot.ToDateTime ascending
                        select FindAppointment.Value;

            return Query.Skip(Skip).Take(Limit);
        }

        public IEnumerable<IAppointment<IdType>> FindAppointmentsForMedicalPractitioner(IdType MedicalPractitionerId, int Skip, int Limit)
        {
            var Query = from FindAppointment in Appointments
                        where FindAppointment.Value.MedicalPractitionerId.Equals(MedicalPractitionerId)
                        orderby FindAppointment.Value.TimeSlot.ToDateTime ascending
                        select FindAppointment.Value;

            return Query.Skip(Skip).Take(Limit);
        }

        public IAppointment<IdType> SaveAppointment(IAppointment<IdType> Appointment)
        {
            CheckAllUniqueKeys(Appointment);
            AddAllUniqueKeys(Appointment);
            return Appointment;
        }

        private void CheckAllUniqueKeys(IAppointment<IdType> Appointment)
        {
            CheckPrimaryKey(Appointment);
            CheckClientTimeSlotUniqueKey(Appointment);
            CheckMedicalPractitionerTimeSlotUniqueKey(Appointment);
        }

        private void AddAllUniqueKeys(IAppointment<IdType> Appointment)
        {
            AddPrimaryKey(Appointment);
            AddClientTimeSlotUniqueKey(Appointment);
            AddMedicalPractitionerTimeSlotUniqueKey(Appointment);
        }

        private void CheckPrimaryKey(IAppointment<IdType> Appointment)
        {
            String PrimaryKey = getPrimaryKey(Appointment);
            if (Appointments.ContainsKey(PrimaryKey))
            {
                throw new TimeSlotConflictException();
            }
        }

        private void CheckClientTimeSlotUniqueKey(IAppointment<IdType> Appointment)
        {
            String ClientUniqueKey = getClientTimeSlotUniqueKey(Appointment);
            if (ClientAppointments.ContainsKey(ClientUniqueKey))
            {
                throw new TimeSlotConflictException();
            }
        }

        private void CheckMedicalPractitionerTimeSlotUniqueKey(IAppointment<IdType> Appointment)
        {
            String MedicalPractitionerUniqueKey = getMedicalPractitionerTimeSlotUniqueKey(Appointment);
            if (MedicalPractitionerAppointments.ContainsKey(MedicalPractitionerUniqueKey))
            {
                throw new TimeSlotConflictException();
            }
        }

        private void AddPrimaryKey(IAppointment<IdType> Appointment)
        {
            String PrimaryKey = getPrimaryKey(Appointment);
            if (!Appointments.TryAdd(PrimaryKey, Appointment))
            {
                throw new TimeSlotConflictException();
            }
        }

        private void AddClientTimeSlotUniqueKey(IAppointment<IdType> Appointment)
        {
            String ClientUniqueKey = getClientTimeSlotUniqueKey(Appointment);
            if (!ClientAppointments.TryAdd(ClientUniqueKey, Appointment))
            {
                throw new TimeSlotConflictException();
            }
        }

        private void AddMedicalPractitionerTimeSlotUniqueKey(IAppointment<IdType> Appointment)
        {
            String MedicalPractitionerUniqueKey = getMedicalPractitionerTimeSlotUniqueKey(Appointment);
            if (!MedicalPractitionerAppointments.TryAdd(MedicalPractitionerUniqueKey, Appointment))
            {
                throw new TimeSlotConflictException();
            }
        }

        private String getPrimaryKey(IAppointment<IdType> Appointment)
        {
            String primaryKey = String.Format("{0},{1},{2}", Appointment.ClientId, Appointment.MedicalPractitionerId.ToString(), Appointment.TimeSlot.Key());
            return primaryKey;
        }

        private String getClientTimeSlotUniqueKey(IAppointment<IdType> Appointment)
        {
            String clientUniqueKey = String.Format("{0},{1}", Appointment.ClientId, Appointment.TimeSlot.Key());
            return clientUniqueKey;
        }

        private String getMedicalPractitionerTimeSlotUniqueKey(IAppointment<IdType> Appointment)
        {
            String medicalPractitionerUniqueKey = String.Format("{0},{1}", Appointment.MedicalPractitionerId.ToString(), Appointment.TimeSlot.Key());
            return medicalPractitionerUniqueKey;
        }
    }
}