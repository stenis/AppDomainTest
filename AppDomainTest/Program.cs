using System;
using System.Reflection;
using System.Threading;
using System.Linq;
using Interfaces;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace Système
{
    public class Module1
    {
        public static void Main()
        {
            var loader = new AssemblyLoader();
            
            var plugin = loader.LoadPlugInAppDomain("Plug.dll");

            plugin.Delay(TimeSpan.FromSeconds(2)).Subscribe(x => Console.WriteLine("Delayed: " + x));
            var isTrue = true;
            
            while (isTrue)
            {
                var text = Console.ReadLine();

                plugin.Push(text);
                
                if (string.IsNullOrEmpty(text))
                    isTrue = false;
            }

            
            plugin.Dispose();

            Console.WriteLine("Plugin disposed.");

            Console.ReadLine();
        }
    }
}