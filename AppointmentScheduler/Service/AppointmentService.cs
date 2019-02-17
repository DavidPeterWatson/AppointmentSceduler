using AppointmentScheduler.Domain;
using AppointmentScheduler.Exception;
using System;
using System.Collections.Generic;

namespace AppointmentScheduler.Service
{
    public class AppointmentService<IdType> : IAppointmentService<IdType>
    {

        private IAppointmentRepository<IdType> AppointmentRepository;

        private ICalendar Calendar;

        public AppointmentService(IAppointmentRepository<IdType> AppointmentRepository, ICalendar Calendar)
        {
            this.AppointmentRepository = AppointmentRepository;
            this.Calendar = Calendar;
        }

        public IAppointment<IdType> ScheduleAppointment(IAppointment<IdType> Appointment)
        {
            SaveAppointment(Appointment);
            return Appointment;
        }

        public TimeSlot FindNextTimeSlotForMedicalPractitioner(IdType MedicalPractitionerId, DateTime FromDateTime)
        {
            DateTime LastAppointmentTime = FindLastAppointmentTime(MedicalPractitionerId, FromDateTime);
            TimeSlot NextTimeSlot = Calendar.FindNextTimeSlot(LastAppointmentTime);
            return NextTimeSlot;
        }

        private DateTime FindLastAppointmentTime(IdType MedicalPractitionerId, DateTime FromDateTime)
        {
            // log.Info("WebApiApplication_BeginRequest");
            IAppointment<IdType> LastAppointment = AppointmentRepository.FindLastAppointmentForMedicalPractitioner(MedicalPractitionerId, FromDateTime);
            if (LastAppointment != null)
            {
                return LastAppointment.TimeSlot.ToDateTime;
            }
            else
            {
                return FromDateTime;
            }
        }

        private IAppointment<IdType> SaveAppointment(IAppointment<IdType> Appointment)
        {
            IAppointment<IdType> SavedAppointment = AppointmentRepository.SaveAppointment(Appointment);
            return SavedAppointment;
        }

        public IEnumerable<IAppointment<IdType>> GetAllAppointments(int Skip, int Limit)
        {
            return AppointmentRepository.GetAllAppointments(Skip, Limit);
        }

        public IEnumerable<IAppointment<IdType>> FindAppointmentsForMedicalPractitioner(IdType MedicalPractitionerId, int Skip, int Limit)
        {
            return AppointmentRepository.FindAppointmentsForMedicalPractitioner(MedicalPractitionerId, Skip, Limit);
        }

        public IEnumerable<IAppointment<IdType>> FindAppointmentsForClient(IdType ClientId, int Skip, int Limit)
        {
            return AppointmentRepository.FindAppointmentsForClient(ClientId, Skip, Limit);
        }
    }
}