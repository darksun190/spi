﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using SPInterface;

namespace SPInterface
{
    public class Plane : Element
    {
        DenseVector Vector;
        DenseVector Position;
        string points_file_name;
        double length, width;

        public Plane(Element fea)
        {
            if (fea.GeoType != FeatureType.Plane)
                throw (new Exception("geoType error"));
            this.xml_paras = fea.xml_paras;
            this.Identifier = fea.Identifier;
            this.geoType = fea.GeoType;
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

            feature_alignment = new Alignment(Vector, Position, Identifier + "_alignment");

        }
        public override List<MeasPoint> Alignment_Points
        {
            get
            {
                return measPoints;
            }
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
        public List<MeasPoint> transferedPoints
        {
            get
            {
                if (feature_alignment_points == null)
                {
                    feature_alignment_points = new List<MeasPoint>();
                    foreach (MeasPoint point in Alignment_Points)
                    {
                        feature_alignment_points.Add(new MeasPoint(point, feature_alignment));
                    }
                }
                return feature_alignment_points;
            }
        }
        public override List<double> Deviations
        {
            get
            {
                if (_devs == null)
                {
                    double x0, y0, z0, i0, j0, k0;
                    Vector vec_base = this.Vector * SPInterface.CurrentAlignment.Transpose();
                    Vector pos_base = this.Position * SPInterface.CurrentAlignment.Transpose();
                    x0 = pos_base[0];
                    y0 = pos_base[1];
                    z0 = pos_base[2];
                    i0 = vec_base[0];
                    j0 = vec_base[1];
                    k0 = vec_base[2];
                    _devs = new List<double>();
                    foreach (var temp in measPoints)
                    {
                        _devs.Add(i0*(temp.x-x0) +j0*(temp.y-y0)+k0*(temp.z-z0));
                    }
                }
                return _devs;
            }
        }
    }
}
