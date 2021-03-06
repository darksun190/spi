﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;

namespace SPInterface
{
    public class Circle : Element
    {
        protected DenseVector Vector;
        protected DenseVector Position;
        protected string points_file_name;
        double length;
        protected double radius;
        protected bool inside;
        List<double> point_angle_offset;

        protected override double max_dev
        {
            get
            {
                return Deviations.Max();
            }
            
        }
        protected override double min_dev
        {
            get
            {
                return Deviations.Min();
            }
          
        }
        //how much revolutions of the measurement
        double round_revs;
        public double round_nr
        {
            get
            {
                if (round_revs == 0)
                    calc_angle_revs();
                return round_revs;
            }
        }
        void calc_angle_revs()
        {
            point_angle_offset = new List<double>();

            //minimum points
            int min_points = 9;
            if (point_no() < min_points)
            {
                throw (new Exception("too less Point, " + min_points.ToString() + " points at least"));
                //means points was too less;
            }


            //clockwise or anticlock

            int direction;
            point_angle_offset.Add(0);      //first point is 0
            double last_angle, cur_angle;
            last_angle = Math.Atan2(transferedPoints[0].y, transferedPoints[0].x);
            double temp_dev;
            for (int i = 1; i < point_no(); ++i)
            {
                cur_angle = Math.Atan2(transferedPoints[i].y, transferedPoints[i].x);
                double t = cur_angle - last_angle; 
                if (Math.Abs(t) > Math.PI)
                {
                    if (t > 0)
                    {
                        temp_dev = t - Math.PI * 2;
                    }
                    else
                    {
                        temp_dev = t + Math.PI * 2;
                    }
                }
                else
                {
                    temp_dev = t;
                }

                last_angle = cur_angle;
                point_angle_offset.Add(temp_dev);

            }
            round_revs = 0;
            foreach(double n in point_angle_offset)
            {
                round_revs += n;
            }
            if (round_revs > 0)
                direction = 1;
            else
                direction = -1;

        }
        public List<double> angle_commu
        {
            get
            {
                if (point_angle_offset == null)
                {
                    calc_angle_revs();
                }
                return point_angle_offset;
            }
        }

        public Circle()
        {
        }

        public Circle(Element fea)
        {
            if (fea.GeoType != FeatureType.Circle && fea.GeoType != FeatureType.Cylinder)
                throw (new Exception("geoType error"));
            this.xml_paras = fea.xml_paras;
            this.Identifier = fea.Identifier;
            this.geoType = fea.GeoType;
            //get length, direction and radius
            //length = Convert.ToDouble(xml_paras["Length"]);
            radius = Convert.ToDouble(xml_paras["Radius"]);
            inside = Convert.ToBoolean(xml_paras["InverseOrientation"]);

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
        /// <summary>
        /// the points base on the feature alignment
        /// </summary>
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
        /// <summary>
        /// the point base on the alignment
        /// </summary>
        public override List<MeasPoint> Alignment_Points
        {
            get
            {
                return measPoints;
            }
        }
        public double x
        {
            get
            {
                return Position[0];
            }
        }
        public double y
        {
            get
            {
                return Position[1];
            }
        }
        public double z
        {
            get
            {
                return Position[2];
            }
        }
        public double d
        {
            get
            {
                return radius * 2;
            }
        }
        public double r
        {
            get
            {
                return radius;
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
                        double u = k0 * (temp.y - y0) - j0 * (temp.z - z0);
                        double v = i0 * (temp.z - z0) - k0 * (temp.x - x0);
                        double w = j0 * (temp.x - x0) - i0 * (temp.y - y0);
                        _devs.Add(Math.Sqrt(u * u + v * v + w * w) / Math.Sqrt(i0 * i0 + j0 * j0 + k0 * k0) - radius);
                    }
                }
                return _devs;
            }
        }

        public double lead
        {
            get
            {
                return calcLead();
            }
        }

        protected virtual double calcLead()
        {
            List<double> z_value = new List<double>();
            foreach (MeasPoint p in transferedPoints)
            {
                z_value.Add(p.z - transferedPoints[0].z);
            }
            double _lead = Math.Round(((z_value.Average() * 2) / (round_nr / (2 * Math.PI))) * 4, 0) / 4.0;
            return _lead;
        }
    }
}
