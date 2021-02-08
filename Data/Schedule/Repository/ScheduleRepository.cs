using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mospolyhelper.Data.Schedule.Local;
using Mospolyhelper.Data.Schedule.Remote;
using Mospolyhelper.Domain.Schedule.Model;
using Mospolyhelper.Domain.Schedule.Repository;

namespace Mospolyhelper.Data.Schedule.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ILogger logger;
        private readonly ScheduleRemoteDataSource remoteDataSource;
        private readonly ScheduleTeacherRemoteDataSource teacherRemoteDataSource;
        private readonly ScheduleLocalDataSource localDataSource;

        public ScheduleRepository(
            ILogger<ScheduleRepository> logger,
            ScheduleRemoteDataSource remoteDataSource,
            ScheduleTeacherRemoteDataSource techerRemoteDataSource,
            ScheduleLocalDataSource localDataSource
            )
        {
            this.logger = logger;
            this.remoteDataSource = remoteDataSource;
            this.teacherRemoteDataSource = techerRemoteDataSource;
            this.localDataSource = localDataSource;
        }

        public async Task<Domain.Schedule.Model.Schedule?> GetSchedule(string groupTitle)
        {
            this.logger.LogDebug($"GetSchedule groupTitle = {groupTitle}");
            return ScheduleExt.Combine(
                await remoteDataSource.Get(groupTitle, false),
                await remoteDataSource.Get(groupTitle, true)
                );
        }

        public async Task<IEnumerable<Domain.Schedule.Model.Schedule>> GetAllSchedules()
        {
            this.logger.LogDebug("GetAllSchedules");
            if (localDataSource.IsRequiredUpdate)
            {
                localDataSource.Schedules = (await remoteDataSource.GetAll(false))
                    .Union(await remoteDataSource.GetAll(true));
            }
            return localDataSource.Schedules;
        }

        public Task<Domain.Schedule.Model.Schedule?> GetByTeacher(string teacherId)
        {
            this.logger.LogDebug($"GetByTeacher teacherId = {teacherId}");
            return teacherRemoteDataSource.Get(teacherId);
        }

        public async Task<IEnumerable<Domain.Schedule.Model.Schedule>> GetAllByTeacher()
        {
            this.logger.LogDebug($"GetAllByTeacher");
            var resList = new List<Domain.Schedule.Model.Schedule?>();
            for (var i = 0; i < 3245; i++)
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
