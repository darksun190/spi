using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPInterface
{
    public class Plane: Element
    {
        new public readonly string geoType;
        new public readonly string identifier;
        new public readonly string fileMeasPoints;
        List<MeasPoint> measPoints;

        new public readonly double[] pos;
        new public readonly double[] vec;
      
        public readonly int size;
        public readonly List<double> angle_commu;

        public List<MeasPoint> transferedPoints;
        public double round_nr;
        public int direction;      //  -1 = clockwise  ;   1 = anticlockwise
        public Plane(Element parent)
        {
            if (!(parent.geoType.Equals("Plane") ))
            {
                throw new Exception("Type not right");
            }
            geoType = parent.geoType;
            identifier = parent.identifier;
            fileMeasPoints = parent.fileMeasPoints;
            measPoints = parent.getMeasPoints();
            pos = parent.pos;
            vec = parent.vec;
       
            angle_commu = new List<double>();
            double[] zero_point = new[] { 0, 0, pos[2] };
            /* transform the coordinate of the data.
             * special program offer the zero position of the circle or cylinder
             * use it as new 0 zero point,
             * O = (x0,y0,z0)
             * special program offer the vector of the cylinder's axis, use it as Z axis
             * the formula is
             * if the vector is (l,m,n)
             * the matrix Mz is
             *                  Keep the X axis in XOY plane
             * |    - m / sqrt ( 1-n^2 )    ,   l / sqrt ( 1-n^2 )      ,   0               |
             * |    - n*l / sqrt ( 1-n^2 )  ,   - m*n / sqrt ( 1-n^2 )  ,   sqrt ( 1-n^2 )  |
             * |            l               ,           m               ,       n           |
             *
             *                  Keep the X axis in XOZ plane
             * |     n / sqrt ( 1-m^2 )     ,           0           ,  - l / sqrt( 1-m^2 )  |
             * |    - m*l / sqrt ( 1-m^2 )  ,   sqrt ( 1-m^2 )      ,  - m*n / sqrt ( 1-m^2)|
             * |            l               ,           m           ,           n           |
             *
             *
             * the new points' coordinate P' = Mz * (P-O)
             */

            double l, m, n;
            l = vec[0];
            m = vec[1];
            n = vec[2];

            var matrix1 =
            new[,]{
                    { n / Math.Sqrt ( 1-m*m ),      0.0,                    -l / Math.Sqrt ( 1-m*m )    }, 
                    { - m*l / Math.Sqrt ( 1-m*m ),  Math.Sqrt ( 1-m*m ),    - m*n / Math.Sqrt ( 1-m*m ) }, 
                    { l,                            m,                      n                           } 
                };
            //start transform

            transferedPoints = new List<MeasPoint>();
            
            foreach (MeasPoint temp in measPoints)
            {
                double[] vect = new[] { temp.x, temp.y, temp.z };
                double[] vecO = new double[3];
                vecO[0] = (vect[0] - zero_point[0]) * (matrix1[0, 0] + matrix1[1, 0] + matrix1[2, 0]);
                vecO[1] = (vect[1] - zero_point[1]) * (matrix1[0, 1] + matrix1[1, 1] + matrix1[2, 1]);
                vecO[2] = (vect[2] - zero_point[2]) * (matrix1[0, 2] + matrix1[1, 2] + matrix1[2, 2]);

                MeasPoint buf_point = new MeasPoint();
                buf_point.x = vecO[0];
                buf_point.y = vecO[1];
                buf_point.z = vecO[2];
                buf_point.seq = temp.seq;
                transferedPoints.Add(buf_point);
            }
            size = transferedPoints.Count();

            if (size < 9)
            {
                ;
                //means points was too less;
            }

            double angle1, angle3, angle5;
            angle1 = Math.Atan2(transferedPoints[0].y, transferedPoints[0].x);
            angle3 = Math.Atan2(transferedPoints[4].y, transferedPoints[4].x);
            angle5 = Math.Atan2(transferedPoints[8].y, transferedPoints[8].x);
            if ((angle5 - angle3) * (angle3 - angle1) < 0)
            {
                //means the start point nearby the 180-degree
                double abs13, abs35;
                abs13 = Math.Abs(angle1 - angle3);
                abs35 = Math.Abs(angle3 - angle5);
                if (abs13 < abs35)
                {
                    direction = angle3 > angle1 ? 1 : -1;
                }
                else
                {
                    direction = angle5 > angle3 ? 1 : -1;
                }
            }
            else
            {
                //normal situation
                direction = angle3 > angle1 ? 1 : -1;
            }
            round_nr = 0;
            angle_commu.Add(round_nr);
            double next_angle, last_angle;
            last_angle = angle1;
            for (int i = 1; i < size; ++i)
            {
                next_angle = Math.Atan2(transferedPoints[i].y, transferedPoints[i].x);
                if (Math.Abs(next_angle - last_angle) > Math.PI)
                {
                    if (direction == 1)
                    {
                        round_nr += next_angle + 2 * Math.PI - last_angle;
                    }
                    else
                    {
                        round_nr += next_angle - 2 * Math.PI - last_angle;
                    }
                }
                else
                {
                    round_nr += next_angle - last_angle;
                }
                //qDebug()<<last_angle<<"\t"<<next_angle<<"\t"<<next_angle-last_angle<<"\t"<<round_nr;
                last_angle = next_angle;
                angle_commu.Add(round_nr);

            }
        }
    }
}
