//Задача:  
//Сделать веб-приложение для администрирования пользователей.
//Возможности: 
//- Просматривать список пользователей (username, имя, роли)
//- Создавать, редактировать и удалять пользователей 
//- Свойства пользователя 
//  - Логин (задаётся при создании и не изменяется)
//  - Имя 
//  - Email 
//  - Пароль 
//  - Роли(несколько)
//Список возможных ролей задан заранее(к примеру, Администратор, Редактор справочников, Заказчик).     
//ребования к реализации: 
//- Приложение должно быть построено по принципу Single-page application, с обменом данными между фронтендом и бэкендом через REST API(строгое соблюдение стандарта REST необязательно)
//- Желательна авторизация на основе jwt
//- Бэкенд должен работать на.net Core и хранить данные в БД 
//- Для фронтенда можно использовать typescript или js любой версии и любые фреймворки/библиотеки


using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;




namespace MyCompany.MyApp.UserAdministrator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("NLog.Config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init app");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "error in init");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
       {
           webBuilder.UseStartup<Startup>();
       })
       .ConfigureLogging(logging =>
       {
           logging.ClearProviders();
           logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
       })
       .UseNLog();
    }
}



