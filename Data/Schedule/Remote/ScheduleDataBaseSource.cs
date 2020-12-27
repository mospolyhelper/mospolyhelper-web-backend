namespace Mospolyhelper.Data.Schedule.Remote
{
    using Microsoft.EntityFrameworkCore;
    using Mospolyhelper.Data.Schedule.ModelDb;

    public class ScheduleDataBaseSource : DbContext
    {
        public DbSet<LessonDb>? Lessons { get; set; }
        public DbSet<TeacherDb>? Teachers { get; set; }
        public DbSet<AuditoriumDb>? Auditoriums { get; set; }
        public DbSet<GroupDb>? Groups { get; set; }
        public DbSet<PreferenceDb>? Preferences { get; set; }

        public ScheduleDataBaseSource()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LessonTeacherDb>()
                .HasKey(lt => new { lt.LessonId, lt.TeacherId });
            modelBuilder.Entity<LessonTeacherDb>()
                .HasOne(lt => lt.Lesson)
                .WithMany(l => l!.LessonTeachers)
                .HasForeignKey(lt => lt.LessonId);
            modelBuilder.Entity<LessonTeacherDb>()
                .HasOne(lt => lt.Teacher)
                .WithMany(t => t!.LessonTeachers)
                .HasForeignKey(lt => lt.TeacherId);


            modelBuilder.Entity<LessonAuditoriumDb>()
                .HasKey(la => new { la.LessonId, la.AuditoriumKey });
            modelBuilder.Entity<LessonAuditoriumDb>()
                .HasOne(la => la.Lesson)
                .WithMany(l => l!.LessonAuditoriums)
                .HasForeignKey(la => la.LessonId);
            modelBuilder.Entity<LessonAuditoriumDb>()
                .HasOne(la => la.Auditorium)
                .WithMany(a => a!.LessonAuditoriums)
                .HasForeignKey(la => la.AuditoriumKey);


            modelBuilder.Entity<LessonGroupDb>()
                .HasKey(lg => new { lg.LessonId, lg.GroupKey });
            modelBuilder.Entity<LessonGroupDb>()
                .HasOne(lg => lg.Lesson)
                .WithMany(l => l!.LessonGroups)
                .HasForeignKey(lg => lg.LessonId);
            modelBuilder.Entity<LessonGroupDb>()
                .HasOne(lg => lg.Group)
                .WithMany(g => g!.LessonGroups)
                .HasForeignKey(lg => lg.GroupKey);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=schedule;Trusted_Connection=True;");
        }
    }
}
