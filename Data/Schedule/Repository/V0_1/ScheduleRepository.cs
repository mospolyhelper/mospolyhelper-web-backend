namespace Mospolyhelper.Data.Schedule.Repository.V0_1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Schedule.Model;
    using Domain.Schedule.Model.V0_1;
    using Domain.Schedule.Repository;
    using Domain.Schedule.Repository.V0_1;
    using Local.V0_1;
    using Microsoft.Extensions.Logging;
    using Remote.V0_1;

    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ILogger logger;
        private readonly ScheduleRemoteDataSource remoteDataSource;
        private readonly ScheduleTeacherRemoteDataSource teacherRemoteDataSource;
        private readonly ScheduleLocalDataSource localDataSource;

        public ScheduleRepository(
            ILogger<ScheduleRepository> logger,
            ScheduleRemoteDataSource remoteDataSource,
            ScheduleTeacherRemoteDataSource teacherRemoteDataSource,
            ScheduleLocalDataSource localDataSource
            )
        {
            this.logger = logger;
            this.remoteDataSource = remoteDataSource;
            this.teacherRemoteDataSource = teacherRemoteDataSource;
            this.localDataSource = localDataSource;
        }

        public async Task<Schedule?> GetSchedule(string groupTitle)
        {
            this.logger.LogDebug($"GetSchedule groupTitle = {groupTitle}");
            return ScheduleExt.Combine(
                await remoteDataSource.Get(groupTitle, false),
                await remoteDataSource.Get(groupTitle, true)
                );
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedules()
        {
            this.logger.LogDebug("GetAllSchedules");
            if (localDataSource.IsRequiredUpdate)
            {
                //localDataSource.Schedules = await GetAllByTeacher();
                localDataSource.Schedules = (await remoteDataSource.GetAll(false))
                    .Union(await remoteDataSource.GetAll(true));
            }
            return localDataSource.Schedules;
        }

        public Task<Schedule?> GetByTeacher(string teacherId)
        {
            this.logger.LogDebug($"GetByTeacher teacherId = {teacherId}");
            return teacherRemoteDataSource.Get(teacherId);
        }

        public async Task<IEnumerable<Schedule>> GetAllByTeacher()
        {
            this.logger.LogDebug("GetAllByTeacher");
            var resList = new List<Schedule?>();
            for (var i = 0; i < 3256; i++)
            {
                Console.WriteLine(i);
                var schedule = await teacherRemoteDataSource.Get(i.ToString());
                if (schedule != null)
                {
                    resList.Add(schedule);
                }
            }
            return resList;
        }
    }
}
