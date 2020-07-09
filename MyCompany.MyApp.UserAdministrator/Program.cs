//������:  
//������� ���-���������� ��� ����������������� �������������.
//�����������: 
//- ������������� ������ ������������� (username, ���, ����)
//- ���������, ������������� � ������� ������������� 
//- �������� ������������ 
//  - ����� (������� ��� �������� � �� ����������)
//  - ��� 
//  - Email 
//  - ������ 
//  - ����(���������)
//������ ��������� ����� ����� �������(� �������, �������������, �������� ������������, ��������).     
//��������� � ����������: 
//- ���������� ������ ���� ��������� �� �������� Single-page application, � ������� ������� ����� ���������� � �������� ����� REST API(������� ���������� ��������� REST �������������)
//- ���������� ����������� �� ������ jwt
//- ������ ������ �������� ��.net Core � ������� ������ � �� 
//- ��� ��������� ����� ������������ typescript ��� js ����� ������ � ����� ����������/����������


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



