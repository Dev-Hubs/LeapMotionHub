using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeapFirst
{
    public interface ILeapEventDelegate
    {
        void LeapEventNotification(string EventName);
    }
    delegate void LeapEventDelegate(string EventName);

}
