using RealTimeStockSimulator.Hubs;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Interfaces;
using RealTimeStockSimulator.Repositories;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services;
using RealTimeStockSimulator.Services.BackgroundServices;
using RealTimeStockSimulator.Services.HostedServices;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();

            builder.Services.AddSingleton<ITradablesRepository, DbTradablesRepository>();
            builder.Services.AddSingleton<IUsersRepository, DbUsersRepository>();

            builder.Services.AddSingleton<ITradablesService, TradablesService>();
            builder.Services.AddSingleton<IUsersService, UsersService>();

            builder.Services.AddSingleton<IStringFormatter, StringFormatter>();

            builder.Services.AddHostedService<ApiCacheInitializer>();
            builder.Services.AddHostedService<MarketWebsocketRelay>();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authentication}/{action=Login}/{id?}");

            app.MapHub<MarketHub>("/marketHub");

            app.Run();
        }
    }
}
