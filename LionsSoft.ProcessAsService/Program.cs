using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Topshelf;
using Westwind.Utilities.Configuration;

namespace LionsSoft.ProcessAsService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(settings =>
            {
                settings.Service<SrvStart>();
                settings.RunAsLocalSystem();
                settings.SetDescription("LionsSoft - Process as Service");
                settings.SetDisplayName("LionsSoft.ProcessAsService");
                settings.SetServiceName("LionsSoft.ProcessAsService");
                settings.StartAutomatically();
                settings.EnableShutdown();
            });
        }
    }

    public class SrvStart : ServiceControl
    {

        Process process = null;
        Logger logger;
        StreamReader outputReader;
        public bool Start(HostControl hostControl)
        {
            
            logger = new LogFactory().GetLogger("*");
            try 
            { 
                logger.Info("ProcessAsServer Started");
                var conf = new Conf();
                conf.Initialize();
                logger.Info("Configuration readed");
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = conf.FileToService;
                psi.WorkingDirectory = conf.StartDirectory;
                psi.UseShellExecute = false;
                psi.LoadUserProfile = false;
                psi.Arguments = conf.Arguments;
                psi.RedirectStandardOutput = true;
                psi.ErrorDialog = false;
                process = new Process();
                process.StartInfo = psi;
                var started = process.Start();
                outputReader = process.StandardOutput;
                logger.Info("Starting Process " + conf.FileToService);
                return started;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error starting process");
                return false;
            }
        }

        public bool Stop(HostControl hostControl)
        {
            if (outputReader != null)
            {
                logger.Info(outputReader.ReadToEnd());
            }
            if (process != null)
                process.Kill();
            return true;
        }
    }

    public class Conf : AppConfiguration
    {
        private string config_file_name = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"LionsSoft\ProcessAsService.config");
        public string FileToService { get; set; }
        public string StartDirectory { get; set; }
        public string Arguments { get; set; }
        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            var provider = new XmlFileConfigurationProvider<Conf>()
            {
                XmlConfigurationFile = config_file_name,
                ConfigurationSection = sectionName,
                EncryptionKey = "ultrasecretkey",
            };

            return provider;
        }
 
    }

}
