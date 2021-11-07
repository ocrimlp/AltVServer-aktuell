using AltV.Net;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVServer
{
    public class TPlayerFactory : IEntityFactory<TPlayer>
    {
        public TPlayer Create(IServer server, IntPtr entityPointer, ushort id)
        {
            return new TPlayer(server, entityPointer, id);
        }
    }
}
