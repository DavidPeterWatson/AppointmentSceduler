using System;

namespace AppointmentScheduler.Domain
{
    public struct TimeSlot
    {
        private DateTime date;
        private DateTime fromTime;
        private DateTime toTime;

        public DateTime Date { get => date; set => date = value; }
        public DateTime FromTime { get => fromTime; set => fromTime = value; }
        public DateTime ToTime { get => toTime; set => toTime = value; }


        public TimeSlot(DateTime date, DateTime fromTime, DateTime toTime)
        {
            this.date = date;
            this.fromTime = date.AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
            this.toTime = date.AddHours(toTime.Hour).AddMinutes(toTime.Minute);;
        }

        public TimeSlot(DateTime fromDateTime, int durationInMinutes)
        {
            this.date = new DateTime(fromDateTime.Year, fromDateTime.Month, fromDateTime.Day);
            this.fromTime = fromDateTime;
            this.toTime = fromTime.AddMinutes(durationInMinutes);
        }

        public DateTime FromDateTime
        {
            get
            {
                return new DateTime(Date.Year, Date.Month, Date.Day, FromTime.Hour, FromTime.Minute, 0);
            }
        }

        public DateTime ToDateTime
        {
            get
            {
                return new DateTime(Date.Year, Date.Month, Date.Day, ToTime.Hour, ToTime.Minute, 0);
            }
        }

        public override bool Equals(object obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((TimeSlot)obj);
        }

        public bool Equals(TimeSlot obj)
        {
            return obj.Date.Equals(Date)
                && obj.FromTime.Equals(this.FromTime)
                && obj.ToTime.Equals(this.ToTime);

        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}