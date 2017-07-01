using System;
using System.Collections.Generic;

namespace Kinteract.Players
{
    public class UsersFacadeEventArgs : EventArgs
    {
        public HashSet<ulong> Users { get; set; }
    }
}