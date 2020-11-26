using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Startup
{
    public static class ElectronicJournalPageNames
    {
        public static class PageNames
        {
            public const string AdministrationUsers = "Administration.Users";
            public const string AdministrationStudyGroups = "Administration.StudyGroups";
            public const string AdministrationAcademicSubjects = "Administarion.AcademicSubjects";

            public const string TeacherAcademicSubjectScores = "Teacher.AcademicSubjectScores";
            public const string TeacherHomeWorks = "Teacher.HomeWorks";

            public const string StudentAcademicSubjectScores = "Student.AcademicSubjectScores";
            public const string StudentHomeWorks = "Student.HomeWorks";
            public const string StudentRating = "Student.Rating";

            public const string CommonInfo = "Common.Info";
        }
        public static class DisplayPageNames
        {
            public const string AdministrationUsers = "Пользователи";
            public const string AdministrationStudyGroups = "Учебные группы";
            public const string AdministrationAcademicSubjects = "Учебные предметы";

            public const string TeacherAcademicSubjectScores = "Оценки";
            public const string TeacherHomeWorks = "Домашнее задание";

            public const string StudentAcademicSubjectScores = "Оценки";
            public const string StudentHomeWorks = "Домашнее задание";
            public const string StudentRating = "Рейтинг";

            public const string CommonInfo = "Информация";
            public const string CommonWelcome = "Добро пожаловать";
        }

    }
}
