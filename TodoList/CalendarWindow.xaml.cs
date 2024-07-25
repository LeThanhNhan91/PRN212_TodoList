using Repositories;
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

            var timeIntervals = GenerateTimeIntervals();
            var todos = _todoService.GetAllTasks().Where(t => t.UserId == User.UserId && t.ModifiedDate.Date == Date.ToDateTime(TimeOnly.MinValue)).ToList();

            foreach (var todo in todos)
            {
                var startHour = todo.ModifiedDate.Hour;
                var duration = todo.Time.Hour - todo.ModifiedDate.Hour + 1; // assuming Time is the end time

                for (int i = 0; i < duration; i++)
                {
                    var interval = timeIntervals.FirstOrDefault(t => t.Time.Hour == (startHour + i) % 24);
                    if (interval != null)
                    {
                        if (todo.ModifiedDate.DayOfWeek == DayOfWeek.Monday) interval.Mon = todo.Title;
                        else if (todo.ModifiedDate.DayOfWeek == DayOfWeek.Tuesday) interval.Tue = todo.Title;
                        else if (todo.ModifiedDate.DayOfWeek == DayOfWeek.Wednesday) interval.Wed = todo.Title;
                        else if (todo.ModifiedDate.DayOfWeek == DayOfWeek.Thursday) interval.Thu = todo.Title;
                        else if (todo.ModifiedDate.DayOfWeek == DayOfWeek.Friday) interval.Fri = todo.Title;
                        else if (todo.ModifiedDate.DayOfWeek == DayOfWeek.Saturday) interval.Sat = todo.Title;
                        else if (todo.ModifiedDate.DayOfWeek == DayOfWeek.Sunday) interval.Sun = todo.Title;
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
    }
}

