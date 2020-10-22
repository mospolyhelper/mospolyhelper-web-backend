using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mospolyhelper.Domain.Schedule.Repository
{
    public interface IScheduleRepository
    {
        public Task<Model.Schedule?> GetSchedule(string groupTitle);

        public Task<IEnumerable<Model.Schedule>> GetAllSchedules();
    }
}
