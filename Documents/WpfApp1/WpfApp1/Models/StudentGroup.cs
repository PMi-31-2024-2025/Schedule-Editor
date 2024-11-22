using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace UniversityScheduler.Models
{
    public class StudentGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public int FacultyId { get; set; }
        public int CourseId { get; set; }
        public int StudentCount { get; set; }
    }

}
