namespace Mospolyhelper.Data.Schedule.Local
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;

    public class ScheduleLocalDataSource
    {
        private readonly ILogger logger;

        public ScheduleLocalDataSource(ILogger<ScheduleLocalDataSource> logger)
        {
            this.logger = logger;
        }

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
