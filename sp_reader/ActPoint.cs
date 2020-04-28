using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra.Double;

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

            Position = new DenseVector(new double[4] { x, y, z, 1 }) * SPInterface.Current_Alignment;
            Vector = new DenseVector(new double[4] { u, v, w, 0 }) * SPInterface.Current_Alignment;

            x = Position[0];
            y = Position[1];
            z = Position[2];
            u = Vector[0];
            v = Vector[1];
            w = Vector[2];
        }

        public DenseVector Position;
        public DenseVector Vector;
        public double x, y, z;
        public double u, v, w;
        public double ProbeRadius;
        public int seq, status;
        public ActPoint(ActPoint old, Alignment fea_align)
        {
            seq = old.seq;
            status = old.status;
            ProbeRadius = old.ProbeRadius;
            Position = old.Position * fea_align;
            Vector = old.Vector * fea_align;
            x = Position[0];
            y = Position[1];
            z = Position[2];
            u = Vector[0];
            v = Vector[1];
            w = Vector[2];
        }
    }
}
