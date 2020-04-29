using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra.Double;
using SPInterface.Core;

namespace SPInterface.RawPoint
{
    public class FeatureMeasurePoint
    {
        public FeatureMeasurePoint(IList<double> datas)
        {
            if (datas.Count() < 3)
                throw (new IndexOutOfRangeException("at least 3 numbers"));
            double x, y, z, u, v, w;
            seq = 0;
            x = datas[0];
            y = datas[1];
            z = datas[2];
            if (datas.Count() >= 6)
            {
                u = datas[3];
                v = datas[4];
                w = datas[5];
            }
            else
            {
                u = 0;
                v = 0;
                w = 1;
            }
            Position = new DenseVector(new double[4] { x, y, z, 1 });
            Vector = new DenseVector(new double[4] { u, v, w, 0 });

        }
        public FeatureMeasurePoint(string buf)
        {

            string pattern = @"\s+";
            string[] result = Regex.Split(buf, pattern);
            double x, y, z, u, v, w;

            x = Convert.ToDouble(result[0]);
            y = Convert.ToDouble(result[1]);
            z = Convert.ToDouble(result[2]);
            u = Convert.ToDouble(result[3]);
            v = Convert.ToDouble(result[4]);
            w = Convert.ToDouble(result[5]);

            Position = new DenseVector(new double[4] { x, y, z, 1 });
            Vector = new DenseVector(new double[4] { u, v, w, 0 });
        }

        public readonly DenseVector Position;
        public readonly DenseVector Vector;
        public double ProbeRadius { get; set; }

        public double X { get => Position[0]; }
        public double Y { get => Position[1]; }
        public double Z { get => Position[2]; }
        public double U { get => Vector[0]; }
        public double V { get => Vector[1]; }
        public double W { get => Vector[2]; }

        public int seq { get; set; }
        public int status { get; set; }
        public FeatureMeasurePoint(FeatureMeasurePoint old, Alignment alignment)
        {
            seq = old.seq;
            status = old.status;
            ProbeRadius = old.ProbeRadius;
            Position = old.Position * alignment;
            Vector = old.Vector * alignment;
        }
        public static FeatureMeasurePoint operator *(FeatureMeasurePoint p, Alignment alignment)
        {
            return new FeatureMeasurePoint(p, alignment);
        }
        public override string ToString()
        {
            return string.Format("{0:F4} {1:F4} {2:F4} {3:F4} {4:F4} {5:F4}",
                new object[]
                {
                    X,
                    Y,
                    Z,
                    U,
                    V,
                    W
                });
        }
    }
}
