using AppointmentScheduler.Domain;
using AppointmentScheduler.Service;
using AppointmentScheduler.Tests.Domain;
using AppointmentScheduler.Tests.Service;
using System;
using Xunit;

using IdType = System.String;
using AppointmentType = AppointmentScheduler.Domain.IAppointment<System.String>;
using AppointmentServiceType = AppointmentScheduler.Service.IAppointmentService<System.String>;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentScheduler.AcceptanceTests
{
    public class AppointmentSchedulerTest
    {

        [Fact]
        public void SaveAppointmentTest()
        {
            AppointmentServiceType AppointmentService = CreateAppointmentService();
            AppointmentType NewAppointment = CreateNewTestAppointment();
            AppointmentService.ScheduleAppointment(NewAppointment);
            int Actual = AppointmentService.GetAllAppointments(0, 100).Count();
            Assert.Equal(1, Actual);
        }

        [Fact]
        public void FindNextTimeSlotAfterSchedulingAppointmentTest()
        {
            AppointmentServiceType AppointmentService = CreateAppointmentService();
            AppointmentType NewAppointment = CreateNewTestAppointment();
            AppointmentService.ScheduleAppointment(CreateNewTestAppointment());
            TimeSlot ActualTimeSlot = AppointmentService.FindNextTimeSlotForMedicalPractitioner(NewAppointment.MedicalPractitionerId, NewAppointment.TimeSlot.FromDateTime);
            TimeSlot ExpectedTimeSlot = new TimeSlot(new DateTime(2019, 02, 19, 13, 00, 0), 30);
            Assert.Equal(ExpectedTimeSlot, ActualTimeSlot);
        }

        private AppointmentServiceType CreateAppointmentService()
        {
            IAppointmentRepository<String> AppointmentRepository = new InMemoryAppointmentRepository<String>();
            ICalendar SimpleCalendar = new StandardWorkingCalendar();
            AppointmentServiceType AppointmentService = new AppointmentService<String>(AppointmentRepository, SimpleCalendar);
            return AppointmentService;
        }

        private AppointmentType CreateNewTestAppointment()
        {
            String ClientId = "David";
            String MedicalPractitionerId = "Dr Watson";
            TimeSlot TimeSlot = new TimeSlot(new DateTime(2019, 02, 19, 12, 30, 0), 30);
            String Description = "For Checkup";
            String Reason = "";
            AppointmentType NewAppointment = new Appointment<System.String>(ClientId, MedicalPractitionerId, TimeSlot, Description, Reason);
            return NewAppointment;
        }
    }
}
