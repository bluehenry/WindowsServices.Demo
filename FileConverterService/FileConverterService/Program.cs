using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace FileConverterService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.UseNLog();

                serviceConfig.Service<ConverterService>(serviceInstance =>
                {
                    serviceInstance.ConstructUsing(() => new ConverterService());
                    serviceInstance.WhenStarted(execute => execute.Start());
                    serviceInstance.WhenStopped(execute => execute.Stop());
                    serviceInstance.WhenPaused(execute => execute.Pause());
                    serviceInstance.WhenContinued(execute => execute.Continue());
                    serviceInstance.WhenCustomCommandReceived((execute, hostControl, commandNumber) => execute.CustomCommand(commandNumber));
                });

                serviceConfig.EnableServiceRecovery(recoveryOption => 
                {
                    recoveryOption.RestartService(1);
                    recoveryOption.RestartComputer(60, "PS Demo");
                    recoveryOption.RunProgram(5, @"C\someprograme.exe");
                });

                serviceConfig.EnablePauseAndContinue();
                serviceConfig.SetServiceName("FileConverterService");
                serviceConfig.SetDisplayName("File Converter Service");
                serviceConfig.SetDescription("A demo service");

                serviceConfig.StartAutomatically();
            });
        }
    }
}
