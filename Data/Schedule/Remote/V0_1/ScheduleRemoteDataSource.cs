namespace Mospolyhelper.Data.Schedule.Remote.V0_1
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Api.V0_1;
    using Converters.V0_1;
    using Domain.Schedule.Model.V0_1;
    using Microsoft.Extensions.Logging;

    public class ScheduleRemoteDataSource
    {
        private readonly ILogger logger;
        private readonly ScheduleClient client;
        private readonly ScheduleRemoteConverter converter;

        public ScheduleRemoteDataSource(
            ILogger<ScheduleRemoteDataSource> logger,
            ScheduleClient client, 
            ScheduleRemoteConverter converter
            )
        {
            this.logger = logger;
            this.client = client;
            this.converter = converter;
        }

        public async Task<Schedule?> Get(string groupTitle, bool isSession)
        {
            this.logger.LogDebug($"Get groupTitle = {groupTitle}, isSession = {isSession}");
            try
            {
                var scheduleString = await client.GetSchedule(groupTitle, isSession);
                return converter.ParseSchedule(scheduleString);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Get");
                return null;
            }
        }

        public async Task<IList<Schedule>> GetAll(bool isSession)
        {
            this.logger.LogDebug($"GetAll isSession = {isSession}");
            try
            {
                var scheduleString = await client.GetAllSchedules(isSession);
                return converter.ParseSchedules(scheduleString);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetAll");
                return Array.Empty<Schedule>();
            }
        }
    }
}
