using AppointmentScheduler.Service;
using AppointmentScheduler.Tests.Service;
using AppointmentScheduler.Domain;
using AppointmentScheduler.Tests.Domain;
using System;
using System.Linq;
using Xunit;
using Xunit.Gherkin.Quick;

using IdType = System.String;
using AppointmentType = AppointmentScheduler.Domain.IAppointment<System.String>;
using AppointmentServiceType = AppointmentScheduler.Service.IAppointmentService<System.String>;
using AppointmentScheduler.Exception;

namespace AppointmentScheduler.Tests.StepDefintions
{
    [FeatureFile("./Features/ScheduleNewAppointment.feature")]
    public class AppointmentSchedulerSteps : Feature
    {

        AppointmentServiceType appointmentService;
        AppointmentType newAppointment;
        AppointmentType scheduledAppointment;
        System.Exception thrownException;

        [Given(@"an empty schedule")]
        public void GivenAnEmptySchedule()
        {
            appointmentService = CreateAppointmentService();
        }

        [Given(@"a schedule with the following appointments")]
        public void GivenAScheduleWithTheFollowingAppointments(Gherkin.Ast.DataTable dataTable)
        {
            GivenAnEmptySchedule();
            ScheduleAppointmentsFromTable(dataTable);
        }

        [And(@"an appointment for (.*) with (.*) on (.*) from (.*) till (.*) for a (.*) so that (.*)")]
        public void GivenAnAppointment(IdType clientId, IdType medicalPractitionerId, String date, String fromTime, String toTime, string description, string reason)
        {
            var timeSlot = new TimeSlot(date, fromTime, toTime);
            newAppointment = new Appointment<IdType>(clientId, medicalPractitionerId, timeSlot, description, reason);
        }

        [When(@"that appointment is scheduled")]
        public void WhenThatAppointmentIsScheduled()
        {
            thrownException = null;
            scheduledAppointment = null;
            try
            {
                scheduledAppointment = appointmentService.ScheduleAppointment(newAppointment);
                newAppointment = null;
            }
            catch (System.Exception exception)
            {
                thrownException = exception;
                scheduledAppointment = null;
                newAppointment = null;
            }

        }

        [Then(@"the schedule must contain (\d+) appointment/s only")]
        public void ThenThereMustOnlyBeNAppointments(int expectedAppointmentCount)
        {
            var actualAppointmentCount = appointmentService.GetAllAppointments(0, 1000).Count();
            Assert.Equal(expectedAppointmentCount, actualAppointmentCount);
        }

        [Then(@"the schedule must contain an appointment for (.*) with (.*) on (.*) from (.*) till (.*) for a (.*) so that (.*)")]
        public void ThenTheScheduleMustContainAnAppointment(String clientId, String medicalPractitionerId, String date, String fromTime, String toTime, String description, String reason)
        {
            var query = from findAppointment in appointmentService.GetAllAppointments(0, 1000)
                        where findAppointment.ClientId == clientId
                        && findAppointment.MedicalPractitionerId == medicalPractitionerId
                        && findAppointment.Description == description
                        && findAppointment.Reason == reason
                        select findAppointment;

            var actualAppointment = query.FirstOrDefault();

            Assert.NotNull(actualAppointment);
        }

        [And(@"a ConflictException must be thrown")]
        public void AConflictExceptionMustBeThrown()
        {
            Assert.IsType<TimeSlotConflictException>(thrownException);
        }

        [Then(@"the appointment must not be scheduled")]
        public void ScheduledAppointmentIsNUll()
        {
            Assert.Null(scheduledAppointment);
        }

        private AppointmentServiceType CreateAppointmentService()
        {
            IAppointmentRepository<IdType> appointmentRepository = new InMemoryAppointmentRepository<IdType>();
            ICalendar SimpleCalendar = new StandardWorkingCalendar();
            AppointmentServiceType appointmentService = new AppointmentService<IdType>(appointmentRepository, SimpleCalendar);
            return appointmentService;
        }

        private void ScheduleAppointmentsFromTable(Gherkin.Ast.DataTable dataTable)
        {
            foreach (var row in dataTable.Rows.Skip(1))
            {
                var givenAppointment = convertRowToAppointment(row);
                appointmentService.ScheduleAppointment(givenAppointment);
            }
        }

        private AppointmentType convertRowToAppointment(Gherkin.Ast.TableRow row)
        {
            var clientId = row.Cells.ElementAt(0).Value;
            var medicalPractitionerId = row.Cells.ElementAt(1).Value;
            var date = row.Cells.ElementAt(2).Value;
            var fromTime = row.Cells.ElementAt(3).Value;
            var toTime = row.Cells.ElementAt(4).Value;
            var description = row.Cells.ElementAt(5).Value;
            var reason = row.Cells.ElementAt(6).Value;
            var timeSlot = new TimeSlot(date, fromTime, toTime);
            var appointment = new Appointment<IdType>(clientId, medicalPractitionerId, timeSlot, description, reason);
            return appointment;
        }
    }
}