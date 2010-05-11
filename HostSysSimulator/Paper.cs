using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HostSysSim
{
    class Paper
    {
        public HostSysSim.Point size{ get; set; }
        public Paper(String sVal)   // should be in %d,%d format
        {
            size = new Point(sVal);
        }
        public Paper(int x, int y)
        {
            size = new Point(x,y);
        }
    }
}
