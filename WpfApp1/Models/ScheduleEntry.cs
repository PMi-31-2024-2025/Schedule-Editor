using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityScheduler.Models;
using Npgsql;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public Subject SubjectObject {get; set;}
        public DateTime StartTime { get; set; } // Час початку заняття
        public DateTime EndTime { get; set; } // Час закінчення заняття
        public string Pair { get; set; } // Номер пари, якщо це має значення (1, 2, 3 тощо)
        public string DayOfWeek { get; set; } // Нове поле для дня тижня
        public string PairTime { get; set; } // Наприклад, "8:30 - 10:00"



        // Відображуваний текст для списку записів
        public string DisplayText => $"{DayOfWeek}: {Subject} - {LessonType}, {Teacher?.Name}, {Room?.RoomNumber}, {StartTime:HH:mm} - {EndTime:HH:mm}";

        public void SaveToDatabase() {
            string connectionString = "Host=localhost;Username=postgres;Password=12505;Database=ScheduleEditor";
            using (var connection = new NpgsqlConnection(connectionString))
                {
                    try{
                        connection.Open();
                        string query = @"
                            INSERT INTO schedule 
                            (student_group_id, lesson_time, lecturer_id, classroom_id, class_type, subject_id, weekday)
                            VALUES (@GroupId, @LessonTime, @LecturerId, @ClassroomId, @ClassType, @SubjectId, @Weekday)
                        ";

                        using (var command = new NpgsqlCommand(query, connection))
                        {

                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@GroupId", Group.Id);
                            command.Parameters.AddWithValue("@LessonTime", PairTime);
                            command.Parameters.AddWithValue("@LecturerId", Teacher.Id);
                            command.Parameters.AddWithValue("@ClassroomId", Room.Id);
                            command.Parameters.AddWithValue("@ClassType", LessonType);
                            command.Parameters.AddWithValue("@SubjectId", SubjectObject.Id);
                            command.Parameters.AddWithValue("@Weekday", DayOfWeek);

                            command.ExecuteNonQuery();

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
        }

        public void DeleteFromDatabase() {
            string connectionString = "Host=localhost;Username=postgres;Password=12505;Database=ScheduleEditor";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try{
                    connection.Open();
                    string query = @"
                        DELETE FROM schedule
                        WHERE
                            student_group_id = @GroupId AND
                            lesson_time = @LessonTime AND
                            lecturer_id = @LecturerId AND
                            classroom_id = @ClassroomId AND
                            class_type = @ClassType AND
                            subject_id = @SubjectId
                            AND weekday = @Weekday
                        ";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Weekday", DayOfWeek);
                        command.Parameters.AddWithValue("@SubjectId", SubjectObject.Id);
                        command.Parameters.AddWithValue("@ClassType", LessonType);
                        command.Parameters.AddWithValue("@ClassroomId", Room.Id);
                        command.Parameters.AddWithValue("@LecturerId", Teacher.Id);
                        command.Parameters.AddWithValue("@LessonTime", PairTime);
                        command.Parameters.AddWithValue("@GroupId", Group.Id);

                        MessageBox.Show($"Weekday: {DayOfWeek}, SubjectId: {SubjectObject.Id}, ClassType: {LessonType}, ClassroomId: {Room.Id}, LecturerId: {Teacher.Id}, LessonTime: {PairTime}, GroupId: {Group.Id}");

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

    }
}
