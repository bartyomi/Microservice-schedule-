﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTable.Models;
using TimeTable.Data;
using TimeTable.Models.Entity;
using System.Data;
using TimeTable.Models.Repository;
using TimeTable.Services;

namespace TimeTable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {   
        private readonly ILessonService _lessonService;
        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet("{id:guid}")]
        public JsonResult GetById(Guid id)
        {
            var result = _lessonService.GetLessonById(id);

            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }

        [HttpGet]
        public JsonResult GetAll(Guid id)
        {
            var result = _lessonService.GetAllLessons();

            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }
        [HttpPost]
        public JsonResult Create(Lesson lesson)
        { 
           if (_lessonService.Add(lesson).Result != Guid.Empty)
            {
                return new JsonResult(Ok());
            }
            else
            {
                return new JsonResult(BadRequest());
            }
        }
        [HttpPost("CreateWithRepeat")]
        public JsonResult CreateWithRepeat([FromBody]Lesson lesson, [FromQuery]List<DateTime> days, DateTime startPeriod, DateTime endPeriod)
        {
            _lessonService.AddWithRepeat(lesson, days, startPeriod, endPeriod);
            return new JsonResult(Ok());
        }
        [HttpPut("{id:guid}")]
        public JsonResult Update(Guid id, Guid subjectId, Guid userId, string className, Guid taskId, DateTime startTime, DateTime endtime)
        {
            var result = _lessonService.Update(id, subjectId, userId, className, taskId, startTime, endtime);
            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }
        [HttpDelete("{id:guid}")]
        public JsonResult Delete(Guid id)
        {
            var result = _lessonService.Delete(id);
            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }
    }
}
