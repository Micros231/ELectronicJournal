using ElectronicJournal.Core.Academic;
using ElectronicJournal.Core.Authorization.Roles;
using ElectronicJournal.Core.Authorization.Users;
using ElectronicJournal.Core.ManyToManyRelationships;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ElectronicJournal.EntityFrameworkCore.Data
{
    public class ElectronicJournalDbContext : IdentityDbContext<User, Role, long>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<AcademicSubject> AcademicSubjects { get; set; }
        public DbSet<AcademicSubjectScore> AcademicSubjectScores { get; set; }
        public DbSet<HomeWork> HomeWorks { get; set; }
        public DbSet<StudyGroup> StudyGroups { get; set; }
        public ElectronicJournalDbContext(DbContextOptions<ElectronicJournalDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasAlternateKey(key => key.UserId);

            modelBuilder.Entity<Teacher>()
                .HasAlternateKey(key => key.UserId);

            modelBuilder.Entity<StudyGroupAcademicSubject>()
                .HasKey(key => new { key.AcademicSubjectId, key.StudyGroupId });

            modelBuilder.Entity<StudyGroupAcademicSubject>()
                .HasOne(groupSubject => groupSubject.StudyGroup)
                .WithMany(group => group.StudyGroupAcademicSubjects)
                .HasForeignKey(key => key.StudyGroupId);

            modelBuilder.Entity<StudyGroupAcademicSubject>()
                .HasOne(groupSubject => groupSubject.AcademicSubject)
                .WithMany(subject => subject.StudyGroupAcademicSubjects)
                .HasForeignKey(key => key.AcademicSubjectId);

            modelBuilder.Entity<TeacherStudyGroup>()
                .HasKey(key => new { key.StudyGroupId, key.TeacherId});

            modelBuilder.Entity<TeacherStudyGroup>()
                .HasOne(teacherGroup => teacherGroup.Teacher)
                .WithMany(teacher => teacher.TeacherStudyGroups)
                .HasForeignKey(key => key.TeacherId);

            modelBuilder.Entity<TeacherStudyGroup>()
                .HasOne(teacheGroup => teacheGroup.StudyGroup)
                .WithMany(group => group.TeacherStudyGroups)
                .HasForeignKey(key => key.StudyGroupId);
            
        }
    }
}
