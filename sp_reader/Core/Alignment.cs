using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using System.Xml;

namespace SPInterface.Core
{
    /*
     * CALYPSO transfer a 4*4 matrix as alignment
     * the measure points of features are NOT based on this alignment but base alignment
     */
    public class Alignment
    {
        private DenseMatrix trans_matrix;
        public string Name;
        public Alignment()
        {
            Name = "Base Alignment";
            trans_matrix = DenseMatrix.OfArray(new double[4, 4]
            {
            {1,0,0,0},
            {0,1,0,0},
            {0,0,1,0},
            {0,0,0,1}
            }
        );
        }
        public Alignment(XmlNode node)
            : this()
        {
            if (node.Attributes.Count == 0)
                return;
            Name = node.Attributes["Identifier"].Value.Trim('\"').Trim();
            string content = node.Attributes["CoordSystem"].Value.Trim('\"').Trim();
            string[] seperator = new string[2]{
                " ",
                "|"
            };
            double[] array =
                content
                .Split(seperator, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(n => Convert.ToDouble(n))
                .ToArray();

            trans_matrix = new DenseMatrix(4, 4, array);
        }
        public Alignment(DenseVector vec, DenseVector pos, string name = "feature_alignment")
            : this(vec[0], vec[1], vec[2], pos[0], pos[1], pos[2], name)
        {

        }
        public Alignment(double i, double j, double k, double x = 0, double y = 0, double z = 0, string name = "feature_alignment")
        {
            DenseMatrix offset_matrix = new DenseMatrix
                (4, 4,
                    new double[]
                    { 
                        1, 0.0, 0 , -x,
                        0, 1, 0 , -y,
                        0, 0, 1 , -z,
                        0, 0, 0 , 1 
                    }
                );
            DenseMatrix rotate_matrix = null;

            if (Math.Abs(Math.Abs(j) - 1) > 0.1)
            {
                rotate_matrix = new DenseMatrix(4, 4,
                new double[]{
                     k / Math.Sqrt ( 1-j*j ),      0.0,                    -i / Math.Sqrt ( 1-j*j )    ,   0, 
                     - j*i / Math.Sqrt ( 1-j*j ),  Math.Sqrt ( 1-j*j ),    - j*k / Math.Sqrt ( 1-j*j ) ,   0, 
                     i,                            j,                      k                           ,   0,
                    0,                           0,                      0                          ,   1
                }
                    );
            }
            else
            {
                rotate_matrix = new DenseMatrix(4,4,
                    new double[]{
                        j/Math.Sqrt(1-k*k),     i/Math.Sqrt(1-k*k)      ,   0.0,    0,
                        - k*i/Math.Sqrt(1-k*k), -k*j/Math.Sqrt(1-k*k)   ,   Math.Sqrt(1-k*k),   0,
                        i,                      j                       ,   k,                  0,
                        0,                      0                       ,   0,                  1
                    });
            }
            trans_matrix = offset_matrix * rotate_matrix;

        }
        public static DenseVector operator *(DenseVector a, Alignment b)
        {
            return a * b.trans_matrix;
        }
        Alignment(DenseMatrix dm)
        {
            this.trans_matrix = dm;
        }
        public Alignment Transpose()
        {
            DenseMatrix t_matrix = DenseMatrix.OfMatrix(trans_matrix.Transpose());
            t_matrix[3, 0] = -t_matrix[0, 3];
            t_matrix[3, 1] = -t_matrix[1, 3];
            t_matrix[3, 2] = -t_matrix[2, 3];
            t_matrix[0, 3] = 0;
            t_matrix[1, 3] = 0;
            t_matrix[2, 3] = 0;

            Alignment result = new Alignment(t_matrix);
            result.Name = this.Name;
            return result;
        }
        public override string ToString()
        {
            string result;
            result = Name + "\n";
            result += trans_matrix.ToString();
            return result;
        }
    }
}
