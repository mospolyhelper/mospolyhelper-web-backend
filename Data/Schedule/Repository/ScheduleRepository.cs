using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mospolyhelper.Data.Schedule.Local;
using Mospolyhelper.Data.Schedule.ModelDb;
using Mospolyhelper.Data.Schedule.Remote;
using Mospolyhelper.Domain.Schedule.Model;
using Mospolyhelper.Domain.Schedule.Repository;

namespace Mospolyhelper.Data.Schedule.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ILogger logger;
        private readonly ScheduleRemoteDataSource remoteDataSource;
        private readonly ScheduleLocalDataSource localDataSource;

        public ScheduleRepository(
            ILogger<ScheduleRepository> logger,
            ScheduleRemoteDataSource remoteDataSource,
            ScheduleLocalDataSource localDataSource
            )
        {
            this.logger = logger;
            this.remoteDataSource = remoteDataSource;
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

            using var dataBaseSource = new ScheduleDataBaseSource();

            var date = dataBaseSource.Preferences.Find("scheduleLastUpdate");
            bool needUpdate = true;
            if (date != null && DateTime.TryParse(date.Value, out var updateDate))
            {
                needUpdate = (DateTime.Now - updateDate).Days >= 1;
            }

            if (needUpdate)
            {
                var schedules = (await remoteDataSource.GetAll(false))
                    .Union(await remoteDataSource.GetAll(true));
                SaveSchedule(schedules, dataBaseSource);
                dataBaseSource.Preferences.Add(
                    new PreferenceDb
                    {
                        Key = "scheduleLastUpdate",
                        Value = DateTime.Now.ToString()
                    }
                    );
                try
                {
                    dataBaseSource.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            var lessons = dataBaseSource.Lessons!
                .Include(l => l.LessonAuditoriums)
                .ThenInclude(a => a.Auditorium)
                .Include(l => l.LessonTeachers)
                .ThenInclude(t => t.Teacher)
                .Include(l => l.LessonGroups)
                .ThenInclude(g => g.Group)
                .ToList();
            var dailySchedules = new IList<Lesson>[7];
            for (var i = 0; i < dailySchedules.Length; i++)
            {
                dailySchedules[i] = lessons
                    .Where(it => (int)it.Day == i)
                    .Select(it => new Lesson(
                        it.Order,
                        it.Title,
                        it.Type,
                        it.LessonTeachers
                        .Select(t => Teacher.FromFullName(t.Teacher.Name))
                        .ToList(),
                        it.LessonAuditoriums
                        .Select(a => new Auditorium(a.Auditorium.Title, a.Auditorium.Color))
                        .ToList(),
                        it.LessonGroups
                        .Select(g => new Group(g.Group.Title, g.Group.Evening))
                        .ToList(),
                        it.DateFrom,
                        it.DateTo
                        )).ToList();
            }
            return new []{ Domain.Schedule.Model.Schedule.From(dailySchedules) };
        }

        private void SaveSchedule(
            IEnumerable<Domain.Schedule.Model.Schedule> schedules, 
            ScheduleDataBaseSource dataBaseSource
            )
        {
            dataBaseSource.Database.ExecuteSqlInterpolatedAsync($"delete from Lessons").Wait();
            dataBaseSource.Database.ExecuteSqlInterpolatedAsync($"delete from Teachers").Wait();
            dataBaseSource.Database.ExecuteSqlInterpolatedAsync($"delete from Auditoriums").Wait();
            dataBaseSource.Database.ExecuteSqlInterpolatedAsync($"delete from Groups").Wait();

            foreach (var it in schedules)
            {
                for (var i = 0; i < it.DailySchedules.Count; i++)
                {
                    var lessons = it.DailySchedules[i];
                    foreach (var lesson in lessons)
                    {
                        var lessonDb = new LessonDb
                        {
                            Day = (DayOfWeek)i,
                            Order = lesson.Order,
                            Type = lesson.Type,
                            Title = lesson.Title,
                            DateFrom = lesson.DateFrom,
                            DateTo = lesson.DateTo
                        };
                        foreach (var teacher in lesson.Teachers)
                        {
                            lessonDb.LessonTeachers.Add(
                                new LessonTeacherDb
                                {
                                    Teacher = GetTeacherDb(dataBaseSource.Teachers, teacher),
                                    Lesson = lessonDb
                                }
                                );
                        }
                        foreach (var group in lesson.Groups)
                        {
                            lessonDb.LessonGroups.Add(
                                new LessonGroupDb
                                {
                                    Group = GetGroupDb(dataBaseSource.Groups, group),
                                    Lesson = lessonDb
                                }
                                );
                        }
                        foreach (var auditorium in lesson.Auditoriums)
                        {
                            lessonDb.LessonAuditoriums.Add(
                                new LessonAuditoriumDb
                                {
                                    Auditorium = GetAuditoriumDb(dataBaseSource.Auditoriums, auditorium),
                                    Lesson = lessonDb
                                }
                                );
                        }
                        dataBaseSource.Lessons.Add(lessonDb);
                    }
                }
            }
            dataBaseSource.SaveChanges();
        }

        private TeacherDb GetTeacherDb(DbSet<TeacherDb> teachers, Teacher teacher)
        {
            var teacherDb = teachers.Where(it => 
                it.Name.Equals(teacher.FullName)
            ).FirstOrDefault();
            if (teacherDb == null)
            {
                teacherDb = teachers.Add(
                    new TeacherDb
                    {
                        Name = teacher.FullName
                    }
                    ).Entity;
            }
            return teacherDb;
        }
        private GroupDb GetGroupDb(DbSet<GroupDb> groups, Group group)
        {
            var groupDb = groups.Find(group.Title);
            if (groupDb == null)
            {
                groupDb = groups.Add(
                    new GroupDb
                    {
                        Title = group.Title,
                        Evening = group.Evening
                    }
                    ).Entity;
            }
            return groupDb;
        }
        private AuditoriumDb GetAuditoriumDb(DbSet<AuditoriumDb> auditoriums, Auditorium auditorium)
        {
            var auditoriumDb = auditoriums.Find(auditorium.Title);
            if (auditoriumDb == null)
            {
                auditoriumDb = auditoriums.Add(
                    new AuditoriumDb
                    {
                        Title = auditorium.Title,
                        Color = auditorium.Color
                    }
                    ).Entity;
            }
            return auditoriumDb;
        }
    }
}
