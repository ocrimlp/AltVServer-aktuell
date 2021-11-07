using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVServer
{
    public class TPlayer : Player
    {
        public int SpielerID { get; set; }
        public String SpielerName { get; set; }
        public long Geld { get; set; }
        public int Adminlevel { get; set; }

        public bool Eingeloggt { get; set; }

        public TPlayer(IServer server, IntPtr nativePointer, ushort id) : base(server, nativePointer, id) 
        {
            Geld = 5000;
            Adminlevel = 0;
            Eingeloggt = false;
        }
    }
}
