using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace UniversityScheduler.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int FacultyId { get; set; }
        public int CourseId { get; set; }
        public Teacher Teacher { get; set; }
        public StudentGroup Group { get; set; }
        public Room Room { get; set; }
        public string Pair { get; set; }
        public Subject Subject { get; set; }
        public LessonType LessonType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<ScheduleEntry> Entries { get; set; } = new List<ScheduleEntry>();

    }
}
