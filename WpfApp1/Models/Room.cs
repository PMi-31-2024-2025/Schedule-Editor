using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityScheduler.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int Capacity { get; set; }

        public int FacultyId {get; set;}
        public bool HasProjector { get; set; }
        public bool HasComputer { get; set; }

        // Додатковий метод для відображення інформації
        public string Display => $"{RoomNumber} (Місць: {Capacity}, Проектор: {(HasProjector ? "Так" : "Ні")}, Комп'ютер: {(HasComputer ? "Так" : "Ні")})";
    }

}
