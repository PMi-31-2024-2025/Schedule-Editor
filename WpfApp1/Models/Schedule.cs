using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp1.Models;
using Npgsql;
using Dapper;

namespace UniversityScheduler.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int FacultyId { get; set; }
        public int CourseId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<ScheduleEntry> Entries { get; set; } = new List<ScheduleEntry>();

        public Schedule(int facultyId, int year)
        {
            ReadDataFromDatabase(facultyId, year);
        }

        public Schedule()
        {
        }


        public void ReadDataFromDatabase(int facultyId, int year)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=12505;Database=ScheduleEditor";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                //clear entries
                Entries.Clear();

                connection.Open();

                FacultyId = facultyId;
                CourseId = year;

                string query = @"
                    SELECT s.id as Id,
                        sg.name as GroupName,
                        f.id as FacultyId,
                        sg.year as Year,
                        sg.student_count as StudentCount,
                        l.id as LecturerId,
                        l.name as LecturerName,
                        sub.id as SubjectId,
                        c.id as RoomId,
                        c.room_number as RoomNumber,
                        c.seating_capacity as Capacity,
                        s.class_type as ClassType,
                        sub.name as SubjectName,
                        s.weekday as Weekday,
                        s.lesson_time as LessonTime

                    FROM schedule s
                    JOIN studentgroup sg ON s.student_group_id = sg.id
                    JOIN specialization sp ON sg.specialization_id = sp.id
                    JOIN faculty f ON sp.faculty_id = f.id
                    JOIN subject sub ON s.subject_id = sub.id
                    JOIN lecturer l ON s.lecturer_id = l.id
                    JOIN classroom c ON s.classroom_id = c.id
                    WHERE sp.faculty_id = @FacultyId AND sg.year = @Year
                ";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FacultyId", FacultyId);
                    command.Parameters.AddWithValue("@Year", CourseId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entry = new ScheduleEntry
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Group = new StudentGroup
                                {
                                    GroupName = reader.GetString(reader.GetOrdinal("GroupName")),
                                    FacultyId = reader.GetInt32(reader.GetOrdinal("FacultyId")),
                                    CourseId = reader.GetInt32(reader.GetOrdinal("Year")),
                                    StudentCount = reader.GetInt32(reader.GetOrdinal("StudentCount"))
                                },
                                Teacher = new Teacher
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("LecturerId")),
                                    Name = reader.GetString(reader.GetOrdinal("LecturerName")),
                                    SubjectId = reader.GetInt32(reader.GetOrdinal("SubjectId"))
                                },
                                SubjectObject = new Subject
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("SubjectId")),
                                    SubjectName = reader.GetString(reader.GetOrdinal("SubjectName")),
                                    FacultyId = reader.GetInt32(reader.GetOrdinal("FacultyId")),
                                    CourseId = reader.GetInt32(reader.GetOrdinal("Year"))
                                },
                                Room = new Room
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                    RoomNumber = reader.GetString(reader.GetOrdinal("RoomNumber")),
                                    Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                                    FacultyId = reader.GetInt32(reader.GetOrdinal("FacultyId"))
                                },
                                LessonType = reader.GetString(reader.GetOrdinal("ClassType")),
                                Subject = reader.GetString(reader.GetOrdinal("SubjectName")),
                                DayOfWeek = reader.GetString(reader.GetOrdinal("Weekday")),
                                PairTime = reader.GetString(reader.GetOrdinal("LessonTime"))
                            };

                            Entries.Add(entry);
                        }
                    }
                }
            }

            
        }
    }
}
