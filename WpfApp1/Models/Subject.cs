using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        
        public int FacultyId { get; set; } 
        public int CourseId { get; set; } // Відноситься до курсу
    }
}
