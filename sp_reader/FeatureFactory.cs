using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPInterface
{
    public static class FeatureFactory
    {
        public static Feature createFeature(XmlNode node)
        {
            Feature f = new Feature(node);
            return createFeature(f);
        }
        public static Feature createFeature(Feature f)
        {
            switch (f.geoType)
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
                default:
                    throw new Exception("unkown type");
            }
        }
    }
}
