using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SPInterface.Core;

namespace SPInterface
{
    public static class FeatureFactory
    {
        public static Element createFeature(XmlNode node)
        {
            Element f = new Element(node);
            return createFeature(f);
        }
        public static Element createFeature(Element f)
        {
            switch (f.GeoType)
            {
                case FeatureType.Circle:
                    return new Circle(f);
                case FeatureType.Cylinder:
                    return new Cylinder(f);
                case FeatureType.Plane:
                    return new Plane(f);
                case FeatureType.Point:
                    return new Point(f);
                case FeatureType.Curve:
                    return new Curve(f);
                case FeatureType.Cone:
                    throw new Exception("didn't implement");
                case FeatureType.Line:
                    return new Line(f);
                case FeatureType.SpacePoint:
                    return new Point(f);
                default:
                    throw new Exception("unkown type");
            }
        }
    }
}
