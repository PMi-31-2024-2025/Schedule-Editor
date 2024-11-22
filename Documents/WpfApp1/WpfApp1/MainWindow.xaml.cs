using System.Text;
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
using WpfApp1.Views;
using System.Diagnostics;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

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
            Console.SetOut(new DebugTextWriter());

            InitializeComponent();

            // Ініціалізація списків
            _schedules = new List<Schedule>();
            _teachers = new List<Teacher>();
            _groups = new List<StudentGroup>();
            _rooms = new List<Room>();
            _faculties = new List<Faculty>();
            _courses = new List<Course>();
            _subjects = new List<Subject>();
            _lessonTypes = new List<LessonType>();

            SeedData();

            // Завантаження початкової сторінки
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
            // Створення базових даних
            var faculty1 = new Faculty { Id = 1, FacultyName = "Факультет Інформатики" };
            _faculties.Add(faculty1);

            var course1 = new Course { Id = 1, CourseName = "Перший курс", FacultyId = faculty1.Id };
            _courses.Add(course1);

            var subject1 = new Subject { Id = 1, SubjectName = "Алгоритми", CourseId = course1.Id };
            _subjects.Add(subject1);

            _lessonTypes.Add(new LessonType { Id = 1, TypeName = "Лекція" });

            _teachers.Add(new Teacher { Id = 1, Name = "Іваненко Іван", SubjectId = subject1.Id, LessonTypeId = 1 });
            _groups.Add(new StudentGroup { Id = 1, GroupName = "Інформатика Група-1", FacultyId = faculty1.Id, CourseId = course1.Id, StudentCount = 20 });
            _rooms.Add(new Room { Id = 1, RoomNumber = "101", Capacity = 30, HasProjector = true });

            // Додаткові дані, як у вашому коді
            var faculty2 = new Faculty { Id = 2, FacultyName = "Факультет Математики" };
            _faculties.Add(faculty2);

            var course2 = new Course { Id = 2, CourseName = "Другий курс", FacultyId = faculty2.Id };
            _courses.Add(course2);

            var subject2 = new Subject { Id = 2, SubjectName = "Математичний аналіз", CourseId = course2.Id };
            _subjects.Add(subject2);

            _lessonTypes.Add(new LessonType { Id = 2, TypeName = "Практична" });

            _teachers.Add(new Teacher { Id = 2, Name = "Петренко Петро", SubjectId = subject2.Id, LessonTypeId = 2 });
            _groups.Add(new StudentGroup { Id = 2, GroupName = "Математика Група-2", FacultyId = faculty2.Id, CourseId = course2.Id, StudentCount = 25 });
            _rooms.Add(new Room { Id = 2, RoomNumber = "202", Capacity = 25, HasProjector = false });

        }

    }

}