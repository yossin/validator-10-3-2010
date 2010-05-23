using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HostSysSim
{
    class Point :IComparable,IComparable<Point>
    {
        public int x{ get; set; }
        public int y{ get; set; }
        public Point(String sVal)   // should be in %d,%d format
        {
            char[] seperator = {','};
            string[] sVals = sVal.Split(seperator);
            if (sVals.Length != 2)
                x = y = 0;
            else
            {
                x = System.Convert.ToInt32(sVals[0], 10);
                y = System.Convert.ToInt32(sVals[1], 10);
            }
        }
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int CompareTo(object obj)
        {
            return CompareTo((Point)obj);
        }

        public int CompareTo(Point other)
        {
            int xDif = x - other.x;
            int yDif = (y - other.y)*1000;
            return (xDif + yDif);
        }

    }
}
