using System;

namespace Système
{
    public class Subscription : MarshalByRefObject, IDisposable
    {
        private Plug plug;
        private IObserver<string> observer;

        public Subscription(Plug plug, IObserver<string> observer)
        {
            this.plug = plug;
            this.observer = observer;
        }

        public void Dispose()
        {
            plug.UnSubscribe(observer);
        }
    }
}