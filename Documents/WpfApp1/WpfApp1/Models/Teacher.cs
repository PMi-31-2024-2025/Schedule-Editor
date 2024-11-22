using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityScheduler.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubjectId { get; set; } // Відноситься до предмету, який він викладає
        public int LessonTypeId { get; set; } // Відповідає типу заняття (лекція чи практика)
    }
}
