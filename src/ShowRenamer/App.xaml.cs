using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShowRenamer.Services.File;
using ShowRenamer.Services.Tmdb;
using ShowRenamer.ViewModels;
using System;
using System.Windows;

namespace ShowRenamer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IHost host = Host
                .CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .ConfigureAppConfiguration(ConfigureConfiguration)
                .Build();

            using (IServiceScope serviceScope = host.Services.CreateScope())
            {
                IServiceProvider services = serviceScope.ServiceProvider;
                MainWindow mainWindow = services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
        }

        private void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection configurationBuilder)
        {
            configurationBuilder.AddTransient<IFileService, FileService>();            
            configurationBuilder.AddHttpClient<ITmdbService, TmdbService>()
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuration["Tmdb:BearerToken"]}");
                });

            configurationBuilder.AddTransient<IMainViewModel, MainViewModel>();
            configurationBuilder.AddTransient<ITmdbSearchViewModel, TmdbSearchViewModel>();

            configurationBuilder.AddTransient<MainWindow>();
        }

        

        private void ConfigureConfiguration(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddJsonFile("appsettings.json");
        }
    }
}
