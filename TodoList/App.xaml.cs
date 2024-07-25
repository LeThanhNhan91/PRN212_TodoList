using GUI;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Services.Interface;
using Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace TodoList
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            //var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            //loginWindow.Show();

        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoDbContext>();
            services.AddTransient<TodoRepository>();
            services.AddTransient<UserRepository>();
            services.AddTransient<ITodoService, TodoService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<LoginWindow>();
           

        }
    }

}
