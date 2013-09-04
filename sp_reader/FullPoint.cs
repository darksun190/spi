using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SPInterface
{
     class FullPoint 
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
        }
        
        public double nom_x, nom_y, nom_z, act_x, act_y, act_z;
        public double u, v, w;
        public double uTol, lTol;
        public int seq,status;
        public double radius;
    }
}
