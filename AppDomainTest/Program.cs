using System;
using System.Reflection;
using System.Threading;
using System.Linq;
using Interfaces;
using System.Collections.Generic;

namespace z
{
    public class Module1
    {
        public static void Main()
        {
            var listener = Observer.Create<string>(x => Console.WriteLine("Recieved: " + x));
            var pusher = Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => x.ToString());
            // Get and display the friendly name of the default AppDomain.
            var callingDomainName = Thread.GetDomain().FriendlyName;
            //Console.WriteLine(callingDomainName);

            // Get and display the full name of the EXE assembly.
            var assembly = Assembly.LoadFrom(Environment.CurrentDirectory + "\\Plug.dll");
            var type = typeof(IPlug);
            var plugType = assembly.GetTypes().Where(t => t.IsClass && type.IsAssignableFrom(t)).First();
            var assemblyName = assembly.FullName;
            
            //Console.WriteLine(assemblyName);

            // Construct and initialize settings for a second AppDomain.
            AppDomainSetup ads = new AppDomainSetup
            {
                ApplicationBase = System.Environment.CurrentDirectory,
                DisallowBindingRedirects = false,
                DisallowCodeDownload = true,
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile
            };

            var domains = new List<AppDomain>();

            for (int i = 0; i < 5; i++)
            {
                // Create the second AppDomain.
                var ad = AppDomain.CreateDomain("AppD" + i, null, ads);

                domains.Add(ad);

                // Create an instance of MarshalbyRefType in the second AppDomain. 
                // A proxy to the object is returned.
                var plug =
                    (Plug)ad.CreateInstanceAndUnwrap(
                        assemblyName,
                        plugType.FullName
                    );

                var subject = plug.Subscribe(listener);
                
                plug.Push(callingDomainName);
            }

            

            domains.ForEach(x => {
                Console.WriteLine("Unloading: " + x.FriendlyName);
                AppDomain.Unload(x); 
            });

            Console.ReadLine();
        }
    }
}