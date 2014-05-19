using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPInterface
{
    public class CircleSection : List<MeasPoint>
    {
        const double max_distance = 1.0d;
        const double max_angleDev = (20.0d * Math.PI) / 180.0d;
        List<double> _singleDev_angles;
        List<double> _commu_angles;
        SectionRotation direction
        {
            get;
            set;
        }

        internal bool tryParser(MeasPoint measPoint)
        {
            if (this.Count() == 0)
            {
                this.Add(measPoint);
                return true;
            }
            if (getDistance(this.Last(), measPoint) > max_distance)
            {
                if (this.Count() == 1)
                {
                    direction = SectionRotation.singlepoint;
                }
                return false;
            }

            if (judgeDirection(measPoint))
            {
                this.Add(measPoint);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool judgeDirection(MeasPoint measPoint)
        {
            if (this.Count() == 0)
                return true;
            if (this.Count() == 1)
            {
                direction = getDirection(this[0], measPoint);
                return true;
            }

            if (getDirection(this.Last(), measPoint) == direction)
                return true;
            else
                return false;
        }

        static private SectionRotation getDirection(MeasPoint measPoint1, MeasPoint measPoint2)
        {
            double angle1 = Math.Atan2(measPoint1.y, measPoint1.x);
            double angle2 = Math.Atan2(measPoint2.y, measPoint2.x);
            SectionRotation result;
            if (angle1 > 0)
            {
                if (angle2 > 0)
                {
                    if (angle2 > angle1)
                        result = SectionRotation.anticlockwise;
                    else
                        result = SectionRotation.clockwise;
                }
                else
                {
                    if (angle1 > 0.9 * Math.PI && angle2 < -0.9 * Math.PI)
                        result = SectionRotation.anticlockwise;
                    else if (angle1 < 0.1 * Math.PI && angle2 > -0.1 * Math.PI)
                        result = SectionRotation.clockwise;
                    else
                        throw new IndexOutOfRangeException("the angle different is too much");
                }
            }
            else
            {
                if (angle2 < 0)
                {
                    if (angle2 > angle1)
                        result = SectionRotation.anticlockwise;
                    else
                        result = SectionRotation.clockwise;
                }
                else
                {
                    if (angle1 > -0.1 * Math.PI && angle2 < 0.1 * Math.PI)
                        result = SectionRotation.anticlockwise;
                    else if (angle1 < -0.9 * Math.PI && angle2 > 0.9 * Math.PI)
                        result = SectionRotation.clockwise;
                    else
                        throw new IndexOutOfRangeException("the angle different is too much");
                }
            }
            return result;
        }

        private double getDistance(MeasPoint measPoint1, MeasPoint measPoint2)
        {
            double x1, y1, z1, x2, y2, z2;
            x1 = measPoint1.x;
            y1 = measPoint1.y;
            z1 = measPoint1.z;
            x2 = measPoint2.x;
            y2 = measPoint2.y;
            z2 = measPoint2.z;
            double result = Math.Sqrt(
                Math.Pow(x1 - x2, 2) +
                Math.Pow(y1 - y2, 2) +
                Math.Pow(z1 - z2, 2)
                );
            return result;
        }



        internal bool isOverLap(CircleSection cs)
        {
            if (cs.direction != direction)
                return false;
            if (direction == SectionRotation.anticlockwise)
            {
                if (isCloseEnough(this.startAngle, cs.endAngle))
                {
                    if (
                        isMoreThan(cs.endAngle, this.startAngle) 
                        || isSoClose(cs.endAngle, this.startAngle)
                        )
                    {
                        if (isCloseEnough(this.startAngle, cs.startAngle)
                            &&
                            isMoreThan(cs.startAngle, this.startAngle)
                            )
                            return false;
                        else
                            return true;
                    }
                }
                if (isCloseEnough(this.endAngle, cs.startAngle))
                {
                    if (
                        isMoreThan(this.endAngle, cs.startAngle)
                        || isSoClose(this.endAngle, cs.startAngle)
                       )
                    {
                        if (isCloseEnough(this.startAngle, cs.startAngle)
                            &&
                            isMoreThan(this.startAngle, cs.startAngle)
                            )
                            return false;
                        else
                            return true;
                    }
                }
            }
            if (direction == SectionRotation.clockwise)
            {
                if (isCloseEnough(this.startAngle, cs.endAngle))
                {
                    if (isLessThan(cs.endAngle, this.startAngle))
                    {
                        if (isCloseEnough(this.startAngle, cs.startAngle)
                            &&
                            isLessThan(cs.startAngle, this.startAngle)
                            )
                            return false;
                        else
                            return true;
                    }
                }
                if (isCloseEnough(this.endAngle, cs.startAngle))
                {
                    if (isLessThan(this.endAngle, cs.startAngle))
                    {
                        if (isCloseEnough(this.startAngle, cs.startAngle)
                            &&
                            isLessThan(this.startAngle, cs.startAngle)
                            )
                            return false;
                        else
                            return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// compare if d1 is a smaller angle than d2
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        private bool isLessThan(double d1, double d2)
        {
            return isMoreThan(d2, d1);
        }
        /// <summary>
        /// compare if d1 is a bigger angle than d2
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        private bool isMoreThan(double d1, double d2)
        {
            double twoPI = 2 * Math.PI;

            double a1 = (d1 % twoPI + twoPI) % twoPI;
            double a2 = (d2 % twoPI + twoPI) % twoPI;

            //this mean no overlapping but very close
            if (Math.Abs(a1 - a2) < 0.01)
                return true;

            if (a1 - a2 > 0)
            {
                if (a1 - a2 < Math.PI)
                    return true;
                else
                    return false;
            }
            else
            {
                if (a2 - a1 < Math.PI)
                    return false;
                else
                    return true;
            }
        }
        /// <summary>
        /// check if d1 is very close to d2
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        private bool isSoClose(double d1, double d2)
        {
            double twoPI = 2 * Math.PI;

            double a1 = (d1 % twoPI + twoPI) % twoPI;
            double a2 = (d2 % twoPI + twoPI) % twoPI;
            return Math.Abs(a1 - a2) < 0.01;
        }
        /// <summary>
        /// compare if two angle is close enough
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        private bool isCloseEnough(double d1, double d2)
        {
            double twoPI = 2 * Math.PI;
            double a1 = (d1 % twoPI + twoPI) % twoPI;
            double a2 = (d2 % twoPI + twoPI) % twoPI;

            if (Math.Abs(a1 - a2) < max_angleDev)
                return true;
            if ((Math.PI * 2) - Math.Abs(a1 - a2) < max_angleDev)
                return true;

            return false;
        }
        /// <summary>
        /// Attention: use this function only after
        /// you check it with function : isOverLap
        /// </summary>
        /// <param name="circleSection"></param>
        /// <returns></returns>
        internal CircleSection merge(CircleSection cs)
        {
            if (!isOverLap(cs))
                throw new Exception("didn't over lap");
            CircleSection s1, s2;
            if (direction == SectionRotation.anticlockwise)
            {
                if (isCloseEnough(this.startAngle, cs.endAngle)
                    &&
                    isMoreThan(cs.endAngle, this.startAngle)
                    )
                {
                    s1 = cs;
                    s2 = this;
                }
                else
                {
                    s1 = this;
                    s2 = cs;
                }
            }
            else
            {
                if (isCloseEnough(this.startAngle, cs.endAngle)
                   &&
                   isLessThan(cs.endAngle, this.startAngle)
                   )
                {
                    s1 = cs;
                    s2 = this;
                }
                else
                {
                    s1 = this;
                    s2 = cs;
                }
            }
            CircleSection result = new CircleSection();
            result.direction = direction;
            foreach (MeasPoint p in s1)
            {
                result.Add(p);
            }
            for (int i = 0; i < s2.Count(); i++)
            {
                double a1 = Math.Atan2(result.Last().y, result.Last().x);
                double a2 = Math.Atan2(s2[i].y, s2[i].x);

                if (isLessThan((int)direction * a2, (int)direction * a1))
                    continue;
                else
                    result.Add(s2[i]);
            }
            return result;
        }

        public double startAngle
        {
            get
            {
                double angle = Math.Atan2(this[0].y, this[0].x);
                if (angle < 0)
                    angle += 2 * Math.PI;
                return angle;
            }
        }

        public double rangeAngle
        {
            get
            {
                double result = 0;
                //for (int i = 0; i < SingleDev_angles.Count(); ++i)
                //{
                //    result += SingleDev_angles[i];
                //}
                result = Commu_angles.Last();
                return result;
            }
        }
        public List<double> Commu_angles
        {
            get
            {
                if (_commu_angles == null)
                {
                    _commu_angles = new List<double>();
                    _commu_angles.Add(0.0d);
                    for (int i = 1; i < SingleDev_angles.Count(); ++i)
                    {
                        _commu_angles.Add(_commu_angles.Last() + SingleDev_angles[i]);
                    }
                }
                return _commu_angles;
            }
        }
        public List<double> SingleDev_angles
        {
            get
            {
                if (_singleDev_angles == null)
                {
                    _singleDev_angles = new List<double>();
                    _singleDev_angles.Add(0.0d);
                    double angle1 = Math.Atan2(this[0].y, this[0].x);

                    double next_angle, last_angle;
                    last_angle = angle1;
                    for (int i = 1; i < this.Count(); ++i)
                    {
                        double this_angle = 0;
                        next_angle = Math.Atan2(this[i].y, this[i].x);
                        if (Math.Abs(next_angle - last_angle) > Math.PI)
                        {
                            if (direction == SectionRotation.anticlockwise)
                            {
                                this_angle = next_angle + 2 * Math.PI - last_angle;
                            }
                            else
                            {
                                this_angle = next_angle - 2 * Math.PI - last_angle;
                            }
                        }
                        else
                        {
                            this_angle = next_angle - last_angle;
                        }

                        last_angle = next_angle;
                        _singleDev_angles.Add(this_angle);

                    }
                }
                return _singleDev_angles;
            }
        }

        public double endAngle
        {
            get
            {
                return startAngle + rangeAngle;
            }
        }
        public override string ToString()
        {
            string result = "";
            result += " Start Angle : " + (startAngle * 180.0d / Math.PI).ToString("F4");
            //result += "\r\n";
            result += " Angle Range : " + (rangeAngle * 180.0d / Math.PI).ToString("F4");
            return result;
        }
    }

    enum SectionRotation : int
    {
        clockwise = -1,
        anticlockwise = 1,
        singlepoint = 0
    }
}
