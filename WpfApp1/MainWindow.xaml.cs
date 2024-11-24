using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Windows;
using UniversityScheduler.Models;
using WpfApp1.Models;
using WpfApp1.Views;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private List<Schedule> _schedules;
        private List<Teacher> _teachers;
        private List<StudentGroup> _groups;
        private List<Room> _rooms;
        private List<Faculty> _faculties;
        private List<Course> _courses;
        private List<Subject> _subjects;
        private List<LessonType> _lessonTypes;

        public MainWindow()
        {
            Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

            InitializeComponent();

            // Initialize lists
            _schedules = new List<Schedule>();
            _teachers = new List<Teacher>();
            _groups = new List<StudentGroup>();
            _rooms = new List<Room>();
            _faculties = new List<Faculty>();
            _courses = new List<Course>();
            _subjects = new List<Subject>();
            _lessonTypes = new List<LessonType>();

            SeedData();

            // Load the initial page
            MainFrame.Navigate(new AllSchedulesPage(_schedules));
        }

        private void OnScheduleClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SchedulePage(_schedules, _teachers, _groups, _rooms, _faculties, _courses, _subjects, _lessonTypes, _schedules.FirstOrDefault()));
        }

        private void OnAllSchedulesClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AllSchedulesPage(_schedules));
        }

        private void SeedData()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=12505;Database=ScheduleEditor";

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve data from Faculty table
                    using (var command = new NpgsqlCommand("SELECT id, name FROM Faculty", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _faculties.Add(new Faculty
                            {
                                Id = reader.GetInt32(0),
                                FacultyName = reader.GetString(1)
                            });
                        }
                    }

                    _courses.Add(new Course{Id=1, CourseName="Перший курс"});
                    _courses.Add(new Course{Id=2, CourseName="Другий курс"});
                    _courses.Add(new Course{Id=3, CourseName="Третій курс"});
                    _courses.Add(new Course{Id=4, CourseName="Четвертий курс"});

                   using (var command = new NpgsqlCommand(@"
                        SELECT 
                            s.id AS SubjectId, 
                            s.name AS SubjectName, 
                            sp.faculty_id AS FacultyId, 
                            s.year AS CourseYear
                        FROM Subject s
                        INNER JOIN Specialization sp ON s.specialization_id = sp.id
                    ", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _subjects.Add(new Subject
                            {
                                Id = reader.GetInt32(0),
                                SubjectName = reader.GetString(1),
                                FacultyId = reader.GetInt32(2),
                                CourseId = reader.GetInt32(3)
                            });
                        }
                    }
                    
                    _lessonTypes.Add(new LessonType
                            {
                                Id = 0,
                                TypeName = "Лекція"
                            });
                    
                    _lessonTypes.Add(new LessonType
                            {
                                Id = 1,
                                TypeName = "Практична"
                            });


                   using (var command = new NpgsqlCommand(@"
                        SELECT 
                            l.id AS LecturerId,
                            l.name AS LecturerName,
                            ls.subject_id AS SubjectId
                        FROM Lecturer l
                        INNER JOIN LecturerSubject ls ON l.id = ls.lecturer_id
                    ", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _teachers.Add(new Teacher
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                SubjectId = reader.GetInt32(2),
                                LessonTypeId = 0
                            });
                            _teachers.Add(new Teacher
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                SubjectId = reader.GetInt32(2),
                                LessonTypeId = 1
                            });
                        }
                    }

                    // Retrieve data from StudentGroup table
                    using (var command = new NpgsqlCommand(@"
                    SELECT 
                        sg.id AS StudentGroupId,
                        sg.name AS GroupName,
                        s.faculty_id AS FacultyId,
                        sg.student_count AS StudentCount,
                        sg.year AS Year
                    FROM StudentGroup sg
                    INNER JOIN Specialization s ON sg.specialization_id = s.id;
                    ", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _groups.Add(new StudentGroup
                            {
                                Id = reader.GetInt32(0),
                                GroupName = reader.GetString(1),
                                FacultyId = reader.GetInt32(2),
                                StudentCount = reader.GetInt32(3),
                                CourseId = reader.GetInt32(4)
                            });
                        }
                    }

                    // Retrieve data from Classroom table
                    using (var classroomCommand = new NpgsqlCommand("SELECT id, room_number, seating_capacity, faculty_id FROM Classroom", connection))
                    using (var classroomReader = classroomCommand.ExecuteReader())
                    {
                        while (classroomReader.Read())
                        {
                            _rooms.Add(new Room
                            {
                                Id = classroomReader.GetInt32(0),
                                RoomNumber = classroomReader.GetString(1),
                                Capacity = classroomReader.GetInt32(2),
                                FacultyId = classroomReader.GetInt32(3),
                                HasProjector = false
                            });
                        }
                    }

                    // Now retrieve all equipment and check for projectors
                    var equipmentQuery = "SELECT classroom_id, name FROM Equipment";
                    using (var equipmentCommand = new NpgsqlCommand(equipmentQuery, connection))
                    using (var equipmentReader = equipmentCommand.ExecuteReader())
                    {
                        while (equipmentReader.Read())
                        {
                            int classroomId = equipmentReader.GetInt32(0);
                            string equipmentName = equipmentReader.GetString(1);

                            if (equipmentName.Equals("Проектор", StringComparison.OrdinalIgnoreCase))
                            {
                                var room = _rooms.FirstOrDefault(r => r.Id == classroomId);
                                if (room != null)
                                {
                                    room.HasProjector = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during data initialization: {ex.Message}");
            }
        }
    }
}
