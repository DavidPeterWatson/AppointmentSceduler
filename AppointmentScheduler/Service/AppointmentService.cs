using AppointmentScheduler.Domain;
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
            ValidateInput(Appointment);
            IAppointment<IdType> SavedAppointment = SaveAppointment(Appointment);
            return SavedAppointment;
        }

        public TimeSlot FindNextTimeSlotForMedicalPractitioner(IdType MedicalPractitionerId, DateTime FromDateTime)
        {
            IAppointment<IdType> LastAppointment = AppointmentRepository.FindLastAppointmentForMedicalPractitioner(MedicalPractitionerId, FromDateTime);
            TimeSlot NextTimeSlot = Calendar.FindNextTimeSlot(LastAppointment.TimeSlot.ToDateTime);
            return NextTimeSlot;
        }

        private void ValidateInput(IAppointment<IdType> Appointment)
        {
            //ToDo check values
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

        public IEnumerable<IAppointment<IdType>> GetAppointmentsForMedicalPractitioner(IdType MedicalPractitionerId, int Skip, int Limit)
        {
            return AppointmentRepository.GetAppointmentsForMedicalPractitioner(MedicalPractitionerId, Skip, Limit);
        }

        public IEnumerable<IAppointment<IdType>> GetAppointmentsForClient(IdType ClientId, int Skip, int Limit)
        {
            return AppointmentRepository.GetAppointmentsForClient(ClientId, Skip, Limit);
        }
    }
}