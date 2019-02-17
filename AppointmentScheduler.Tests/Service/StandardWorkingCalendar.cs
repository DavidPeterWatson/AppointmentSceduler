using System;
using System.Collections.Generic;
using AppointmentScheduler.Domain;

namespace AppointmentScheduler.Tests.Service
{
    public class StandardWorkingCalendar : ICalendar
    {

        private List<DayOfWeek> workDays = new List<DayOfWeek>{
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday};

        private int startOfWorkDayHour = 8;

        private int endOfWorkDayHour = 17;

        private int timeSlotDurationInMinutes = 30;

        public TimeSlot FindNextTimeSlot(DateTime fromDateTime)
        {
            DateTime nextStartTime = fromDateTime;
            nextStartTime = CheckIfWorkingDay(nextStartTime);
            nextStartTime = CheckIfAfterEndOfDay(nextStartTime);
            nextStartTime = CheckIfBeforeStartOfDay(nextStartTime);
            nextStartTime = CheckIfStartOfTimeSlot(nextStartTime);
            return new TimeSlot(nextStartTime, timeSlotDurationInMinutes);
        }

        private DateTime CheckIfWorkingDay(DateTime fromDateTime)
        {
            DateTime newDateTime = fromDateTime;
            while (!workDays.Contains(newDateTime.DayOfWeek))
            {
                newDateTime = StartOfNextDay(newDateTime);
            }
            return newDateTime;
        }

        private DateTime CheckIfAfterEndOfDay(DateTime fromDateTime)
        {
            DateTime NewDateTime = fromDateTime;
            if (NewDateTime.Hour > endOfWorkDayHour)
            {
                NewDateTime = StartOfNextDay(NewDateTime);
            }
            return NewDateTime;
        }

        private DateTime CheckIfBeforeStartOfDay(DateTime fromDateTime)
        {
            DateTime newDateTime = fromDateTime;
            if (newDateTime.Hour < startOfWorkDayHour)
            {
                newDateTime = new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day, startOfWorkDayHour, 0, 0);
            }
            return newDateTime;
        }

        private DateTime CheckIfStartOfTimeSlot(DateTime fromDateTime)
        {
            DateTime startOfTimeSlot = new DateTime(fromDateTime.Year, fromDateTime.Month, fromDateTime.Day)
                    .AddHours(startOfWorkDayHour);
            while (startOfTimeSlot < fromDateTime)
            {
                startOfTimeSlot = startOfTimeSlot.AddMinutes(timeSlotDurationInMinutes);
            }
            return startOfTimeSlot;
        }

        private DateTime StartOfNextDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day).AddDays(1);
        }
    }
}