using System;
using System.Collections.Generic;
using AppointmentScheduler.Domain;

namespace AppointmentScheduler.Tests.Service
{
    public class StandardWorkingCalendar : ICalendar
    {

        private List<DayOfWeek> WorkDays = new List<DayOfWeek>{
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday};

        private int startOfWorkDayHour = 8;

        private int endOfWorkDayHour = 17;

        private int TimeSlotDurationInMinutes = 30;

        public TimeSlot FindNextTimeSlot(DateTime FromDateTime)
        {
            DateTime NextStartTime = FromDateTime;
            NextStartTime = CheckIfWorkingDay(NextStartTime);
            NextStartTime = CheckIfAfterEndOfDay(NextStartTime);
            NextStartTime = CheckIfBeforeStartOfDay(NextStartTime);
            NextStartTime = CheckIfStartOfTimeSlot(NextStartTime);
            return new TimeSlot(NextStartTime, TimeSlotDurationInMinutes);
        }

        private DateTime CheckIfWorkingDay(DateTime FromDateTime)
        {
            DateTime NewDateTime = FromDateTime;
            while (!WorkDays.Contains(NewDateTime.DayOfWeek))
            {
                NewDateTime = StartOfNextDay(NewDateTime);
            }
            return NewDateTime;
        }

        private DateTime CheckIfAfterEndOfDay(DateTime FromDateTime)
        {
            DateTime NewDateTime = FromDateTime;
            if (NewDateTime.Hour > endOfWorkDayHour)
            {
                NewDateTime = StartOfNextDay(NewDateTime);
            }
            return NewDateTime;
        }

        private DateTime CheckIfBeforeStartOfDay(DateTime FromDateTime)
        {
            DateTime NewDateTime = FromDateTime;
            if (NewDateTime.Hour < startOfWorkDayHour)
            {
                NewDateTime = new DateTime(NewDateTime.Year, NewDateTime.Month, NewDateTime.Day, startOfWorkDayHour, 0, 0);
            }
            return NewDateTime;
        }

        private DateTime CheckIfStartOfTimeSlot(DateTime FromDateTime)
        {
            DateTime StartOfTimeSlot = new DateTime(FromDateTime.Year, FromDateTime.Month, FromDateTime.Day)
                    .AddHours(startOfWorkDayHour);
            while (StartOfTimeSlot < FromDateTime)
            {
                StartOfTimeSlot = StartOfTimeSlot.AddMinutes(TimeSlotDurationInMinutes);
            }
            return StartOfTimeSlot;
        }

        private DateTime StartOfNextDay(DateTime DateTime)
        {
            return new DateTime(DateTime.Year, DateTime.Month, DateTime.Day).AddDays(1);
        }
    }
}