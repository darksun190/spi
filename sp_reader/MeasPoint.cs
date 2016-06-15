using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SPInterface
{
    public class MeasPoint
    {
        public MeasPoint()
        {

        }
        public MeasPoint(IList<double> datas)
        {
            if (datas.Count() < 3)
                throw (new IndexOutOfRangeException("at least 3 numbers"));
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
            Vector = new DenseVector(new double[4] { u, v, w, 0 }) ;

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
            Position = new DenseVector(new double[4] { x, y, z, 1 }) * SPI.current_alignment.Transpose();
            Vector = new DenseVector(new double[4] { u, v, w, 0 }) * SPI.current_alignment.Transpose();

            x = Position[0];
            y = Position[1];
            z = Position[2];
            u = Vector[0];
            v = Vector[1];
            w = Vector[2];
        }

        public readonly DenseVector Position;
        public readonly DenseVector Vector;
        public double x, y, z;
        public double u, v, w;
        public double ProbeRadius;
        public int seq, status;
        public MeasPoint(MeasPoint old, Alignment fea_align)
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
        public override string ToString()
        {
            return string.Format("{0:F4} {1:F4} {2:F4} {3:F4} {4:F4} {5:F4}",
                new object[]
                {
                    x,
                    y,
                    z,
                    u,
                    v,
                    w
                });
        }
    }
}
