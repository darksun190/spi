using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;

namespace SPInterface
{
    class Plane : Feature
    {
        DenseVector Vector;
        DenseVector Position;
        string points_file_name;
        double length, width;

        public Plane(Feature fea)
        {
            if (fea.geoType != "Plane")
                throw (new Exception("geoType error"));
            this.xml_paras = fea.xml_paras;
            this.identifier = fea.identifier;
            this.geoType = fea.geoType;
            //get length and width
            length = Convert.ToDouble(xml_paras["Length"]);
            width = Convert.ToDouble(xml_paras["Width"]);

            //init Vector
            string vector_string = xml_paras["Vector"].Trim('\"');
            Vector = new DenseVector(
                vector_string
                .Split(' ')
                .Select(n => Convert.ToDouble(n))
                .ToArray()
                );

            //init Position
            string position_string = xml_paras["Position"].Trim('\"');
            Position = new DenseVector(
                position_string
                .Split(' ')
                .Select(n => Convert.ToDouble(n))
                .ToArray()
                );

            //init get meas Points
            points_file_name = xml_paras["Points"];
            FileInfo meas_fi = new FileInfo(points_file_name); 
            StreamReader meas_sr = new StreamReader(meas_fi.FullName);
            string line;
            measPoints = new List<MeasPoint>();
            measMaskedPoints = new List<MeasPoint>();
            int seq_nr = 1;
            while ((line = meas_sr.ReadLine()) != null)
            {
                MeasPoint temp_Meas_point = new MeasPoint(line, false);
                if (temp_Meas_point.status == 0)
                {
                    measPoints.Add(temp_Meas_point);
                    measPoints.Last().seq = seq_nr++;
                }
                else
                {
                    measMaskedPoints.Add(temp_Meas_point);
                    measMaskedPoints.Last().seq = seq_nr++;
                }
            }

            feature_alignment = new Alignment(Vector, Position, identifier + "_alignment");

        }
        public double i
        {
            get
            {
                return Vector[0];
            }
        }
        public double j
        {
            get
            {
                return Vector[1];
            }
        }
        public double k
        {
            get
            {
                return Vector[2];
            }
        }
        public double area
        {
            get
            {
                return length * width;
            }
        }
    }
}
