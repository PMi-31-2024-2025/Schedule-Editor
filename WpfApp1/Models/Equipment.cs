using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityScheduler.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Type { get; set; } // Наприклад, проектор, комп'ютер, дошка тощо
        public string Name { get; set; } // Конкретна назва або модель обладнання
        public bool IsAvailable { get; set; } // Позначає, чи доступне обладнання
    }
}
