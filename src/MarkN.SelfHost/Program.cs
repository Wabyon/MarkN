using System;
using System.Linq;
using Microsoft.Win32;
using Topshelf;
using Topshelf.Nancy;

namespace MarkN.SelfHost
{
    class Program
    {
        private static readonly string _serviceName = "MarkN";
        private static readonly string _displayName = "MarkN";
        private static readonly string _description = "Markdown Transform Application.";

        static void Main()
        {
            if (!Settings.ExistsFile && !CheckInstalledVsCpp()) return;

            var host = HostFactory.New(configurator =>
            {
                string portParameter = null;

                configurator.AddCommandLineDefinition("port", p => { portParameter = p; });
                configurator.ApplyCommandLine();

                int o;
                Settings.Instance.Port = int.TryParse(portParameter, out o) ? o : Settings.Instance.Port;

                configurator.EnableServiceRecovery(recover =>
                {
                    recover.RestartService(0);
                });

                configurator.Service<Service>(s =>
                {
                    s.ConstructUsing(name => new Service());
                    s.WhenStarted(sc => sc.Start());
                    s.WhenStopped(sc => sc.Stop());

                    s.WithNancyEndpoint(configurator, nancyConfigurator =>
                    {
                        nancyConfigurator.AddHost(port: Settings.Instance.Port);
                    });
                });

                configurator.StartAutomaticallyDelayed();

                configurator.RunAsLocalSystem();

                configurator.SetServiceName(_serviceName);
                configurator.SetDisplayName(_displayName);
                configurator.SetDescription(_description);
            });

            Console.WriteLine("Running Port: {0}", Settings.Instance.Port);

            host.Run();
        }

        static bool CheckInstalledVsCpp()
        {
            const string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key != null)
                {
                    if (
                        key.GetSubKeyNames()
                            .Select(keyName => key.OpenSubKey(keyName))
                            .Select(subkey => subkey.GetValue("DisplayName") as string)
                            .Any(displayName => displayName != null &&
                                                (displayName.Contains("Microsoft Visual C++ 2013") ||
                                                 displayName.Contains("Microsoft Visual C++ 2012"))))
                    {
                        return true;
                    }
                }
            }

            Console.WriteLine("you must install 32-bit and 64-bit Visual C++ Redistributable packages:");
            Console.WriteLine("Visual Studio 2012:");
            Console.WriteLine("http://www.microsoft.com/en-us/download/details.aspx?id=30679");
            Console.WriteLine("Visual Studio 2013:");
            Console.WriteLine("http://www.microsoft.com/en-us/download/details.aspx?id=40784");
            Console.ReadKey();
            return false;
        }
    }
}
