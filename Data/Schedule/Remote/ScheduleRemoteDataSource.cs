using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mospolyhelper.Data.Schedule.Api;
using Mospolyhelper.Data.Schedule.Converters;

namespace Mospolyhelper.Data.Schedule.Remote
{
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

        public async Task<Domain.Schedule.Model.Schedule?> Get(string groupTitle, bool isSession)
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

        public async Task<IList<Domain.Schedule.Model.Schedule>> GetAll(bool isSession)
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
                return Array.Empty<Domain.Schedule.Model.Schedule>();
            }
        }
    }
}
