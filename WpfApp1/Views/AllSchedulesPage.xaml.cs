//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows;
//using System.Windows.Controls;
//using UniversityScheduler.Models;
//using WpfApp1.Models;
//using System.Windows.Media;


//namespace WpfApp1.Views
//{
//    public partial class AllSchedulesPage : Page
//    {
//        private List<Schedule> _schedules;

//        public AllSchedulesPage(List<Schedule> schedules)
//        {
//            InitializeComponent();
//            _schedules = schedules;
//            LoadScheduleGrid();
//        }



//        private void LoadScheduleGrid()
//        {
//            Console.WriteLine("Кількість розкладів: " + _schedules.Count);
//            foreach (var schedule in _schedules)
//            {
//                Console.WriteLine($"Факультет ID: {schedule.FacultyId}, Курс ID: {schedule.CourseId}, Кількість записів: {schedule.Entries.Count}");
//                foreach (var entry in schedule.Entries)
//                {
//                    Console.WriteLine($"  Предмет: {entry.Subject}, День: {entry.DayOfWeek}, Пара: {entry.Pair}");
//                }
//            }

//            // Очищаємо старі дані
//            ScheduleGrid.Children.Clear();
//            ScheduleGrid.RowDefinitions.Clear();
//            ScheduleGrid.ColumnDefinitions.Clear();

//            // Додаємо колонки (дні тижня)
//            ScheduleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Перша колонка для пар
//            var days = new[] { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота" };
//            foreach (var day in days)
//            {
//                ScheduleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//            }

//            // Додаємо рядок заголовків
//            var headerRow = new RowDefinition { Height = GridLength.Auto };
//            ScheduleGrid.RowDefinitions.Add(headerRow);

//            var headerText = new TextBlock
//            {
//                Text = "Пари / Дні",
//                FontWeight = FontWeights.Bold,
//                TextAlignment = TextAlignment.Center,
//                Margin = new Thickness(5)
//            };
//            Grid.SetRow(headerText, 0);
//            Grid.SetColumn(headerText, 0);
//            ScheduleGrid.Children.Add(headerText);

//            for (int i = 0; i < days.Length; i++)
//            {
//                var dayHeader = new TextBlock
//                {
//                    Text = days[i],
//                    FontWeight = FontWeights.Bold,
//                    TextAlignment = TextAlignment.Center,
//                    Margin = new Thickness(5)
//                };
//                Grid.SetRow(dayHeader, 0);
//                Grid.SetColumn(dayHeader, i + 1);
//                ScheduleGrid.Children.Add(dayHeader);
//            }

//            // Додаємо пари (ряди)
//            var pairs = new[]
//            {
//            "8:30 - 10:00",
//            "10:15 - 11:45",
//            "12:00 - 13:30",
//            "13:45 - 15:15",
//            "15:30 - 17:00",
//            "17:15 - 18:45"
//        };

//            for (int i = 0; i < pairs.Length; i++)
//            {
//                ScheduleGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

//                var pairHeader = new TextBlock
//                {
//                    Text = pairs[i],
//                    FontWeight = FontWeights.Bold,
//                    TextAlignment = TextAlignment.Center,
//                    Margin = new Thickness(5)
//                };
//                Grid.SetRow(pairHeader, i + 1);
//                Grid.SetColumn(pairHeader, 0);
//                ScheduleGrid.Children.Add(pairHeader);
//            }

//            // Додаємо рядок для gridline
//            ScheduleGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
//            var emptyCell = new TextBlock
//            {
//                Text = string.Empty, // Пустий текст, щоб лінія залишалася видимою
//                Margin = new Thickness(0)
//            };
//            Grid.SetRow(emptyCell, pairs.Length + 1); // Рядок після останньої пари
//            Grid.SetColumn(emptyCell, 0); // У першій колонці
//            Grid.SetColumnSpan(emptyCell, days.Length + 1); // На всю ширину
//            ScheduleGrid.Children.Add(emptyCell);

//            // Заповнення розкладу
//            foreach (var schedule in _schedules)
//            {
//                foreach (var entry in schedule.Entries)
//                {
//                    Console.WriteLine($"Шукаємо день: {entry.DayOfWeek}, пару: {entry.Pair}");

//                    // Обробка часу пари
//                    string pairTime;
//                    if (int.TryParse(entry.Pair, out int pairIndex) && pairIndex > 0 && pairIndex <= pairs.Length)
//                    {
//                        // Якщо це індекс пари, отримуємо відповідний час
//                        pairTime = pairs[pairIndex - 1];
//                    }
//                    else
//                    {
//                        // Якщо значення вже є форматом часу, використовуйте його напряму
//                        pairTime = entry.Pair;
//                    }

//                    Console.WriteLine($"Час пари: {pairTime}");

//                    // Знаходимо індекси дня і пари
//                    int dayIndex = Array.IndexOf(days, entry.DayOfWeek);
//                    int pairIndexInGrid = Array.IndexOf(pairs, pairTime);

//                    Console.WriteLine($"Індекс дня: {dayIndex}, Індекс пари: {pairIndexInGrid}");

//                    if (dayIndex >= 0 && pairIndexInGrid >= 0)
//                    {
//                        var scheduleText = new TextBlock
//                        {
//                            Text = $"{entry.Subject}\n{entry.Group.GroupName}\n{entry.Teacher.Name}\n{entry.Room.RoomNumber}",
//                            TextAlignment = TextAlignment.Center,
//                            Margin = new Thickness(5)
//                        };
//                        Grid.SetRow(scheduleText, pairIndexInGrid + 1);
//                        Grid.SetColumn(scheduleText, dayIndex + 1);
//                        ScheduleGrid.Children.Add(scheduleText);
//                    }
//                    else
//                    {
//                        Console.WriteLine($"Не знайдено відповідності для дня {entry.DayOfWeek} або пари {pairTime}");
//                    }
//                }
//            }
//        }

//        private void ReloadSchedules_Click(object sender, RoutedEventArgs e)
//        {
//            LoadScheduleGrid();
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UniversityScheduler.Models;
using WpfApp1.Models;
using System.Windows.Media;
using System.Windows.Documents;

namespace WpfApp1.Views
{
    public partial class AllSchedulesPage : Page
    {
        private readonly List<Schedule> _schedules;

        public AllSchedulesPage(List<Schedule> schedules)
        {
            InitializeComponent();
            _schedules = schedules;
            LoadScheduleGrid();
        }

        private void LoadScheduleGrid()
        {
            Console.WriteLine("Кількість розкладів: " + _schedules.Count);

            // Очищаємо старі дані
            ScheduleGrid.Children.Clear();
            ScheduleGrid.RowDefinitions.Clear();
            ScheduleGrid.ColumnDefinitions.Clear();

            // Отримуємо унікальні назви груп
            var groupNames = _schedules
                .SelectMany(s => s.Entries.Select(e => e.Group.GroupName))
                .Distinct()
                .ToArray();

            // Додаємо колонки: для днів, годин та груп
            ScheduleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Перша колонка для днів
            ScheduleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Друга колонка для годин

            foreach (var group in groupNames)
            {
                ScheduleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Додаємо рядок для заголовків
            ScheduleGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Заголовок "Дні"
            var dayHeader = new TextBlock
            {
                Text = "Дні",
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5)
            };
            Grid.SetRow(dayHeader, 0);
            Grid.SetColumn(dayHeader, 0);
            ScheduleGrid.Children.Add(dayHeader);

            // Заголовок "Години"
            var timeHeader = new TextBlock
            {
                Text = "Години",
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5)
            };
            Grid.SetRow(timeHeader, 0);
            Grid.SetColumn(timeHeader, 1);
            ScheduleGrid.Children.Add(timeHeader);

            // Заголовки груп
            for (int i = 0; i < groupNames.Length; i++)
            {
                var groupHeader = new TextBlock
                {
                    Text = groupNames[i],
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(5)
                };
                Grid.SetRow(groupHeader, 0);
                Grid.SetColumn(groupHeader, i + 2);
                ScheduleGrid.Children.Add(groupHeader);
            }

          

            // Додаємо рядки для кожного дня та годин
            var days = new[] { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота" };
            var pairs = new[]
            {
                "8:30 - 10:00",
                "10:15 - 11:45",
                "12:00 - 13:30",
                "13:45 - 15:15",
                "15:30 - 17:00",
                "17:15 - 18:45"
            };

            int currentRow = 1; // Починаємо з другого рядка, перший зайнятий заголовками

            foreach (var day in days)
            {
                foreach (var pair in pairs)
                {
                    // Додаємо рядок
                    
               
                    ScheduleGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto, });

                    // Додаємо назву дня
                    if (pair == pairs[0]) // Назва дня додається тільки для першого часу
                    {
                        var dayText = new TextBlock
                        {
                            Text = day,
                            FontWeight = FontWeights.Bold,
                            TextAlignment = TextAlignment.Center,
                            Margin = new Thickness(1)
                        };
                        Grid.SetRow(dayText, currentRow);
                        Grid.SetColumn(dayText, 0);
                        ScheduleGrid.Children.Add(dayText);
                    }

                    // Додаємо час пари
                    var timeText = new TextBlock
                    {
                        Text = pair,
                        TextAlignment = TextAlignment.Center,
                        Margin = new Thickness(5)
                    };
                    Grid.SetRow(timeText, currentRow);
                    Grid.SetColumn(timeText, 1);
                    ScheduleGrid.Children.Add(timeText);


                    // Додаємо заняття для кожної групи
                    for (int col = 0; col < groupNames.Length; col++)
                    {
                        var groupName = groupNames[col];

                        // Виправляємо умову пошуку entry
                        var entry = _schedules
                            .SelectMany(s => s.Entries)
                            .FirstOrDefault(e =>
                                e.Group.GroupName == groupName &&
                                string.Equals(e.DayOfWeek, day, StringComparison.OrdinalIgnoreCase) &&
                                string.Equals(e.PairTime, pair, StringComparison.OrdinalIgnoreCase));

                   

                        var cellText = new TextBlock
                        {
                            Text = entry != null
                                ? $"{entry.Subject}\n{entry.Teacher.Name}\n{entry.Room.RoomNumber}"
                                : string.Empty,
                            TextAlignment = TextAlignment.Center,
                            Margin = new Thickness(5)
                        };
                        Grid.SetRow(cellText, currentRow);
                        Grid.SetColumn(cellText, col + 2);
                        ScheduleGrid.Children.Add(cellText);
                    }

                    currentRow++;
                }
                
            }
           
        }

        private void ReloadSchedules_Click(object sender, RoutedEventArgs e)
        {
            LoadScheduleGrid();
        }
    }
}
