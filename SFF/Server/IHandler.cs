using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    interface IHandler
    {
        void HandleMessage(string arguments);

    }
}
