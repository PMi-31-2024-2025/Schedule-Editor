using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UniversityScheduler.Models;
using WpfApp1.Models;
using System.Windows.Media;
using System.Windows.Documents;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

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
                                ? $"{entry.Subject} ({entry.LessonType})\n{entry.Teacher.Name}\n{entry.Room.RoomNumber}"
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


        private void ExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PDF файли (*.pdf)|*.pdf",
                Title = "Збережіть розклад"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                // Створення документа
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));
                pdfDoc.Open();
                
                // Шрифти arial

                BaseFont baseFont = BaseFont.CreateFont(@"D:\Coding\Projects\Schedule-Editor\WpfApp1\ARIAL.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

                // Заголовок
                iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("Розклад занять", font);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20;
                pdfDoc.Add(title);

                // Таблиця
                PdfPTable table = new PdfPTable(ScheduleGrid.ColumnDefinitions.Count);
                table.WidthPercentage = 100;

                // Заголовки колонок
                foreach (var child in ScheduleGrid.Children)
                {
                    if (child is TextBlock textBlock && Grid.GetRow(textBlock) == 0)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(textBlock.Text, font))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BackgroundColor = BaseColor.LIGHT_GRAY,
                            Padding = 5
                        };
                        table.AddCell(cell);
                    }
                }

                // Додати рядки з даними
                for (int row = 1; row < ScheduleGrid.RowDefinitions.Count; row++)
                {
                    for (int col = 0; col < ScheduleGrid.ColumnDefinitions.Count; col++)
                    {
                        var cellContent = ScheduleGrid.Children
                            .Cast<UIElement>()
                            .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);

                        if (cellContent is TextBlock textBlock)
                        {
                            string[] lines = textBlock.Text.Split('\n');
                            PdfPCell cell = new PdfPCell(new Phrase(string.Join("\n", lines), font))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 5
                            };
                            table.AddCell(cell);
                        }
                        else
                        {
                            table.AddCell(new PdfPCell(new Phrase("", font)));
                        }
                    }
                }

                // Add the table to the document
                pdfDoc.Add(table);
                pdfDoc.Close();
            }
        }
    }
}
