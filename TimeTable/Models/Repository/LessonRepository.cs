﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using TimeTable.Data;
using TimeTable.Models.Entity;
using TimeTable.Logging;

namespace TimeTable.Models.Repository
{
    //Слой для взаимодействия с бд
    public class LessonRepository : ILessonRepository
    {
        private readonly LessonDbContext _dbContext;
        public LessonRepository(LessonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Lesson>> GetAll()
        {
            return await _dbContext.Lessons
                .AsNoTracking()
                .OrderBy(l => l.StartTime)
                .ToListAsync();
        }

        public async Task<Lesson> GetById(Guid id)
        {
            return await _dbContext.Lessons
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Guid?> Add(Lesson lesson)
        {
            Guid? id = lesson.Id;
            try
            {
                await _dbContext.Lessons.AddAsync(lesson);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ConsoleLogger.Logger.LogError(ex.Message);
            }
            finally
            {
                id = null;
            }
            return id;
        }
        
        public async Task<Guid?> Delete(Guid id)
        {
            var query = _dbContext.Lessons.Where(x => x.Id == id);

            if (!query.Any())
            {
                ConsoleLogger.Logger.LogInformation($"Объект {id} для удаления не найден");
                return null;
            }

            await query.ExecuteDeleteAsync();
            return id;
        }

        public async Task<Guid?> Update(
            Guid id,
            string? subject = null,
            Guid? userId = null,
            string? className = null,
            Guid? taskId = null,
            DateOnly? date = null,
            TimeOnly? startTime = null,
            TimeOnly? endTime = null)
        {
            var query = _dbContext.Lessons.Where(x => x.Id == id);

            if (!query.Any())
            {
                ConsoleLogger.Logger.LogInformation($"Объект {id} для обновления не найден");
                return null;
            }

            await query.ExecuteUpdateAsync(s => s
                .SetProperty(x => x.Subject, x => subject ?? x.Subject)
                .SetProperty(x => x.UserId, x => userId ?? x.UserId)
                .SetProperty(x => x.ClassName, x => className ?? x.ClassName)
                .SetProperty(x => x.TaskID, x => taskId ?? x.TaskID)
                .SetProperty(x => x.Date, x => date ?? x.Date)
                .SetProperty(x => x.StartTime, x => startTime ?? x.StartTime)
                .SetProperty(x => x.EndTime, x => endTime ?? x.EndTime));

            return id;
        }

        public async Task<List<Lesson>> GetAllForPeriod(TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            return await _dbContext.Lessons
            .Where(l => l.StartTime >= startTime)
            .Where(l => l.EndTime <= endTime)
            .Where(l => l.Date >= startDate)
            .Where(l => l.Date <= endDate)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<Lesson>> GetUserLessons(Guid userid, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            return await _dbContext.Lessons
            .Where(x => x.UserId == userid)
            .Where(l => l.StartTime >= startTime)
            .Where(l => l.EndTime <= endTime)
            .Where(l => l.Date >= startDate)
            .Where(l => l.Date <= endDate)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<Lesson>> GetClassLessons(string className, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            return await _dbContext.Lessons
            .Where(x => x.ClassName == className)
            .Where(l => l.StartTime >= startTime)
            .Where(l => l.EndTime <= endTime)
            .Where(l => l.Date >= startDate)
            .Where(l => l.Date <= endDate)
            .AsNoTracking()
            .ToListAsync();
        }
    }
}
