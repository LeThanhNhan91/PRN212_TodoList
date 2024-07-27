using Hardcodet.Wpf.TaskbarNotification;
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

using System.Windows.Threading;

using Microsoft.VisualBasic.ApplicationServices;

using TodoList;
using Services.Interface;

namespace GUI
{
    /// <summary>
    /// Interaction logic for CalendarWindow.xaml
    /// </summary>
    public partial class CalendarWindow : Window
    {
        private readonly Services.TodoService _todoService = new();
        private TaskbarIcon _notifyIcon;
        private DispatcherTimer _timer;
        private DateOnly _current = DateOnly.FromDateTime(DateTime.Now);
        public Repositories.User User { get; set; }
        public DateOnly Date { get; set; }
        private TodoService _service = new TodoService();

        public CalendarWindow()
        {
            InitializeComponent();
            Closing += Window_Closing;
            InitializeNotification();
        }

        private void CalendarWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void InitializeNotification()
        {
            _notifyIcon = new TaskbarIcon();
            _notifyIcon.Icon = new System.Drawing.Icon("favicon.ico");
            _notifyIcon.ToolTipText = "TodoList Application";
            _notifyIcon.TrayLeftMouseUp += NotifyIcon_TrayLeftMouseUp;
            _notifyIcon.ContextMenu = (ContextMenu)FindResource("NotifyIconContextMenu");

            //------------- khai bao thông báo giờ gần đến
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }
        private void QuitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _notifyIcon.Dispose();
            Application.Current.Shutdown();
        }
        private void NotifyIcon_TrayLeftMouseUp(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }

        public void ShowWindow()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            CheckUpcomingTasks();
        }

        private void CheckUpcomingTasks()
        {
            var tasks = _todoService.GetTasksByUserAndTime(User.UserId, _current);
            DateTime now = DateTime.Now;

            foreach (var task in tasks)
            {
                DateTime taskTime = task.Time;
                var minutesUntilTask = (taskTime - now).TotalMinutes;

                if (minutesUntilTask <= 5 && minutesUntilTask > 4.9)
                {
                    _notifyIcon.ShowBalloonTip("Upcoming Task", $"5 minutes left until {task.Title} starts", BalloonIcon.Info);
                }
                else if (minutesUntilTask <= 0 && minutesUntilTask >= -0.1)
                {
                    _notifyIcon.ShowBalloonTip("Task Starting", $"Task {task.Title} is starting now!", BalloonIcon.Info);
                }

            }
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            if (_notifyIcon == null)
            {
                _notifyIcon = new TaskbarIcon();
                _notifyIcon.Icon = new System.Drawing.Icon("favicon.ico");
               

            }
            _notifyIcon.ShowBalloonTip("TodoList", "The application has been minimized to the system tray.", BalloonIcon.Info);
        }
    }
}

