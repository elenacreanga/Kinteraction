using System;
using System.Collections.Generic;

namespace Kinteraction.Kinteract.Players
{
    public class UsersFacadeEventArgs : EventArgs
    {
        public HashSet<ulong> Users { get; set; }
    }
}