﻿using TimeTable.Models.Entity;
using TimeTable.Models.Repository;

namespace TimeTable.Services
{
    // Слой с бизнес логикой
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private const int week = 7;
        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }
  
        public async Task<List<Lesson>> GetAllLessons()
        {
            return await _lessonRepository.GetAll();
        }

        public async Task<Lesson> GetLessonById(Guid id)
        {
            return await _lessonRepository.GetById(id);
        }

        public async Task<Guid?> Add(Lesson lesson)
        {
            if (lesson.StartTime > lesson.EndTime) 
                return Guid.Empty;
           return await _lessonRepository.Add(lesson);
        }
        public async Task<Guid?> AddWithRepeats(Lesson lesson, List<DateTime> days, DateOnly startPeriod, DateOnly endPeriod)
        {
            if (startPeriod > endPeriod)
                return Guid.Empty;
            var startTime = startPeriod;
            while (startTime < endPeriod)
            {
                startTime =  await ForEach(lesson, days, startTime, endPeriod);
            }
            return lesson.Id;
        }
        private async Task<DateOnly> ForEach(Lesson lesson, List<DateTime> days, DateOnly startTime, DateOnly endPeriod)
        {
            foreach (var day in days)
            {
                var time = Offset(startTime, day);
                if (time > endPeriod)
                    continue;
                await AddWithOffset(lesson, time);
            }
            return startTime.AddDays(week);
        }
        private DateOnly Offset(DateOnly startTime, DateTime day)
        {
            var time = startTime;
            var dayBefore = time.AddDays((int)day.DayOfWeek - (int)time.DayOfWeek);
            var dayAfter = time.AddDays(week - ((int)time.DayOfWeek - (int)day.DayOfWeek));
            time = (double)time.DayOfWeek <= (double)day.DayOfWeek 
                ? dayBefore 
                : dayAfter;
            return time;
        }
        private async Task AddWithOffset(Lesson lesson, DateOnly time)
        {
            lesson.Date = time;
            lesson.Id = Guid.NewGuid();
            await _lessonRepository.Add(lesson);
        }
        public async Task<Guid> Delete(Guid id)
        {
            return await _lessonRepository.Delete(id);
        }
        public async Task<Guid> Update(Guid id, string subject, Guid userId, string className, Guid taskId, DateOnly date, TimeOnly startTime, TimeOnly endtime)
        {
            return await _lessonRepository.Update(id, subject, userId, className, taskId, date, startTime, endtime);
        }

        public async Task<List<Lesson>> GetUserSchedule(Guid id)
        {
            return await _lessonRepository.GetUserLessons(id);
        }
    }
}
