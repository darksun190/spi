using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace SPInterface
{
     public class Element1
    {
        public readonly string geoType;
        public readonly string identifier;
        public readonly string fileNomPoints;
        public readonly string fileMeasPoints;
        public readonly string fileActPoints;
        public CurvePara paras;
        public readonly double length;
        public readonly double width;
        List<NomPoint> nomPoints;
        List<ActPoint> actPoints;
        List<ActPoint> actMaskedPoints;
        List<MeasPoint> measPoints;
        List<MeasPoint> measMaskedPoints;

        public List<NomPoint> getNominalPoints()
        {
            return nomPoints;
        }
        public List<MeasPoint> getMeasPoints()
        {
            return measPoints;
        }

        public List<MeasPoint> getMeasMaskedPointsRemoved()
        {
            return measMaskedPoints;
        }
        public List<ActPoint> getActualPoints()
        {
            return actPoints;
        }
        public List<ActPoint> getActualMaskedPointsRemoved()
        {
            return actMaskedPoints;
        }
        public readonly double[] pos;
        public readonly double[] vec;
        public readonly double height;
        public readonly double radius;
        public readonly bool isOutside;

        public Element1()
        {
        }

        public Element1(XmlNode node)
        {
            geoType = node.Attributes["GeoType"].Value;
            identifier = node.Attributes["Identifier"].Value;

            //construct for type cylinder & circle
            if (geoType.Equals("Plane", StringComparison.CurrentCultureIgnoreCase))
            {
                pos = new double[3];
                vec = new double[3];
                {
                    string buf = node.Attributes["Position"].Value;
                    string[] spli_str = buf.Split(' ');
                    for (int i = 0; i < 3; ++i)
                    {
                        pos[i] = Convert.ToDouble(spli_str[i]);
                    }
                }
                {
                    string buf = node.Attributes["Vector"].Value;
                    string[] spli_str = buf.Split(' ');
                    for (int i = 0; i < 3; ++i)
                    {
                        vec[i] = Convert.ToDouble(spli_str[i]);
                    }
                }
                length = Convert.ToDouble(node.Attributes["Length"].Value);
                width = Convert.ToDouble(node.Attributes["Width"].Value);
              
                FileInfo meas_fi = new FileInfo(node.Attributes["Points"].Value);
                fileMeasPoints = meas_fi.FullName;
                StreamReader meas_sr = new StreamReader(fileMeasPoints);
                string line;
                measPoints = new List<MeasPoint>();
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
            } 
            if (geoType.Equals("Cylinder", StringComparison.CurrentCultureIgnoreCase))
            {
                pos = new double[3];
                vec = new double[3];
                {
                    string buf = node.Attributes["Position"].Value;
                    string[] spli_str = buf.Split(' ');
                    for (int i = 0; i < 3; ++i)
                    {
                        pos[i] = Convert.ToDouble(spli_str[i]);
                    }
                }
                {
                    string buf = node.Attributes["Vector"].Value;
                    string[] spli_str = buf.Split(' ');
                    for (int i = 0; i < 3; ++i)
                    {
                        vec[i] = Convert.ToDouble(spli_str[i]);
                    }
                }
                height = Convert.ToDouble(node.Attributes["Height"].Value);
                radius = Convert.ToDouble(node.Attributes["Radius"].Value);
                isOutside = Convert.ToBoolean(node.Attributes["InverseOrientation"].Value);
                FileInfo meas_fi = new FileInfo(node.Attributes["Points"].Value);
                fileMeasPoints = meas_fi.FullName;
                StreamReader meas_sr = new StreamReader(fileMeasPoints);
                string line;
                measPoints = new List<MeasPoint>();

                while ((line = meas_sr.ReadLine()) != null)
                {
                    MeasPoint temp_Meas_point = new MeasPoint(line,false);
                    if (temp_Meas_point.status == 0)
                        measPoints.Add(temp_Meas_point);
                    else
                        measMaskedPoints.Add(temp_Meas_point);
                }
            }
            if (geoType.Equals("Circle", StringComparison.CurrentCultureIgnoreCase))
            {
                pos = new double[3];
                vec = new double[3];
                {
                    string buf = node.Attributes["Position"].Value;
                    string[] spli_str = buf.Split(' ');
                    for (int i = 0; i < 3; ++i)
                    {
                        pos[i] = Convert.ToDouble(spli_str[i]);
                    }
                }
                {
                    string buf = node.Attributes["Vector"].Value;
                    string[] spli_str = buf.Split(' ');
                    for (int i = 0; i < 3; ++i)
                    {
                        vec[i] = Convert.ToDouble(spli_str[i]);
                    }
                }
                length = Convert.ToDouble(node.Attributes["Length"].Value);
                radius = Convert.ToDouble(node.Attributes["Radius"].Value);
                isOutside = Convert.ToBoolean(node.Attributes["InverseOrientation"].Value);
                FileInfo meas_fi = new FileInfo(node.Attributes["Points"].Value);
                fileMeasPoints = meas_fi.FullName;
                StreamReader meas_sr = new StreamReader(fileMeasPoints);
                string line;
                measPoints = new List<MeasPoint>();

                while ((line = meas_sr.ReadLine()) != null)
                {
                    MeasPoint temp_Meas_point = new MeasPoint(line, false);
                    if (temp_Meas_point.status == 0)
                        measPoints.Add(temp_Meas_point);
                    else
                        measMaskedPoints.Add(temp_Meas_point);
                }
            }
            //construct for type curve
            if (geoType.Equals("curve", StringComparison.CurrentCultureIgnoreCase))
            {
                //constructor for actpoints;
                actPoints = new List<ActPoint>();
                actMaskedPoints = new List<ActPoint>();
                FileInfo act_fi = new FileInfo(node.Attributes["ActPoints"].Value);
                fileActPoints = act_fi.FullName;
                StreamReader act_sr = new StreamReader(fileActPoints);
                String buf;

                while ((buf = act_sr.ReadLine()) != null)
                {
                    ActPoint temp_act_point = new ActPoint(buf);
                    if (temp_act_point.status == 0)
                        actPoints.Add(temp_act_point);
                    else
                        actMaskedPoints.Add(temp_act_point);
                }
                //constructor for nompoints;
                nomPoints = new List<NomPoint>();
                FileInfo nom_fi = new FileInfo(node.Attributes["NomPoints"].Value);
                fileNomPoints = nom_fi.FullName;
                StreamReader nom_sr = new StreamReader(fileNomPoints);


                while ((buf = nom_sr.ReadLine()) != null)
                {
                    NomPoint temp_nom_point = new NomPoint(buf);

                    nomPoints.Add(temp_nom_point);
                }
                measPoints = new List<MeasPoint>();

                measMaskedPoints = new List<MeasPoint>();
                FileInfo meas_fi = new FileInfo(node.Attributes["MeasPoints"].Value);
                fileMeasPoints = meas_fi.FullName;
                StreamReader meas_sr = new StreamReader(fileMeasPoints);


                while ((buf = meas_sr.ReadLine()) != null)
                {
                    MeasPoint temp_Meas_point = new MeasPoint(buf);
                    if (temp_Meas_point.status == 0)
                        measPoints.Add(temp_Meas_point);
                    else
                        measMaskedPoints.Add(temp_Meas_point);
                }
                //Console.WriteLine("{0} , {1} , {2} , {3} , {4}",nomPoints.Count,actPoints.Count,measPoints.Count,
                //    actMaskedPoints.Count, measMaskedPoints.Count);

                string curve_xmlpath = new FileInfo(node.Attributes["CurveParam"].Value).FullName;
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(curve_xmlpath);
                XmlNode xmlnode = xmldoc.SelectSingleNode("CurveParameters");
                paras = new CurvePara(xmlnode);
            }
        }
    }
}
