namespace Mospolyhelper.Data.Schedule.Local.V0_1
{
    using System;
    using System.Collections.Generic;
    using Domain.Schedule.Model.V0_1;
    using Microsoft.Extensions.Logging;

    public class ScheduleLocalDataSource
    {
        private readonly ILogger logger;

        public ScheduleLocalDataSource(ILogger<ScheduleLocalDataSource> logger)
        {
            this.logger = logger;
        }

        private IEnumerable<Schedule> schedules = Array.Empty<Schedule>();
        private DateTime updateDate = DateTime.MinValue;

        public IEnumerable<Schedule> Schedules
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
