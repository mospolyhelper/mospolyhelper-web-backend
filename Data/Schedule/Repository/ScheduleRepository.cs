using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mospolyhelper.Data.Schedule.Remote;
using Mospolyhelper.Domain.Schedule.Model;
using Mospolyhelper.Domain.Schedule.Repository;

namespace Mospolyhelper.Data.Schedule.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private ScheduleRemoteDataSource remoteDataSource;

        public ScheduleRepository(ScheduleRemoteDataSource remoteDataSource)
        {
            this.remoteDataSource = remoteDataSource;
        }

        public async Task<Domain.Schedule.Model.Schedule?> GetSchedule(string groupTitle)
        {
            return ScheduleExt.Combine(
                await remoteDataSource.Get(groupTitle, false),
                await remoteDataSource.Get(groupTitle, true)
                );
        }

        public async Task<IEnumerable<Domain.Schedule.Model.Schedule>> GetAllSchedules()
        {
            return (await remoteDataSource.GetAll(false))
                .Union(await remoteDataSource.GetAll(true));
        }
    }
}
