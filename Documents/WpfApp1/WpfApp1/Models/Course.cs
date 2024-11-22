using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int FacultyId { get; set; } // Відноситься до факультету
    }
}
