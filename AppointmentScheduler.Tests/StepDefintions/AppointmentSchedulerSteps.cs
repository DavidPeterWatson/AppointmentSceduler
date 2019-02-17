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
        public void GivenAnAppointment(IdType clientId, IdType MedicalPractitionerId, String Date, String FromTime, String ToTime, string Description, string Reason)
        {
            var TimeSlot = new TimeSlot(Date, FromTime, ToTime);
            newAppointment = new Appointment<IdType>(clientId, MedicalPractitionerId, TimeSlot, Description, Reason);
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
        public void ThenThereMustOnlyBeNAppointments(int ExpectedAppointmentCount)
        {
            var ActualAppointmentCount = appointmentService.GetAllAppointments(0, 1000).Count();
            Assert.Equal(ExpectedAppointmentCount, ActualAppointmentCount);
        }

        [Then(@"the schedule must contain an appointment for (.*) with (.*) on (.*) from (.*) till (.*) for a (.*) so that (.*)")]
        public void ThenTheScheduleMustContainAnAppointment(String ClientId, String MedicalPractitionerId, String Date, String FromTime, String ToTime, String Description, String Reason)
        {
            var Query = from FindAppointment in appointmentService.GetAllAppointments(0, 1000)
                        where FindAppointment.ClientId == ClientId
                        && FindAppointment.MedicalPractitionerId == MedicalPractitionerId
                        && FindAppointment.Description == Description
                        && FindAppointment.Reason == Reason
                        select FindAppointment;

            var ActualAppointment = Query.FirstOrDefault();

            Assert.NotNull(ActualAppointment);
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
            IAppointmentRepository<IdType> AppointmentRepository = new InMemoryAppointmentRepository<IdType>();
            ICalendar SimpleCalendar = new StandardWorkingCalendar();
            AppointmentServiceType appointmentService = new AppointmentService<IdType>(AppointmentRepository, SimpleCalendar);
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