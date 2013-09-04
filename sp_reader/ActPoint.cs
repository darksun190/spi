using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SPInterface
{
    public class ActPoint
    {
        public ActPoint(string buf)
        {
            string pattern = @"\s+";
            string[] result = Regex.Split(buf, pattern);
            seq = Convert.ToInt32(result[0]);
            x = Convert.ToDouble(result[1]);
            y = Convert.ToDouble(result[2]);
            z = Convert.ToDouble(result[3]);
            u = Convert.ToDouble(result[4]);
            v = Convert.ToDouble(result[5]);
            w = Convert.ToDouble(result[6]);
            status = Convert.ToInt32(result[7]);
            ProbeRadius = Convert.ToDouble(result[8]);

        }


        public double x, y, z;
        public double u, v, w;
        public double ProbeRadius;
        public int seq, status;
    }
}
