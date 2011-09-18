using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reactive.Subjects;

namespace Système
{
    public class Proxy : MarshalByRefObject, ISubject<string>
    {
        private IObserver<string> listener;

        public void OnCompleted()
        {
            if (listener != null)
            {
                listener.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            if (listener != null)
            {
                listener.OnError(error);
            }
        }

        public void OnNext(string value)
        {
            //string callingDomainName = Thread.GetDomain().FriendlyName;
            //Console.WriteLine("AppDomain: {0} | {1}", callingDomainName, value);

            if (listener != null)
            {
                listener.OnNext(value);
            }
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            if (listener != null)
            {
                throw new Exception("Only one subscriber allowed.");
            }

            listener = observer;
            return null;
        }
    }
}
