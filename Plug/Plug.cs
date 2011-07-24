using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using System.Threading;

namespace z
{
    public class Plug : BasePlug, ISubject<string>, IDisposable
    {
        private List<IObserver<string>> listeners;

        public Plug()
        { 
            listeners = new List<IObserver<string>>();
        }

        public override void Push(string text)
        {
            string callingDomainName = Thread.GetDomain().FriendlyName;
            Console.WriteLine("AppDomain: {0} | {1}", callingDomainName, text);
            
            listeners.ForEach(o => o.OnNext("I got pushed!"));
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            listeners.Add(observer);
            return this;
        }

        public void OnCompleted()
        {
        
        }

        public void OnError(Exception error)
        {
        
        }

        public void OnNext(string value)
        {
            Push(value);
        }

        public void Dispose()
        {
            listeners.ForEach(o => o.OnCompleted());
        }
    }
}
