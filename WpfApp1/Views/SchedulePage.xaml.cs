using System;
using System.Collections.Generic;
using System.Linq;
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
using UniversityScheduler.Models;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        private List<Schedule> _schedules;
        private List<Teacher> _teachers;
        private List<StudentGroup> _groups;
        private List<Room> _rooms;
        private List<Faculty> _faculties;
        private List<Course> _courses;
        private List<Subject> _subjects;
        private List<LessonType> _lessonTypes;
        private Schedule _currentSchedule;
        private Schedule _fullSchedule;


        public SchedulePage(List<Schedule> schedules, List<Teacher> teachers, List<StudentGroup> groups, List<Room> rooms, List<Faculty> faculties, List<Course> courses, List<Subject> subjects, List<LessonType> lessonTypes, Schedule schedule)
        {
            InitializeComponent();
            _schedules = schedules;
            _teachers = teachers;
            _groups = groups;
            _rooms = rooms;
            _faculties = faculties;
            _courses = courses;
            _subjects = subjects;
            _lessonTypes = lessonTypes;
            _currentSchedule = schedule ?? new Schedule();
            _fullSchedule = new Schedule();

            FacultyComboBox.ItemsSource = _faculties;
            LessonTypeComboBox.ItemsSource = _lessonTypes;
            LessonTypeComboBox.DisplayMemberPath = "TypeName";

            RoomComboBox.ItemsSource = _rooms;
            RoomComboBox.DisplayMemberPath = "RoomNumber";
            PairComboBox.Items.Clear();
            PairComboBox.ItemsSource = new List<string>
            {
                "8:30 - 10:00",
                "10:15 - 11:45",
                "12:00 - 13:30",
                "13:45 - 15:15",
                "15:30 - 17:00",
                "17:15 - 18:45"
            };


            UpdateScheduleList();
        }



        private void LoadOrCreateSchedule()
        {
            if (FacultyComboBox.SelectedItem is Faculty selectedFaculty && CourseComboBox.SelectedItem is Course selectedCourse)
            {
                _currentSchedule = _schedules.FirstOrDefault(s => s.FacultyId == selectedFaculty.Id && s.CourseId == selectedCourse.Id);

                if (_currentSchedule == null)
                {
                    _currentSchedule = new Schedule { FacultyId = selectedFaculty.Id, CourseId = selectedCourse.Id };
                    _schedules.Add(_currentSchedule);
                }

                ScheduleListBox.ItemsSource = _currentSchedule.Entries;
            }
        }
        private void OnFacultySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FacultyComboBox.SelectedItem is Faculty selectedFaculty)
            {
                CourseComboBox.ItemsSource = _courses;
                RoomComboBox.ItemsSource = _rooms.Where(r => r.FacultyId == selectedFaculty.Id).ToList();
                RoomComboBox.DisplayMemberPath = "Display";
                RoomComboBox.SelectedItem = null;
                CourseComboBox.SelectedItem = null;
                SubjectComboBox.ItemsSource = null;
                GroupComboBox.ItemsSource = null;
                TeacherComboBox.ItemsSource = null;

                _fullSchedule.ReadDataFromDatabase(selectedFaculty.Id, 1, false);
                _fullSchedule.ReadDataFromDatabase(selectedFaculty.Id, 2, false);
                _fullSchedule.ReadDataFromDatabase(selectedFaculty.Id, 3, false);
                _fullSchedule.ReadDataFromDatabase(selectedFaculty.Id, 4, false);
            }
        }

        private void OnCourseSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CourseComboBox.SelectedItem is Course selectedCourse)
            {
                if (FacultyComboBox.SelectedItem is Faculty selectedFaculty) {
                    SubjectComboBox.ItemsSource = _subjects.Where(s => s.CourseId == selectedCourse.Id && s.FacultyId == selectedFaculty.Id).ToList();
                    GroupComboBox.ItemsSource = _groups.Where(g => g.CourseId == selectedCourse.Id && g.FacultyId == selectedFaculty.Id)
                        .Select(g => new { g, Display = $"{g.GroupName} (Студентів: {g.StudentCount})" }).ToList();
                    GroupComboBox.DisplayMemberPath = "Display";
                    GroupComboBox.SelectedItem = null;
                    TeacherComboBox.ItemsSource = null;

                    _currentSchedule.ReadDataFromDatabase(selectedFaculty.Id, selectedCourse.Id);
                    _schedules.Insert(0, _currentSchedule);
                    UpdateScheduleList();
                }
                
            }
        }

        private void OnSubjectSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectComboBox.SelectedItem is Subject selectedSubject && LessonTypeComboBox.SelectedItem is LessonType selectedLessonType)
            {
                TeacherComboBox.ItemsSource = _teachers
                    .Where(t => t.SubjectId == selectedSubject.Id && t.LessonTypeId == selectedLessonType.Id)
                    .ToList();
                TeacherComboBox.SelectedItem = null;
            }
        }

        private void OnLessonTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectComboBox.SelectedItem is Subject selectedSubject && LessonTypeComboBox.SelectedItem is LessonType selectedLessonType)
            {
                TeacherComboBox.ItemsSource = _teachers
                    .Where(t => t.SubjectId == selectedSubject.Id && t.LessonTypeId == selectedLessonType.Id)
                    .ToList();

                TeacherComboBox.DisplayMemberPath = "Name"; // Відображати ім'я викладача
                TeacherComboBox.SelectedItem = null; // Скидання вибору
            }
        }
        private void SaveSchedule_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Click worked!!!");

            if (FacultyComboBox.SelectedItem != null &&
                CourseComboBox.SelectedItem != null &&
                SubjectComboBox.SelectedItem != null &&
                LessonTypeComboBox.SelectedItem != null &&
                GroupComboBox.SelectedItem != null &&
                TeacherComboBox.SelectedItem != null &&
                RoomComboBox.SelectedItem != null &&
                PairComboBox.SelectedItem != null &&
                DayComboBox.SelectedItem != null) // Перевірка дня тижня
            {
                // Отримуємо вибрані дані
                var selectedGroup = (GroupComboBox.SelectedItem as dynamic)?.g;
                var selectedTeacher = (Teacher)TeacherComboBox.SelectedItem;
                var selectedRoom = (Room)RoomComboBox.SelectedItem;
                var selectedDay = DayComboBox.SelectedItem.ToString();
                var selectedPair = PairComboBox.SelectedItem.ToString();

                // Перевірка на конфлікт аудиторії
                bool isRoomConflict = _schedules
                    .SelectMany(s => s.Entries)
                    .Concat(_fullSchedule.Entries)
                    .Any(entry =>
                        entry.DayOfWeek == selectedDay &&
                        entry.PairTime == selectedPair &&
                        entry.Room.Id == selectedRoom.Id);

                if (isRoomConflict)
                {
                    MessageBox.Show($"Конфлікт: Аудиторія {selectedRoom.RoomNumber} вже зайнята на {selectedPair} у {selectedDay}.", "Конфлікт розкладу", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Зупиняємо виконання, якщо знайдено конфлікт
                }

                // Перевірка на конфлікт викладача
                bool isTeacherConflict = _schedules
                    .SelectMany(s => s.Entries)
                    .Concat(_fullSchedule.Entries)
                    .Any(entry =>
                        entry.DayOfWeek == selectedDay &&
                        entry.PairTime == selectedPair &&
                        entry.Teacher.Id == selectedTeacher.Id);

                if (isTeacherConflict)
                {
                    MessageBox.Show($"Конфлікт: Викладач {selectedTeacher.Name} вже зайнятий на {selectedPair} у {selectedDay}.", "Конфлікт розкладу", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Зупиняємо виконання, якщо знайдено конфлікт
                }

                // Додавання запису до розкладу
                var newEntry = new ScheduleEntry
                {
                    Subject = ((Subject)SubjectComboBox.SelectedItem).SubjectName,
                    SubjectObject = (Subject)SubjectComboBox.SelectedItem,
                    LessonType = ((LessonType)LessonTypeComboBox.SelectedItem).TypeName,
                    Group = selectedGroup,
                    Teacher = selectedTeacher,
                    Room = selectedRoom,
                    PairTime = selectedPair,
                    DayOfWeek = selectedDay,
                };

                if (!_schedules.Contains(_currentSchedule))
                {
                    _schedules.Add(_currentSchedule);
                }

                _currentSchedule.Entries.Add(newEntry);

                newEntry.SaveToDatabase();
                // Логування даних
                Console.WriteLine("Новий запис додано до розкладу:");
                Console.WriteLine($"  Предмет: {newEntry.Subject}");
                Console.WriteLine($"  Тип заняття: {newEntry.LessonType}");
                Console.WriteLine($"  Група: {newEntry.Group?.GroupName ?? "Невідома"}");
                Console.WriteLine($"  Викладач: {newEntry.Teacher?.Name ?? "Невідомий"}");
                Console.WriteLine($"  Аудиторія: {newEntry.Room?.RoomNumber ?? "Невідома"}");
                Console.WriteLine($"  Пара: {newEntry.PairTime}");
                Console.WriteLine($"  День: {newEntry.DayOfWeek}");

                Console.WriteLine($"Усього записів у розкладі: {_currentSchedule.Entries.Count}");

                UpdateScheduleList();
            }
            else
            {
                MessageBox.Show("Будь ласка, заповніть усі поля.");
            }
        }


        //private void SaveSchedule_Click(object sender, RoutedEventArgs e)
        //{
        //    Console.WriteLine("Click worked!!!");

        //    if (FacultyComboBox.SelectedItem != null &&
        //        CourseComboBox.SelectedItem != null &&
        //        SubjectComboBox.SelectedItem != null &&
        //        LessonTypeComboBox.SelectedItem != null &&
        //        GroupComboBox.SelectedItem != null &&
        //        TeacherComboBox.SelectedItem != null &&
        //        RoomComboBox.SelectedItem != null &&
        //        PairComboBox.SelectedItem != null &&
        //        DayComboBox.SelectedItem != null) // Перевірка дня тижня

        //    {
        //        // Отримуємо вибрану групу
        //        var selectedGroup = (GroupComboBox.SelectedItem as dynamic)?.g;
        //        var selectedTeacher = (Teacher)TeacherComboBox.SelectedItem;
        //        var selectedRoom = (Room)RoomComboBox.SelectedItem;

        //        var newEntry = new ScheduleEntry
        //        {
        //            Subject = ((Subject)SubjectComboBox.SelectedItem).SubjectName,
        //            LessonType = ((LessonType)LessonTypeComboBox.SelectedItem).TypeName,
        //            Group = selectedGroup,
        //            Teacher = selectedTeacher,
        //            Room = selectedRoom,
        //            PairTime = PairComboBox.SelectedItem.ToString(),
        //            DayOfWeek = DayComboBox.SelectedItem.ToString(), // Обов'язково передайте день


        //        };

        //        if (!_schedules.Contains(_currentSchedule))
        //        {
        //            _schedules.Add(_currentSchedule);
        //        }

        //        _currentSchedule.Entries.Add(newEntry);

        //        // Логування даних
        //        Console.WriteLine("Новий запис додано до розкладу:");
        //        Console.WriteLine($"  Предмет: {newEntry.Subject}");
        //        Console.WriteLine($"  Тип заняття: {newEntry.LessonType}");
        //        Console.WriteLine($"  Група: {newEntry.Group?.GroupName ?? "Невідома"}");
        //        Console.WriteLine($"  Викладач: {newEntry.Teacher?.Name ?? "Невідомий"}");
        //        Console.WriteLine($"  Аудиторія: {newEntry.Room?.RoomNumber ?? "Невідома"}");
        //        Console.WriteLine($"  Пара: {newEntry.PairTime}");
        //        Console.WriteLine($"  Day: {newEntry.DayOfWeek ?? "No day found"}");

        //        Console.WriteLine($"Усього записів у розкладі: {_currentSchedule.Entries.Count}");

        //        UpdateScheduleList();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Будь ласка, заповніть усі поля.");
        //    }
        //}





        private void DeleteSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleListBox.SelectedItem is ScheduleEntry selectedEntry)
            {
                selectedEntry.DeleteFromDatabase();
                _currentSchedule.Entries.Remove(selectedEntry);
                UpdateScheduleList();
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть запис для видалення.");
            }
        }

        private void UpdateScheduleList()
        {
            ScheduleListBox.ItemsSource = null;
            ScheduleListBox.ItemsSource = _currentSchedule.Entries;
        }
    }
}