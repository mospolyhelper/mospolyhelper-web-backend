namespace Mospolyhelper.Domain.Schedule.Repository.V0_1
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model.V0_1;

    public interface IScheduleRepository
    {
        public Task<Schedule?> GetSchedule(string groupTitle);

        public Task<IEnumerable<Schedule>> GetAllSchedules();

        public Task<Schedule?> GetByTeacher(string teacherId);

        public Task<IEnumerable<Schedule>> GetAllByTeacher();
    }
}
