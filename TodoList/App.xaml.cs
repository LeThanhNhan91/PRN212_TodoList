using GUI;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Services.Interface;
using Services;
using System.Configuration;
using System.Data;
using System.Windows;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;


namespace TodoList
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();

        }

        private void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();

            services.AddSingleton(Configuration);

            services.AddDbContext<TodoDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionStringDB")));


            services.AddDbContext<TodoDbContext>();
            services.AddTransient<TodoRepository>();
            services.AddTransient<UserRepository>();
            services.AddTransient<ITodoService, TodoService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<LoginWindow>();
            // phần redis
            var multiplexer = ConnectionMultiplexer.Connect(Configuration.GetConnectionString("RedisConnection"));
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddTransient<RedisService>();


        }
    }

}
