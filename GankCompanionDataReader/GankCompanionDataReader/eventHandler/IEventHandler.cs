using events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GankCompanionDataReader.eventHandler
{
    interface IEventHandler
    {

        void HandleEvent(EventCodes eventCode, Dictionary<byte, object> parameters);
    }
}
