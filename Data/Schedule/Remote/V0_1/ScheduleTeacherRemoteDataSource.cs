namespace Mospolyhelper.Data.Schedule.Remote.V0_1
{
    using System;
    using System.Threading.Tasks;
    using Api.V0_1;
    using Converters.V0_1;
    using Domain.Schedule.Model.V0_1;
    using Microsoft.Extensions.Logging;

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

        public async Task<Schedule?> Get(string teacherId)
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
