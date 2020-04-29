using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SPInterface.RawPoint
{
    public class NomPoint
    {

        public NomPoint(string buf)
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
            radius = Convert.ToDouble(result[7]);
            uTol = Convert.ToDouble(result[8]);
            lTol = Convert.ToDouble(result[9]);


            Position = new DenseVector(new double[4] { x, y, z, 1 }) * SPInterface.CurrentAlignment;
            Vector = new DenseVector(new double[4] { u, v, w, 0 }) * SPInterface.CurrentAlignment;

            x = Position[0];
            y = Position[1];
            z = Position[2];
            u = Vector[0];
            v = Vector[1];
            w = Vector[2];
        }

        DenseVector Position;
        DenseVector Vector;
        public double x, y, z;
        public double u, v, w;
        public double radius;
        public int seq;
        public double lTol, uTol;
    }
}
