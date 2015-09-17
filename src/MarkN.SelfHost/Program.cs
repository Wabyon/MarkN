using System;
using System.Linq;
using MarkN.Modules;
using Microsoft.Win32;
using Nancy.Hosting.Self;
using Topshelf;

namespace MarkN.SelfHost
{
    class Program
    {
        private static readonly string _serviceName = "MarkN";
        private static readonly string _displayName = "MarkN";
        private static readonly string _description = "Markdown Transform Application.";

        static void Main(string[] args)
        {
            if (!CheckEnvironment()) return;

            Console.WriteLine("PortNo: {0}", Settings.Instance.PortNo);

            var uri = string.Format("http://localhost:{0}/", Settings.Instance.PortNo);

            HostFactory.Run(x =>
            {
                x.EnableServiceRecovery(recover =>
                {
                    recover.RestartService(0);
                });

                x.Service<NancyHost>(s =>
                {
                    s.ConstructUsing(name => new NancyHost(new Uri(uri)));
                    s.WhenStarted(sc => sc.Start());
                    s.WhenStopped(sc => sc.Stop());
                });

                x.StartAutomaticallyDelayed();

                x.RunAsLocalSystem();

                x.SetServiceName(_serviceName);
                x.SetDisplayName(_displayName);
                x.SetDescription(_description);
            });
        }

        static bool CheckEnvironment()
        {
            if (Settings.ExistsFile) return true;

            if (!CheckInstalledVsCpp()) return false;

            SetParams();

            return true;
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

        static void SetParams()
        {
            Console.WriteLine("set portno like 8080");
            while (true)
            {
                var portNoString = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(portNoString))
                {
                    continue;
                }

                int portno;
                if (!int.TryParse(portNoString, out portno))
                {
                    Console.WriteLine("set number.");
                    continue;
                }

                if (portno < 0 || portno > 65535)
                {
                    Console.WriteLine("choose 0 - 65535");
                    continue;
                }

                Settings.Instance.PortNo = portno;
                break;
            }
        }
    }
}
