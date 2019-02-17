using System;

namespace AppointmentScheduler.Domain
{
    public interface ICalendar
    {
        TimeSlot FindNextTimeSlot(DateTime FromDateTime);

    }
}