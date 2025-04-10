﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using TimeTable.Data;
using TimeTable.Models.Entity;

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

        public async Task<Guid> Add(Lesson lesson)
        {
            await _dbContext.Lessons.AddAsync(lesson);
            await _dbContext.SaveChangesAsync();
            return lesson.Id;   
        }
        
        public async Task<Guid> Delete(Guid id)
        {
            await _dbContext.Lessons
                 .Where(x => x.Id == id)
                 .ExecuteDeleteAsync();
            return id;
        }


        public async Task<Guid> Update(
            Guid id,
            Guid? subjectId = null,
            Guid? userId = null,
            string? className = null,
            Guid? taskId = null,
            DateTime? startTime = null,
            DateTime? endTime = null)
        {
            var query = _dbContext.Lessons.Where(x => x.Id == id);

            await query.ExecuteUpdateAsync(s => s
                .SetProperty(x => x.SubjectId, x => subjectId ?? x.SubjectId)
                .SetProperty(x => x.UserId, x => userId ?? x.UserId)
                .SetProperty(x => x.ClassName, x => className ?? x.ClassName)
                .SetProperty(x => x.TaskID, x => taskId ?? x.TaskID)
                .SetProperty(x => x.StartTime, x => startTime ?? x.StartTime)
                .SetProperty(x => x.EndTime, x => endTime ?? x.EndTime));

            return id;
        }

        public async Task<List<Lesson>> GetUserLessons(Guid userid)
        {
            return await _dbContext.Lessons
            .Where(x => x.UserId == userid)
            .AsNoTracking()
            .ToListAsync();
        }
    }
}
