using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Enumerations;

namespace Core.Events
{
    public class NetworkEventArgs : EventArgs
    {
        public NetworkStatus Status { get; private set; }

        public NetworkEventArgs(NetworkStatus status)
        {
            Status = status;
        }
    }

    public delegate void NetworkStatusUpdateHandler(object sender, NetworkEventArgs e);
}
