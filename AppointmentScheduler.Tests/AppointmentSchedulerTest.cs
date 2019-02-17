using AppointmentScheduler.Domain;
using AppointmentScheduler.Service;
using AppointmentScheduler.Tests.Domain;
using AppointmentScheduler.Tests.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using IdType = System.String;
using AppointmentType = AppointmentScheduler.Domain.IAppointment<System.String>;
using AppointmentServiceType = AppointmentScheduler.Service.IAppointmentService<System.String>;


namespace AppointmentScheduler.Tests
{
    public class AppointmentSchedulerTest
    {

        [Fact]
        public void SaveAppointmentTest()
        {
            AppointmentServiceType appointmentService = CreateAppointmentService();
            AppointmentType newAppointment = CreateNewTestAppointment();
            appointmentService.ScheduleAppointment(newAppointment);
            int actual = appointmentService.GetAllAppointments(0, 100).Count();
            Assert.Equal(1, actual);
        }

        [Fact]
        public void FindNextTimeSlotAfterSchedulingAppointmentTest()
        {
            AppointmentServiceType appointmentService = CreateAppointmentService();
            AppointmentType newAppointment = CreateNewTestAppointment();
            appointmentService.ScheduleAppointment(CreateNewTestAppointment());
            TimeSlot actualTimeSlot = appointmentService.FindNextTimeSlotForMedicalPractitioner(newAppointment.MedicalPractitionerId, newAppointment.TimeSlot.FromDateTime);
            TimeSlot expectedTimeSlot = new TimeSlot(new DateTime(2019, 02, 19, 13, 00, 0), 30);
            Assert.Equal(expectedTimeSlot, actualTimeSlot);
        }

        private AppointmentServiceType CreateAppointmentService()
        {
            IAppointmentRepository<IdType> appointmentRepository = new InMemoryAppointmentRepository<IdType>();
            ICalendar standardWorkingCalendar = new StandardWorkingCalendar();
            AppointmentServiceType appointmentService = new AppointmentService<IdType>(appointmentRepository, standardWorkingCalendar);
            return appointmentService;
        }

        private AppointmentType CreateNewTestAppointment()
        {
            IdType clientId = "David";
            IdType medicalPractitionerId = "Dr Watson";
            TimeSlot timeSlot = new TimeSlot(new DateTime(2019, 02, 19, 12, 30, 0), 30);
            String description = "For Checkup";
            String reason = "";
            AppointmentType newAppointment = new Appointment<IdType>(clientId, medicalPractitionerId, timeSlot, description, reason);
            return newAppointment;
        }
    }
}
