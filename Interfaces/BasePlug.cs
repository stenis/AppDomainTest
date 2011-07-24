using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Interfaces
{
    public abstract class BasePlug : MarshalByRefObject, IPlug
    {
        public virtual void Push(string text) { }
    }
}
