using Repositories;
using Services;
using Services.Models;
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
using System.Windows.Shapes;
using TodoList;

namespace GUI
{
    /// <summary>
    /// Interaction logic for CalendarWindow.xaml
    /// </summary>
    public partial class CalendarWindow : Window
    {
        private readonly Services.TodoService _todoService = new();
        public User User { get; set; }
        public DateOnly Date { get; set; }
        private TodoService _service = new TodoService();

        public CalendarWindow()
        {
            InitializeComponent();
        }

        private void CalendarWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }


        private void LoadData()
        {
            Date = DateOnly.FromDateTime(DateTime.Now);

            AcccountNameLabel.Content = User.FullName;
            // Update the DateLabel to show the date range from Monday to Sunday

            var timeIntervals = GenerateTimeIntervals();

            // Calculate the start and end dates of the current week
            var startOfWeek = Date.AddDays(-(int)Date.DayOfWeek + (int)DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(6);

            DateLabel.Content = $"{startOfWeek.ToString("D")} - {endOfWeek.ToString("D")}";
            // Retrieve tasks for the entire week
            var todos = _service.GetAllTasks()
                .Where(t => t.UserId == User.UserId && t.Time.Date >= startOfWeek.ToDateTime(TimeOnly.MinValue) && t.Time.Date <= endOfWeek.ToDateTime(TimeOnly.MinValue))
                .ToList();

            foreach (var todo in todos)
            {
                var startHour = todo.Time.Hour;
                var duration = 1; // assuming each task lasts 1 hour

                for (int i = 0; i < duration; i++)
                {
                    var interval = timeIntervals.FirstOrDefault(t => t.Time.Hour == (startHour + i) % 24);
                    if (interval != null)
                    {

                        switch (todo.Time.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                interval.Mon = todo.Title;
                                break;
                            case DayOfWeek.Tuesday:
                                interval.Tue = todo.Title;
                                break;
                            case DayOfWeek.Wednesday:
                                interval.Wed = todo.Title;
                                break;
                            case DayOfWeek.Thursday:
                                interval.Thu = todo.Title;
                                break;
                            case DayOfWeek.Friday:
                                interval.Fri = todo.Title;
                                break;
                            case DayOfWeek.Saturday:
                                interval.Sat = todo.Title;
                                break;
                            case DayOfWeek.Sunday:
                                interval.Sun = todo.Title;
                                break;
                        }

                    }
                }
            }

            TodoListView.ItemsSource = timeIntervals;
        }



        private List<TimeInterval> GenerateTimeIntervals()
        {
            var intervals = new List<TimeInterval>();
            for (int hour = 0; hour < 24; hour++)
            {
                intervals.Add(new TimeInterval { Time = DateTime.Today.AddHours(hour) });
            }
            return intervals;
        }

        private void ViewTodayButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.User = User;
            mainWindow.ShowDialog();
            LoadData();
        }

        private void CalendarPick_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CalendarPick.SelectedDate.HasValue)
            {
                Date = DateOnly.FromDateTime(CalendarPick.SelectedDate.Value);
                LoadDataForSelectedWeek();
            }
        }

        private void LoadDataForSelectedWeek()
        {
            // Calculate the start and end dates of the selected week
            var startOfWeek = Date.AddDays(-(int)Date.DayOfWeek + (int)DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(6);

            // Update the DateLabel to show the date range from Monday to Sunday
            DateLabel.Content = $"{startOfWeek.ToString("D")} - {endOfWeek.ToString("D")}";

            var timeIntervals = GenerateTimeIntervals();

            // Retrieve tasks for the entire week
            var todos = _service.GetAllTasks()
                .Where(t => t.UserId == User.UserId && t.Time.Date >= startOfWeek.ToDateTime(TimeOnly.MinValue) && t.Time.Date <= endOfWeek.ToDateTime(TimeOnly.MinValue))
                .ToList();

            foreach (var todo in todos)
            {
                var startHour = todo.Time.Hour;
                var duration = 1; // assuming each task lasts 1 hour

                for (int i = 0; i < duration; i++)
                {
                    var interval = timeIntervals.FirstOrDefault(t => t.Time.Hour == (startHour + i) % 24);
                    if (interval != null)
                    {
                        switch (todo.Time.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                interval.Mon = todo.Title;
                                break;
                            case DayOfWeek.Tuesday:
                                interval.Tue = todo.Title;
                                break;
                            case DayOfWeek.Wednesday:
                                interval.Wed = todo.Title;
                                break;
                            case DayOfWeek.Thursday:
                                interval.Thu = todo.Title;
                                break;
                            case DayOfWeek.Friday:
                                interval.Fri = todo.Title;
                                break;
                            case DayOfWeek.Saturday:
                                interval.Sat = todo.Title;
                                break;
                            case DayOfWeek.Sunday:
                                interval.Sun = todo.Title;
                                break;
                        }
                    }
                }
            }

            TodoListView.ItemsSource = timeIntervals;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

