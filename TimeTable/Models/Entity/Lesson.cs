﻿using System;

namespace TimeTable.Models.Entity
{
    public class Lesson
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public Guid UserId { get; set; }
        public string? ClassName { get; set; }
        public Guid TaskID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Subject? Subgect { get; set; }
    }
}
