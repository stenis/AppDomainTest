using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using System.Threading;
using System.Reactive.Subjects;

namespace Système
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
            //string callingDomainName = Thread.GetDomain().FriendlyName;
            //Console.WriteLine("AppDomain: {0} | {1}", callingDomainName, text);
            
            listeners.ForEach(o => o.OnNext("echo: " + text));
        }


        public IDisposable Subscribe(IObserver<string> observer)
        {
            listeners.Add(observer);
            return new Subscription(this, observer);
        }

        public void UnSubscribe(IObserver<string> observer)
        {
            Console.WriteLine("Unsubscribed.");
            listeners.Remove(observer);
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
