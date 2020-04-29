﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using SPInterface.Core;

namespace SPInterface.Feature
{
    public class Cylinder : Circle
    {

        public double Height { get; set; }
        public Cylinder(Element element)
            :base(element)
        {

            if (element.GeoType != FeatureType.Type.Cylinder)
                throw (new Exception("geoType error"));

            Height = Convert.ToDouble(Parameters["Height"]);

        }

    }
}
