﻿using System;
using TimeTable.Models.Entity;

namespace TimeTable.Services
{
    public interface ILessonService
    {
        Task<List<Lesson>> GetAllLessons();
        Task<Lesson> GetLessonById(Guid id);
        Task<Guid?> Add(Lesson lesson);
        Task<Guid?> AddWithRepeats(Lesson lesson, List<DateTime> days, DateOnly startPeriod, DateOnly endPeriod);
        Task<Guid> Delete(Guid id);
        Task<Guid> Update(Guid id, String subject, Guid userId, string className, Guid taskId, DateOnly date, TimeOnly startTime, TimeOnly endtime);
        Task<List<Lesson>> GetUserSchedule(Guid id);
    }
}
