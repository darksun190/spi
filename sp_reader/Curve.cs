using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using System.Xml;

namespace SPInterface
{
    public class Curve : Feature
    {
        public readonly string fileNomPoints;
        public readonly string fileMeasPoints;
        public readonly string fileActPoints;
        public List<FullPoint> results_points;
        List<NomPoint> nomPoints;
        List<ActPoint> actPoints;
        List<ActPoint> actMaskedPoints;
        CurvePara curve_paras;

        public Curve(Feature fea)
        {
            if (fea.geoType != FeatureType.Curve)
                throw (new Exception("geoType error"));
            this.xml_paras = fea.xml_paras;
            this.identifier = fea.identifier;
            this.geoType = fea.geoType;

            actPoints = new List<ActPoint>();
            actMaskedPoints = new List<ActPoint>();
            FileInfo act_fi = new FileInfo(xml_paras["ActPoints"]);
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

            nomPoints = new List<NomPoint>();
            FileInfo nom_fi = new FileInfo(xml_paras["NomPoints"]);
            fileNomPoints = nom_fi.FullName;
            StreamReader nom_sr = new StreamReader(fileNomPoints);


            while ((buf = nom_sr.ReadLine()) != null)
            {
                NomPoint temp_nom_point = new NomPoint(buf);

                nomPoints.Add(temp_nom_point);
            }

            measPoints = new List<MeasPoint>();

            measMaskedPoints = new List<MeasPoint>();
            FileInfo meas_fi = new FileInfo(xml_paras["MeasPoints"]);
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

            results_points = new List<FullPoint>();
            for (int i = 0; i < nomPoints.Count; ++i)
            {
                results_points.Add(new FullPoint(nomPoints[i], actPoints[i]));
            }

            string curve_xmlpath = new FileInfo(xml_paras["CurveParam"]).FullName;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(curve_xmlpath);
            XmlNode xmlnode = xmldoc.SelectSingleNode("CurveParameters");
            curve_paras = new CurvePara(xmlnode);
        }
    }
}
