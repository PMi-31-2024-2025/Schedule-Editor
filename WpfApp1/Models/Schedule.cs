using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp1.Models;


namespace UniversityScheduler.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int FacultyId { get; set; }
        public int CourseId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<ScheduleEntry> Entries { get; set; } = new List<ScheduleEntry>();

    }
}
