namespace Mospolyhelper.Data.Schedule.Remote
{
    using Microsoft.Extensions.Logging;
    using Mospolyhelper.Data.Schedule.Api;
    using Mospolyhelper.Data.Schedule.Converters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ScheduleTeacherRemoteDataSource
    {
        private readonly ILogger logger;
        private readonly ScheduleClient client;
        private readonly ScheduleTeacherRemoteConverter converter;

        public ScheduleTeacherRemoteDataSource(
            ILogger<ScheduleTeacherRemoteDataSource> logger,
            ScheduleClient client,
            ScheduleTeacherRemoteConverter converter
            )
        {
            this.logger = logger;
            this.client = client;
            this.converter = converter;
        }

        public async Task<Domain.Schedule.Model.Schedule?> Get(string teacherId)
        {
            this.logger.LogDebug($"Get teacherId = {teacherId}");
            string scheduleString = "";
            try
            {
                scheduleString = await client.GetScheduleByTeacher(teacherId);
                return converter.Parse(scheduleString);
            }
            catch (Exception e)
            {
                this.logger.LogError(scheduleString + "\n" + e, "Get");
                return null;
            }
        }
    }
}
