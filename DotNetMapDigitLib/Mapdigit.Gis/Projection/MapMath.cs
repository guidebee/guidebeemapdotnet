//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 29SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using Mapdigit.Gis.Geometry;
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Projection
{
    internal class MapMath
    {

        public const double Halfpi = MathEx.Pi / 2.0;
        public const double Quarterpi = MathEx.Pi / 4.0;
        public const double Twopi = MathEx.Pi * 2.0;
        public const double Rtd = 180.0 / MathEx.Pi;
        public const double Dtr = MathEx.Pi / 180.0;
        public GeoBounds WorldBoundsRad =
                new GeoBounds(-MathEx.Pi, -MathEx.Pi / 2, MathEx.Pi * 2, MathEx.Pi);
        public GeoBounds WorldBounds
                = new GeoBounds(-180, -90, 360, 180);

        /**
         * Degree versions of trigonometric functions
         */
        public static double Sind(double v)
        {
            return MathEx.Sin(v * Dtr);
        }

        public static double Cosd(double v)
        {
            return MathEx.Cos(v * Dtr);
        }

        public static double Tand(double v)
        {
            return MathEx.Tan(v * Dtr);
        }

        public static double Asind(double v)
        {
            return MathEx.Asin(v) * Rtd;
        }

        public static double Acosd(double v)
        {
            return MathEx.Acos(v) * Rtd;
        }

        public static double Atand(double v)
        {
            return MathEx.Atan(v) * Rtd;
        }

        public static double Atan2D(double y, double x)
        {
            return MathEx.Atan2(y, x) * Rtd;
        }

        public static double Asin(double v)
        {
            if (MathEx.Abs(v) > 1.0)
            {
                return v < 0.0 ? -MathEx.Pi / 2 : MathEx.Pi / 2;
            }
            return MathEx.Asin(v);
        }

        public static double Acos(double v)
        {
            if (MathEx.Abs(v) > 1.0)
            {
                return v < 0.0 ? MathEx.Pi : 0.0;
            }
            return MathEx.Acos(v);
        }

        public static double Sqrt(double v)
        {
            return v < 0.0 ? 0.0 : MathEx.Sqrt(v);
        }

        public static double Distance(double dx, double dy)
        {
            return MathEx.Sqrt(dx * dx + dy * dy);
        }

        public static double Distance(GeoPoint a, GeoPoint b)
        {
            return Distance(a.X - b.X, a.Y - b.Y);
        }

        public static double Hypot(double x, double y)
        {
            if (x < 0.0)
            {
                x = -x;
            }
            else if (x == 0.0)
            {
                return y < 0.0 ? -y : y;
            }
            if (y < 0.0)
            {
                y = -y;
            }
            else if (y == 0.0)
            {
                return x;
            }
            if (x < y)
            {
                x /= y;
                return y * MathEx.Sqrt(1.0 + x * x);
            }
            y /= x;
            return x * MathEx.Sqrt(1.0 + y * y);
        }

        public static double Atan2(double y, double x)
        {
            return MathEx.Atan2(y, x);
        }

        public static double Trunc(double v)
        {
            return v < 0.0 ? MathEx.Ceil(v) : MathEx.Floor(v);
        }

        public static double Frac(double v)
        {
            return v - Trunc(v);
        }

        public static double DegToRad(double v)
        {
            return v * MathEx.Pi / 180.0;
        }

        public static double RadToDeg(double v)
        {
            return v * 180.0 / MathEx.Pi;
        }

        // For negative angles, d should be negative, m & s positive.
        public static double DmsToRad(double d, double m, double s)
        {
            if (d >= 0)
            {
                return (d + m / 60 + s / 3600) * MathEx.Pi / 180.0;
            }
            return (d - m / 60 - s / 3600) * MathEx.Pi / 180.0;
        }

        // For negative angles, d should be negative, m & s positive.
        public static double DmsToDeg(double d, double m, double s)
        {
            if (d >= 0)
            {
                return (d + m / 60 + s / 3600);
            }
            return (d - m / 60 - s / 3600);
        }

        public static double NormalizeLatitude(double angle)
        {
            if (Double.IsInfinity(angle) || Double.IsNaN(angle))
            {
                throw new ProjectionException("Infinite latitude");
            }
            while (angle > Halfpi)
            {
                angle -= MathEx.Pi;
            }
            while (angle < -Halfpi)
            {
                angle += MathEx.Pi;
            }
            return angle;
        }

        public static double NormalizeLongitude(double angle)
        {
            if (Double.IsInfinity(angle) || Double.IsNaN(angle))
            {
                throw new ProjectionException("Infinite longitude");
            }
            while (angle > MathEx.Pi)
            {
                angle -= Twopi;
            }
            while (angle < -MathEx.Pi)
            {
                angle += Twopi;
            }
            return angle;
        }

        public static double NormalizeAngle(double angle)
        {
            if (Double.IsInfinity(angle) || Double.IsNaN(angle))
            {
                throw new ProjectionException("Infinite angle");
            }
            while (angle > Twopi)
            {
                angle -= Twopi;
            }
            while (angle < 0)
            {
                angle += Twopi;
            }
            return angle;
        }


        public static double GreatCircleDistance(double lon1, double lat1,
                double lon2, double lat2)
        {
            double dlat = MathEx.Sin((lat2 - lat1) / 2);
            double dlon = MathEx.Sin((lon2 - lon1) / 2);
            double r = MathEx.Sqrt(dlat * dlat + MathEx.Cos(lat1)
                    * MathEx.Cos(lat2) * dlon * dlon);
            return 2.0 * MathEx.Asin(r);
        }

        public static double SphericalAzimuth(double lat0, double lon0,
                double lat, double lon)
        {
            double diff = lon - lon0;
            double coslat = MathEx.Cos(lat);

            return MathEx.Atan2(
                    coslat * MathEx.Sin(diff),
                    (MathEx.Cos(lat0) * MathEx.Sin(lat) -
                    MathEx.Sin(lat0) * coslat * MathEx.Cos(diff)));
        }

        public static bool SameSigns(double a, double b)
        {
            return a < 0 == b < 0;
        }

        public static bool SameSigns(int a, int b)
        {
            return a < 0 == b < 0;
        }

        public static double TakeSign(double a, double b)
        {
            a = MathEx.Abs(a);
            if (b < 0)
            {
                return -a;
            }
            return a;
        }

        public static int TakeSign(int a, int b)
        {
            a = MathEx.Abs(a);
            if (b < 0)
            {
                return -a;
            }
            return a;
        }
        public const int DontIntersect = 0;
        public const int DoIntersect = 1;
        public const int Collinear = 2;

        public static int IntersectSegments(GeoPoint aStart, GeoPoint aEnd,
                GeoPoint bStart, GeoPoint bEnd, GeoPoint p)
        {
            double a1 = aEnd.Y - aStart.Y;
            double b1 = aStart.X - aEnd.X;
            double c1 = aEnd.X * aStart.Y - aStart.X * aEnd.Y;
            double r3 = a1 * bStart.X + b1 * bStart.Y + c1;
            double r4 = a1 * bEnd.X + b1 * bEnd.Y + c1;

            if (r3 != 0 && r4 != 0 && SameSigns(r3, r4))
            {
                return DontIntersect;
            }

            double a2 = bEnd.Y - bStart.Y;
            double b2 = bStart.X - bEnd.X;
            double c2 = bEnd.X * bStart.Y - bStart.X * bEnd.Y;
            double r1 = a2 * aStart.X + b2 * aStart.Y + c2;
            double r2 = a2 * aEnd.X + b2 * aEnd.Y + c2;

            if (r1 != 0 && r2 != 0 && SameSigns(r1, r2))
            {
                return DontIntersect;
            }

            double denom = a1 * b2 - a2 * b1;
            if (denom == 0)
            {
                return Collinear;
            }

            double offset = denom < 0 ? -denom / 2 : denom / 2;

            double num = b1 * c2 - b2 * c1;
            p.X = (num < 0 ? num - offset : num + offset) / denom;

            num = a2 * c1 - a1 * c2;
            p.Y = (num < 0 ? num - offset : num + offset) / denom;

            return DoIntersect;
        }

        public static double Dot(GeoPoint a, GeoPoint b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static GeoPoint Perpendicular(GeoPoint a)
        {
            return new GeoPoint(-a.Y, a.X);
        }

        public static GeoPoint Add(GeoPoint a, GeoPoint b)
        {
            return new GeoPoint(a.X + b.X, a.Y + b.Y);
        }

        public static GeoPoint Subtract(GeoPoint a, GeoPoint b)
        {
            return new GeoPoint(a.X - b.X, a.Y - b.Y);
        }

        public static GeoPoint Multiply(GeoPoint a, GeoPoint b)
        {
            return new GeoPoint(a.X * b.X, a.Y * b.Y);
        }

        public static double Cross(GeoPoint a, GeoPoint b)
        {
            return a.X * b.Y - b.X * a.Y;
        }

        public static double Cross(double x1, double y1, double x2, double y2)
        {
            return x1 * y2 - x2 * y1;
        }

        public static void Normalize(GeoPoint a)
        {
            double d = Distance(a.X, a.Y);
            a.X /= d;
            a.Y /= d;
        }

        public static void Negate(GeoPoint a)
        {
            a.X = -a.X;
            a.Y = -a.Y;
        }

        public static double LongitudeDistance(double l1, double l2)
        {
            return MathEx.Min(
                    MathEx.Abs(l1 - l2),
                    ((l1 < 0) ? l1 + MathEx.Pi : MathEx.Pi - l1)
                    + ((l2 < 0) ? l2 + MathEx.Pi : MathEx.Pi - l2));
        }

        public static double GeocentricLatitude(double lat, double flatness)
        {
            double f = 1.0 - flatness;
            return MathEx.Atan((f * f) * MathEx.Tan(lat));
        }

        public static double GeographicLatitude(double lat, double flatness)
        {
            double f = 1.0 - flatness;
            return MathEx.Atan(MathEx.Tan(lat) / (f * f));
        }

        public static double Tsfn(double phi, double sinphi, double e)
        {
            sinphi *= e;
            return (MathEx.Tan(.5 * (Halfpi - phi)) /
                    MathEx.Pow((1.0 - sinphi) / (1.0 + sinphi), 0.5 * e));
        }

        public static double Msfn(double sinphi, double cosphi, double es)
        {
            return cosphi / MathEx.Sqrt(1.0 - es * sinphi * sinphi);
        }
        private const int NIter = 15;

        public static double Phi2(double ts, double e)
        {
            double dphi;

            double eccnth = .5 * e;
            double phi = Halfpi - 2.0 * MathEx.Atan(ts);
            int i = NIter;
            do
            {
                double con = e * MathEx.Sin(phi);
                dphi = Halfpi - 2.0 * MathEx.Atan(ts * MathEx.Pow((1.0 - con)
                        / (1.0 + con), eccnth)) - phi;
                phi += dphi;
            } while (MathEx.Abs(dphi) > 1e-10 && --i != 0);
            if (i <= 0)
            {
                throw new ProjectionException();
            }
            return phi;
        }
        private const double C00 = 1.0;
        private const double C02 = .25;
        private const double C04 = .046875;
        private const double C06 = .01953125;
        private const double C08 = .01068115234375;
        private const double C22 = .75;
        private const double C44 = .46875;
        private const double C46 = .01302083333333333333;
        private const double C48 = .00712076822916666666;
        private const double C66 = .36458333333333333333;
        private const double C68 = .00569661458333333333;
        private const double C88 = .3076171875;
        private const int MaxIter = 10;

        public static double[] Enfn(double es)
        {
            double t;
            double[] en = new double[5];
            en[0] = C00 - es * (C02 + es * (C04 + es * (C06 + es * C08)));
            en[1] = es * (C22 - es * (C04 + es * (C06 + es * C08)));
            en[2] = (t = es * es) * (C44 - es * (C46 + es * C48));
            en[3] = (t *= es) * (C66 - es * C68);
            en[4] = t * es * C88;
            return en;
        }

        public static double Mlfn(double phi, double sphi, double cphi, double[] en)
        {
            cphi *= sphi;
            sphi *= sphi;
            return en[0] * phi - cphi * (en[1] + sphi * (en[2]
                    + sphi * (en[3] + sphi * en[4])));
        }

        public static double InvMlfn(double arg, double es, double[] en)
        {
            double k = 1.0 / (1.0 - es);

            double phi = arg;
            for (int i = MaxIter; i != 0; i--)
            {
                double s = MathEx.Sin(phi);
                double t = 1.0 - es * s * s;
                phi -= t = (Mlfn(phi, s, MathEx.Cos(phi), en) - arg)
                        * (t * MathEx.Sqrt(t)) * k;
                if (MathEx.Abs(t) < 1e-11)
                {
                    return phi;
                }
            }
            return phi;
        }
        private const double P00 = .33333333333333333333;
        private const double P01 = .17222222222222222222;
        private const double P02 = .10257936507936507936;
        private const double P10 = .06388888888888888888;
        private const double P11 = .06640211640211640211;
        private const double P20 = .01641501294219154443;

        public static double[] Authset(double es)
        {
            double[] apa = new double[3];
            apa[0] = es * P00;
            double t = es * es;
            apa[0] += t * P01;
            apa[1] = t * P10;
            t *= es;
            apa[0] += t * P02;
            apa[1] += t * P11;
            apa[2] = t * P20;
            return apa;
        }

        public static double Authlat(double beta, double[] apa)
        {
            double t = beta + beta;
            return (beta + apa[0] * MathEx.Sin(t) + apa[1] * MathEx.Sin(t + t)
                    + apa[2] * MathEx.Sin(t + t + t));
        }

        public static double Qsfn(double sinphi, double e, double oneEs)
        {
            if (e >= 1.0e-7)
            {
                double con = e * sinphi;
                return (oneEs * (sinphi / (1.0 - con * con) -
                        (.5 / e) * MathEx.Log((1.0 - con) / (1.0 + con))));
            }
            return (sinphi + sinphi);
        }

        /*
         * Java translation of "Nice Numbers for Graph Labels"
         * by Paul Heckbert
         * from "Graphics Gems", Academic Press, 1990
         */
        public static double NiceNumber(double x, bool round)
        {
            double nf;				/* nice, rounded fraction */

            int expv = (int)MathEx.Floor(MathEx.Log(x) / MathEx.Log(10));
            double f = x / MathEx.Pow(10.0, expv);
            if (round)
            {
                if (f < 1.5)
                {
                    nf = 1.0;
                }
                else if (f < 3.0)
                {
                    nf = 2.0;
                }
                else if (f < 7.0)
                {
                    nf = 5.0;
                }
                else
                {
                    nf = 10.0;
                }
            }
            else if (f <= 1.0)
            {
                nf = 1.0;
            }
            else if (f <= 2.0)
            {
                nf = 2.0;
            }
            else if (f <= 5.0)
            {
                nf = 5.0;
            }
            else
            {
                nf = 10.0;
            }
            return nf * MathEx.Pow(10.0, expv);
        }
    }


}
