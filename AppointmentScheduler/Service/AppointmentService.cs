using AppointmentScheduler.Domain;
using AppointmentScheduler.Exception;
using System;
using System.Collections.Generic;

namespace AppointmentScheduler.Service
{
    public class AppointmentService<IdType> : IAppointmentService<IdType>
    {

        private IAppointmentRepository<IdType> appointmentRepository;

        private ICalendar calendar;

        public AppointmentService(IAppointmentRepository<IdType> appointmentRepository, ICalendar calendar)
        {
            this.appointmentRepository = appointmentRepository;
            this.calendar = calendar;
        }

        public IAppointment<IdType> ScheduleAppointment(IAppointment<IdType> appointment)
        {
            SaveAppointment(appointment);
            return appointment;
        }

        public TimeSlot FindNextTimeSlotForMedicalPractitioner(IdType medicalPractitionerId, DateTime fromDateTime)
        {
            DateTime lastAppointmentTime = FindLastAppointmentTime(medicalPractitionerId, fromDateTime);
            TimeSlot nextTimeSlot = calendar.FindNextTimeSlot(lastAppointmentTime);
            return nextTimeSlot;
        }

        private DateTime FindLastAppointmentTime(IdType medicalPractitionerId, DateTime fromDateTime)
        {
            IAppointment<IdType> lastAppointment = appointmentRepository.FindLastAppointmentForMedicalPractitioner(medicalPractitionerId, fromDateTime);
            if (lastAppointment != null)
            {
                return lastAppointment.TimeSlot.ToDateTime;
            }
            else
            {
                return fromDateTime;
            }
        }

        private IAppointment<IdType> SaveAppointment(IAppointment<IdType> appointment)
        {
            IAppointment<IdType> savedAppointment = appointmentRepository.SaveAppointment(appointment);
            return savedAppointment;
        }

        public IEnumerable<IAppointment<IdType>> GetAllAppointments(int skip, int limit)
        {
            return appointmentRepository.GetAllAppointments(skip, limit);
        }

        public IEnumerable<IAppointment<IdType>> FindAppointmentsForMedicalPractitioner(IdType medicalPractitionerId, int skip, int limit)
        {
            return appointmentRepository.FindAppointmentsForMedicalPractitioner(medicalPractitionerId, skip, limit);
        }

        public IEnumerable<IAppointment<IdType>> FindAppointmentsForClient(IdType ClientId, int skip, int limit)
        {
            return appointmentRepository.FindAppointmentsForClient(ClientId, skip, limit);
        }
    }
}