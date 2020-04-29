using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPInterface.Feature
{
    public class FeatureType
    {
        public enum Type
        {
            Point,
            Circle,
            Cylinder,
            Cone,
            Plane,
            Curve,
            Unknown,
            Line,
            Line3d,
            SpacePoint
        }
        internal static Type getType(string type)
        {
            switch (type.ToLower())
            {
                case "cylinder":
                    return Type.Cylinder;
                case "circle":
                    return Type.Circle;
                case "spacepoint":
                case "point":
                    return Type.Point;
                case "curve":
                    return Type.Curve;
                case "plane":
                    return Type.Plane;
                case "cone":
                    return Type.Cone;
                case "line":
                    return Type.Line;
                case "line3d":
                    return Type.Line3d;
                default:
                    return Type.Unknown;
            }
        }
    }
}
