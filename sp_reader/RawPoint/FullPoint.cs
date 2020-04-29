using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SPInterface.RawPoint
{
    public class FullPoint
    {
        public FullPoint(NomPoint nom, ActPoint act)
        {
            if (nom.seq != act.seq)
            {
                throw new IndexOutOfRangeException();
            }
            nom_x = nom.x;
            nom_y = nom.y;
            nom_z = nom.z;
            act_x = act.x;
            act_y = act.y;
            act_z = act.z;
            u = nom.u;
            v = nom.v;
            w = nom.w;
            uTol = nom.uTol;
            lTol = nom.lTol;
            seq = nom.seq;
            status = act.status;
            radius = act.ProbeRadius;
            double xdev, ydev, zdev;
            xdev = Math.Abs(act_x - nom_x);
            ydev = Math.Abs(act_y - nom_y);
            zdev = Math.Abs(act_z- nom_z);
            //calculate the dev is minus or plus
            int max_no = 0;
            if (ydev > xdev)
            {
                if (zdev > ydev)
                {
                    max_no = 2;
                }
                else
                {
                    max_no = 1;
                }
            }
            else if (zdev > xdev)
            {
                max_no = 2;
            }
            double si=0;
            switch (max_no)
            {
                case 0:
                    si = Math.Sign((act_x - nom_x) / u);
                    break;
                case 1:
                    si = Math.Sign((act_y - nom_y) / v);
                    break;
                case 2:
                    si = Math.Sign((act_z - nom_z) / w);
                    break;
                default:
                    break;
             }

            dev = si * Math.Sqrt(Math.Pow(act_x - nom_x,2) + Math.Pow(act_y - nom_y,2) + Math.Pow(act_z - nom_z,2));
            if (dev < uTol && dev > lTol)
                ok = true;
            else
                ok = false;
        }
        public double nom_x, nom_y, nom_z, act_x, act_y, act_z;
        public double u, v, w;
        public double uTol, lTol;
        public double dev;
        public bool ok;
        public int seq, status;
        public double radius;
    }
}
