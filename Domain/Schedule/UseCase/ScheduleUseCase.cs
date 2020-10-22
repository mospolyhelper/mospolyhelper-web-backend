using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mospolyhelper.Data.Schedule.Repository;
using Mospolyhelper.Domain.Schedule.Model;
using Mospolyhelper.Domain.Schedule.Repository;

namespace Mospolyhelper.Domain.Schedule.UseCase
{
    public class ScheduleUseCase
    {
        private IScheduleRepository scheduleRepository;

        public ScheduleUseCase(IScheduleRepository scheduleRepository)
        {
            this.scheduleRepository = scheduleRepository;
        }

        public Task<Model.Schedule?> GetSchedule(string id)
        {
            return scheduleRepository.GetSchedule(id);
        }

        public async Task<Model.Schedule?> GetSchedule(
            IEnumerable<string> groups, 
            IEnumerable<string> teachers, 
            IEnumerable<string> auditoriums, 
            IEnumerable<string> types, 
            IEnumerable<string> titles
            )
        {
            var schedules = await scheduleRepository.GetAllSchedules();
            return schedules.Filter(titles, types, auditoriums, teachers, groups);
        }

        public Task<IEnumerable<Model.Schedule>> GetAllSchedules()
        {
            return scheduleRepository.GetAllSchedules();
        }

        public async Task<IEnumerable<string>> GetGroupList()
        {
            var schedules = await scheduleRepository.GetAllSchedules();
            return schedules.SelectMany(it =>
                it.DailySchedules.SelectMany(lessons =>
                    lessons.SelectMany(lesson =>
                        lesson.Groups.Select(group => group.Title)
                    )
                )
            ).ToImmutableSortedSet();
        }

        public async Task<IEnumerable<string>> GetTeacherList()
        {
            var schedules = await scheduleRepository.GetAllSchedules();
            return schedules.SelectMany(it =>
                it.DailySchedules.SelectMany(lessons =>
                    lessons.SelectMany(lesson =>
                        lesson.Teachers.Select(teacher => teacher.FullName)
                    )
                )
            ).ToImmutableSortedSet();
        }

        public async Task<IEnumerable<string>> GetAuditoriumList()
        {
            var schedules = await scheduleRepository.GetAllSchedules();
            return schedules.SelectMany(it =>
                it.DailySchedules.SelectMany(lessons =>
                    lessons.SelectMany(lesson =>
                        lesson.Auditoriums.Select(auditorium => auditorium.Title)
                    )
                )
            ).ToImmutableSortedSet();
        }

        public async Task<IEnumerable<string>> GetTypeList()
        {
            var schedules = await scheduleRepository.GetAllSchedules();
            return schedules.SelectMany(it =>
                it.DailySchedules.SelectMany(lessons =>
                    lessons.Select(lesson => lesson.Type)
                )
            ).ToImmutableSortedSet();
        }

        public async Task<IEnumerable<string>> GetTitleList()
        {
            var schedules = await scheduleRepository.GetAllSchedules();
            return schedules.SelectMany(it =>
                it.DailySchedules.SelectMany(lessons =>
                    lessons.Select(lesson => lesson.Title)
                )
            ).ToImmutableSortedSet();
        }
    }
}
