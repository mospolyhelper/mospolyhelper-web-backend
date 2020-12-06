namespace Mospolyhelper.Data.Schedule.Local
{
    using System;
    using System.Collections.Generic;

    public class ScheduleLocalDataSource
    {
        private IEnumerable<Domain.Schedule.Model.Schedule> schedules = Array.Empty<Domain.Schedule.Model.Schedule>();
        private DateTime updateDate = DateTime.MinValue;

        public IEnumerable<Domain.Schedule.Model.Schedule> Schedules
        {
            set
            {
                schedules = value;
                updateDate = DateTime.Now;
            }
            get => this.schedules;
        }

        public bool IsRequiredUpdate => (DateTime.Now - updateDate).Days != 0;
    }
}
