using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Subjects;

namespace Système
{
    public class Plugin : ISubject<string>, IDisposable
    {
        private AppDomain domain;
        private Plug plug;
        private Proxy proxy;

        public Plugin(AppDomain domain, Plug plug)
        {
            this.domain = domain;
            this.plug = plug;
            proxy = new Proxy();
            plug.Subscribe(proxy);
        }

        public void Dispose()
        {
            #if DEBUG
            System.Diagnostics.Trace.WriteLine("Disposing plug.");
            #endif
            
            plug.Dispose();

            #if DEBUG
            System.Diagnostics.Trace.WriteLine("Unloading App Domain.");
            #endif

            AppDomain.Unload(domain);
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            return proxy.Subscribe(observer);
        }

        public void OnCompleted()
        {
            return;
        }

        public void OnError(Exception error)
        {
            return;
        }

        public void OnNext(string value)
        {
            plug.Push(value);
        }

        public void Push(string text)
        {
            plug.Push(text);
        }
    }
}
