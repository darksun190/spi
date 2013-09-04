using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SPInterface
{
    public class MeasPoint
    {
        public MeasPoint()
        {

        }
        public MeasPoint(string buf,bool isCurve = true)
        {
            string pattern = @"\s+";
            string[] result = Regex.Split(buf, pattern);
            if (isCurve)
            {
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
            else
            {
                seq = 0;
                x = Convert.ToDouble(result[0]);
                y = Convert.ToDouble(result[1]);
                z = Convert.ToDouble(result[2]);
                u = Convert.ToDouble(result[3]);
                v = Convert.ToDouble(result[4]);
                w = Convert.ToDouble(result[5]);
                status = 0;
                ProbeRadius = 0.0;
            }
        }


        public double x, y, z;
        public double u, v, w;
        public double ProbeRadius;
        public int seq, status;
    }
}
