using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityScheduler.Models;

namespace WpfApp1.Models
{
    public class ScheduleEntry
    {
        public int Id { get; set; } // Ідентифікатор запису
        public StudentGroup Group { get; set; } // Група, яка відвідує заняття
        public Teacher Teacher { get; set; } // Викладач
        public Room Room { get; set; } // Аудиторія
        public string LessonType { get; set; } // Тип заняття (Лекція, Практична тощо)
        public string Subject { get; set; } // Предмет
        public DateTime StartTime { get; set; } // Час початку заняття
        public DateTime EndTime { get; set; } // Час закінчення заняття
        public string Pair { get; set; } // Номер пари, якщо це має значення (1, 2, 3 тощо)
        public string DayOfWeek { get; set; } // Нове поле для дня тижня
        public string PairTime { get; set; } // Наприклад, "8:30 - 10:00"



        // Відображуваний текст для списку записів
        public string DisplayText => $"{DayOfWeek}: {Subject} - {LessonType}, {Teacher?.Name}, {Room?.RoomNumber}, {StartTime:HH:mm} - {EndTime:HH:mm}";

    }
}
