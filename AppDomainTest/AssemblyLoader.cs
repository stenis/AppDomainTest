using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Interfaces;
using System.Reactive.Subjects;

namespace Système
{
    public class AssemblyLoader
    {
        AppDomainSetup ads;

        public AssemblyLoader()
        {
            ads = new AppDomainSetup
            {
                ApplicationBase = System.Environment.CurrentDirectory,
                DisallowBindingRedirects = false,
                DisallowCodeDownload = true,
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile
            };
        }

        public Plugin LoadPlugInAppDomain(string name)
        {
            var assembly = Assembly.ReflectionOnlyLoadFrom(Environment.CurrentDirectory + "\\" + name);
            var assemblyName = assembly.FullName;
            
            var type = typeof(IPlug);
            //var plugTypeName = assembly. GetTypes().Where(t => t.IsClass && type.IsAssignableFrom(t)).First().FullName;

            var guid = Guid.NewGuid().ToString();
            var ad = AppDomain.CreateDomain(guid, null, ads);

            var addAsm = ad.Load(assemblyName);
            var plugTypeName = addAsm.GetTypes().Where(t => t.IsClass && type.IsAssignableFrom(t)).First().FullName;

            var plug = (Plug)ad.CreateInstanceAndUnwrap(assemblyName, plugTypeName);
           
            return new Plugin(ad, plug);
        }
    }
}
