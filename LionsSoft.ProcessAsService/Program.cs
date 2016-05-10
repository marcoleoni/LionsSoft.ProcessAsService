using NLog;
using System;
using System.Diagnostics;
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

    /// <summary>
    /// Service class implementing Topshelf ServiceControl interface
    /// </summary>
    public class SrvStart : ServiceControl
    {

        /// <summary>
        /// Reference to process
        /// </summary>
        Process process = null;

        /// <summary>
        /// The logger instance
        /// </summary>
        Logger logger;

        /// <summary>
        /// Start the service
        /// </summary>
        /// <param name="hostControl">passed by topshelf</param>
        /// <returns>true if service started correctly</returns>
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
                psi.ErrorDialog = false;
                process = new Process();
                process.StartInfo = psi;
                var started = process.Start();
                logger.Info("Starting Process " + conf.FileToService);
                return started;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error starting process");
                return false;
            }
        }

        /// <summary>
        /// Stop the service
        /// </summary>
        /// <param name="hostControl">PAssed by topshelf</param>
        /// <returns>true if the service stopped correctly</returns>
        public bool Stop(HostControl hostControl)
        {
            if (process != null)
                process.Kill();
            if (logger != null)
            {
                logger.Info("ProcessAsService Closing");
            }
            return true;
        }
    }

    /// <summary>
    /// Configuration class implementing westwind AppConfiguration class
    /// </summary>
    public class Conf : AppConfiguration
    {
        private string config_file_name = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"LionsSoft\ProcessAsService.config");

        /// <summary>
        /// File name with full path of the file to service
        /// </summary>
        public string FileToService { get; set; }

        /// <summary>
        /// Working directory
        /// </summary>
        public string StartDirectory { get; set; }

        /// <summary>
        /// Optional arguments 
        /// </summary>
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
