//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 27SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Geometry
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The GreateCircleCalculator compute true course and distance
    /// between points.
    /// </summary>
    internal class GreateCircleCalculator
    {

        /**
         * Sphere model.
         */
        public const int EarthModelSphere = 1;
        /**
         * WGS84 model. radius=6378.137km
         */
        public const int EarthModelWgs84 = 2;
        /**
         * NAD27 model. radius=6378.2064km
         */
        public const int EarthModelNad27 = 3;
        /**
         * international model. radius=6378.388km.
         */
        public const int EarthModelInternational = 4;
        /**
         * krasovsky model. radius=6378.245km.
         */
        public const int EarthModelKrasovsky = 5;
        /**
         * bessel model. radius=6377.397155km.
         */
        public const int EarthModelBessel = 6;
        /**
         * WGS72 model. radius=6378.135km.
         */
        public const int EarthModelWgs72 = 7;
        /**
         * WGS66 model. radius=6378.145.
         */
        public const int EarthModelWgs66 = 8;
        /**
         * FAI sphere. radius=6371.0km.
         */
        public const int EarthModelFaiSphere = 9;

        //public const int EARTH_MODEL_USER=0;
        /**
         * in nautical mile.
         */
        public const int UnitNm = 1;
        /**
         * in Kilometers.
         */
        public const int UnitKm = 2;
        private readonly int _currentEarthMode = 2;
        private readonly int _currentUnit = 2;
        private static readonly double[][] EarthModel = new[]{
                new[] {0.0, 0.0},
                new[] {180 * 60 / MathEx.Pi, double.PositiveInfinity},
                new[] {6378.137 / 1.852, 298.257223563},
                new[] {6378.2064 / 1.852, 294.9786982138},
                new[] {6378.388 / 1.852, 297.0},
                new[] {6378.245 / 1.852, 298.3},
                new[] {6377.397155 / 1.852, 299.1528},
                new[] {6378.135 / 1.852, 298.26},
                new[] {6378.145 / 1.852, 298.25},
                new[] {6371.0 / 1.852, 1000000000.0}
    };

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GreateCircleCalculator"/> class.
        /// </summary>
        /// <param name="earthModel">the model of the earth</param>
        /// <param name="unit">Km or mile, default is km</param>
        public GreateCircleCalculator(int earthModel, int unit)
        {
            _currentEarthMode = earthModel % 10;
            _currentUnit = unit;
            if (unit > 2 || unit < 1)
            {
                _currentUnit = UnitKm;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculate the distance between two point in great circle.
        /// </summary>
        /// <param name="pt1"> the first location.</param>
        /// <param name="pt2">the second location..</param>
        /// <returns>the distance between the two points.</returns>
        public double CalculateDistance(GeoLatLng pt1, GeoLatLng pt2)
        {
            CrsdistResult ret = ComputeFormCd(pt1, pt2);
            return ret._d;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Calculate the distance between two point in great circle.
        /// </summary>
        /// <param name="pt1">the first location.</param>
        /// <param name="pt2">the second location.</param>
        /// <returns>the distance and course between the 2 point</returns>
        internal GeoLatLng CalculateDistanceAndCourse(GeoLatLng pt1, GeoLatLng pt2)
        {
            CrsdistResult ret = ComputeFormCd(pt1, pt2);
            GeoLatLng ret1 = new GeoLatLng(ret._crs12, ret._d);
            return ret1;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get a location with given distance ,direction of one point.
        /// </summary>
        /// <param name="pt1">the first location.</param>
        /// <param name="distance">the distance to the first point.</param>
        /// <param name="direction">the direction to the first point.</param>
        /// <returns>the second point</returns>
        public GeoLatLng GetLocationWithDistance(GeoLatLng pt1, double distance, double direction)
        {
            return ComputeFormDir(pt1, distance, direction);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Computes the form dir.
        /// </summary>
        /// <param name="pt1">the first location.</param>
        /// <param name="distance">The distance.</param>
        /// <param name="crs">The CRS.</param>
        /// <returns></returns>
        private GeoLatLng ComputeFormDir(GeoLatLng pt1, double distance, double crs)
        {
            //get select  values
            double lat2, lon2;
            /* Input and validate data */

            double lat1 = pt1.LatRadians();
            double lon1 = -pt1.LngRadians();

            double d12 = distance;
            double dc = _currentUnit == UnitNm ? 1 : 1.852;
            d12 /= dc;
            double crs12 = crs * MathEx.Pi / 180.0;
            if (_currentEarthMode == EarthModelSphere)
            {
                // spherical code
                d12 /= (180 * 60 / MathEx.Pi);  // in radians
                DirectResult cd = Direct(lat1, lon1, crs12, d12);
                lat2 = cd._lat * (180 / MathEx.Pi);
                lon2 = cd._lon * (180 / MathEx.Pi);
            }
            else
            {
                // elliptic code// ellipse uses East negative
                DirectResult cde = DirectEll(lat1, -lon1, crs12, d12, _currentEarthMode);
                lat2 = cde._lat * (180 / MathEx.Pi);
                lon2 = -cde._lon * (180 / MathEx.Pi);// ellipse uses East negative
            }
            double retLat = lat2;
            double retLon = -lon2;
            return new GeoLatLng(retLat, retLon);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Computes the form cd.
        /// </summary>
        /// <param name="pt1">The PT1.</param>
        /// <param name="pt2">The PT2.</param>
        /// <returns></returns>
        private CrsdistResult ComputeFormCd(GeoLatLng pt1, GeoLatLng pt2)
        {
            double d, crs12, crs21;
            CrsdistResult outValue = new CrsdistResult();
            double lat1 = pt1.LatRadians();
            double lat2 = pt2.LatRadians();
            double lon1 = -pt1.LngRadians();
            double lon2 = -pt2.LngRadians();
            double dc = _currentUnit == UnitNm ? 1 : 1.852;

            if (_currentEarthMode == EarthModelSphere)
            {
                // spherical code// compute crs and Distance
                CrsdistResult cd = Crsdist(lat1, lon1, lat2, lon2);
                crs12 = cd._crs12 * (180 / MathEx.Pi);
                crs21 = cd._crs21 * (180 / MathEx.Pi);
                d = cd._d * (180 / MathEx.Pi) * 60 * dc; // go to physical units
            }
            else
            {
                // elliptic code // ellipse uses East negative
                CrsdistResult cde = CrsdistEll(lat1, -lon1, lat2, -lon2, _currentEarthMode);
                crs12 = cde._crs12 * (180 / MathEx.Pi);
                crs21 = cde._crs21 * (180 / MathEx.Pi);
                d = cde._d * dc; // go to physical units
            }
            outValue._crs12 = crs12;
            outValue._crs21 = crs21;
            outValue._d = d;
            return outValue;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Acosfs the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        private static double Acosf(double x)
        { /* protect against rounding error on input argument */
            if (MathEx.Abs(x) > 1)
            {
                x /= MathEx.Abs(x);
            }
            return MathEx.Acos(x);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Atan2s the specified y.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        private static double Atan2(double y, double x)
        {
            double outValue = 0;
            if (x < 0)
            {
                outValue = MathEx.Atan(y / x) + MathEx.Pi;
            }
            if ((x > 0) && (y >= 0))
            {
                outValue = MathEx.Atan(y / x);
            }
            if ((x > 0) && (y < 0))
            {
                outValue = MathEx.Atan(y / x) + 2 * MathEx.Pi;
            }
            if ((x == 0) && (y > 0))
            {
                outValue = MathEx.Pi / 2;
            }
            if ((x == 0) && (y < 0))
            {
                outValue = 3 * MathEx.Pi / 2;
            }
            if ((x == 0) && (y == 0))
            {
                outValue = 0;
            }
            return outValue;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Mods the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        private static double Mod(double x, double y)
        {
            return x - y * MathEx.Floor(x / y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Modlons the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        private static double Modlon(double x)
        {
            return Mod(x + MathEx.Pi, 2 * MathEx.Pi) - MathEx.Pi;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Modcrses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        private static double Modcrs(double x)
        {
            return Mod(x, 2 * MathEx.Pi);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Modlats the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        private static double Modlat(double x)
        {
            return Mod(x + MathEx.Pi / 2, 2 * MathEx.Pi) - MathEx.Pi / 2;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Crsdists the specified lat1.
        /// </summary>
        /// <param name="lat1">The lat1.</param>
        /// <param name="lon1">The lon1.</param>
        /// <param name="lat2">The lat2.</param>
        /// <param name="lon2">The lon2.</param>
        /// <returns></returns>
        private static CrsdistResult Crsdist(double lat1, double lon1, double lat2, double lon2)
        { // radian args
            /* compute course and Distance (spherical) */
            double crs12, crs21;
            double argacos;
            if ((lat1 + lat2 == 0.0) && (MathEx.Abs(lon1 - lon2) == MathEx.Pi)
                && (MathEx.Abs(lat1) != (MathEx.Pi / 180) * 90.0))
            {
                throw new ArgumentException("Course between antipodal points is undefined");
            }

            double d = MathEx.Acos(MathEx.Sin(lat1) * MathEx.Sin(lat2)
                + MathEx.Cos(lat1) * MathEx.Cos(lat2) * MathEx.Cos(lon1 - lon2));

            if ((d == 0.0) || (lat1 == -(MathEx.Pi / 180) * 90.0))
            {
                crs12 = 2 * MathEx.Pi;
            }
            else if (lat1 == (MathEx.Pi / 180) * 90.0)
            {
                crs12 = MathEx.Pi;
            }
            else
            {
                argacos = (MathEx.Sin(lat2) - MathEx.Sin(lat1) * MathEx.Cos(d))
                    / (MathEx.Sin(d) * MathEx.Cos(lat1));
                if (MathEx.Sin(lon2 - lon1) < 0)
                {
                    crs12 = Acosf(argacos);
                }
                else
                {
                    crs12 = 2 * MathEx.Pi - Acosf(argacos);
                }
            }
            if ((d == 0.0) || (lat2 == -(MathEx.Pi / 180) * 90.0))
            {
                crs21 = 0.0;
            }
            else if (lat2 == (MathEx.Pi / 180) * 90.0)
            {
                crs21 = MathEx.Pi;
            }
            else
            {
                argacos = (MathEx.Sin(lat1) - MathEx.Sin(lat2) * MathEx.Cos(d)
                    ) / (MathEx.Sin(d) * MathEx.Cos(lat2));
                if (MathEx.Sin(lon1 - lon2) < 0)
                {
                    crs21 = Acosf(argacos);
                }
                else
                {
                    crs21 = 2 * MathEx.Pi - Acosf(argacos);
                }
            }

            CrsdistResult outValue = new CrsdistResult { _d = d, _crs12 = crs12, _crs21 = crs21 };
            return outValue;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        private static CrsdistResult CrsdistEll(double glat1, double glon1,
            double glat2, double glon2, int ellipse)
        {
            // glat1 initial geodetic latitude in radians N positive
            // glon1 initial geodetic longitude in radians E positive
            // glat2 final geodetic latitude in radians N positive
            // glon2 final geodetic longitude in radians E positive
            double a = EarthModel[ellipse][0];
            double f = 1 / EarthModel[ellipse][1];
            //alert("a="+a+" f="+f)
            double sx = 0, cx = 0, sy = 0, cy = 0, y = 0;
            double c2A = 0, cz = 0, e = 0, c;
            const double eps = 0.00000000005;
            double iter = 1;
            const double maxiter = 100;
            CrsdistResult outValue = new CrsdistResult();
            if ((glat1 + glat2 == 0.0) && (MathEx.Abs(glon1 - glon2) == MathEx.Pi))
            {
                glat1 = glat1 + 0.00001; // allow algorithm to complete

            }
            if (glat1 == glat2 && (glon1 == glon2 ||
                MathEx.Abs(MathEx.Abs(glon1 - glon2) - 2 * MathEx.Pi) < eps))
            {


                outValue._d = 0;
                outValue._crs12 = 0;
                outValue._crs21 = MathEx.Pi;
                return outValue;
            }
            double r = 1 - f;
            double tu1 = r * MathEx.Tan(glat1);
            double tu2 = r * MathEx.Tan(glat2);
            double cu1 = 1.0 / MathEx.Sqrt(1.0 + tu1 * tu1);
            double su1 = cu1 * tu1;
            double cu2 = 1.0 / MathEx.Sqrt(1.0 + tu2 * tu2);
            double s1 = cu1 * cu2;
            double b1 = s1 * tu2;
            double f1 = b1 * tu1;
            double x = glon2 - glon1;
            double d = x + 1;
            while ((MathEx.Abs(d - x) > eps) && (iter < maxiter))
            {
                iter = iter + 1;
                sx = MathEx.Sin(x);
                cx = MathEx.Cos(x);
                tu1 = cu2 * sx;
                tu2 = b1 - su1 * cu2 * cx;
                sy = MathEx.Sqrt(tu1 * tu1 + tu2 * tu2);
                cy = s1 * cx + f1;
                y = Atan2(sy, cy);
                double sa = s1 * sx / sy;
                c2A = 1 - sa * sa;
                cz = f1 + f1;
                if (c2A > 0.0)
                {
                    cz = cy - cz / c2A;
                }
                e = cz * cz * 2.0 - 1.0;
                c = ((-3.0 * c2A + 4.0) * f + 4.0) * c2A * f / 16.0;
                d = x;
                x = ((e * cy * c + cz) * sy * c + y) * sa;
                x = (1.0 - c) * x * f + glon2 - glon1;
            }
            double faz = Modcrs(Atan2(tu1, tu2));
            double baz = Modcrs(Atan2(cu1 * sx, b1 * cx - su1 * cu2) + MathEx.Pi);
            x = MathEx.Sqrt((1 / (r * r) - 1) * c2A + 1);
            x += 1;
            x = (x - 2.0) / x;
            c = 1.0 - x;
            c = (x * x / 4.0 + 1.0) / c;
            d = (0.375 * x * x - 1.0) * x;
            x = e * cy;
            double s = ((((sy * sy * 4.0 - 3.0) * (1.0 - e - e) * cz * d / 6.0 - x) * d / 4.0 + cz) * sy * d + y)
                * c * a * r;

            outValue._d = s;
            outValue._crs12 = faz;
            outValue._crs21 = baz;
            if (MathEx.Abs(iter - maxiter) < eps)
            {
                throw new ArgumentException("Algorithm did not converge");
            }
            return outValue;

        }

        private static DirectResult Direct(double lat1, double lon1, double crs12, double d12)
        {
            const double eps = 0.00000000005;
            double lon;
            if ((MathEx.Abs(MathEx.Cos(lat1)) < eps) && !(MathEx.Abs(MathEx.Sin(crs12)) < eps))
            {
                throw new ArgumentException("Only N-S courses are meaningful, starting at a pole!");
            }

            double lat = MathEx.Asin(MathEx.Sin(lat1) * MathEx.Cos(d12) +
                                     MathEx.Cos(lat1) * MathEx.Sin(d12) * MathEx.Cos(crs12));
            if (MathEx.Abs(MathEx.Cos(lat)) < eps)
            {
                lon = 0.0; //endpoint a pole
            }
            else
            {
                double dlon = MathEx.Atan2(MathEx.Sin(crs12) * MathEx.Sin(d12) * MathEx.Cos(lat1),
                                           MathEx.Cos(d12) - MathEx.Sin(lat1) * MathEx.Sin(lat));
                lon = Mod(lon1 - dlon + MathEx.Pi, 2 * MathEx.Pi) - MathEx.Pi;
            }
            DirectResult outValue = new DirectResult { _lat = lat, _lon = lon };
            return outValue;
        }

        private static DirectResult DirectEll(double glat1, double glon1, double faz, double s, int ellipse)
        {
            // glat1 initial geodetic latitude in radians N positive
            // glon1 initial geodetic longitude in radians E positive
            // faz forward azimuth in radians
            // s Distance in units of a (=nm)

            const double eps = 0.00000000005;
            double b;
            double sy = 0, cy = 0, cz = 0, e = 0;

            if ((MathEx.Abs(MathEx.Cos(glat1)) < eps) && !(MathEx.Abs(MathEx.Sin(faz)) < eps))
            {
                throw new ArgumentException("Only N-S courses are meaningful, starting at a pole!");
            }

            double a = EarthModel[ellipse][0];
            double f = 1 / EarthModel[ellipse][1];
            double r = 1 - f;
            double tu = r * MathEx.Tan(glat1);
            double sf = MathEx.Sin(faz);
            double cf = MathEx.Cos(faz);
            if (cf == 0)
            {
                b = 0.0;
            }
            else
            {
                b = 2.0 * Atan2(tu, cf);
            }
            double cu = 1.0 / MathEx.Sqrt(1 + tu * tu);
            double su = tu * cu;
            double sa = cu * sf;
            double c2A = 1 - sa * sa;
            double x = 1.0 + MathEx.Sqrt(1.0 + c2A * (1.0 / (r * r) - 1.0));
            x = (x - 2.0) / x;
            double c = 1.0 - x;
            c = (x * x / 4.0 + 1.0) / c;
            double d = (0.375 * x * x - 1.0) * x;
            tu = s / (r * a * c);
            double y = tu;
            c = y + 1;
            while (MathEx.Abs(y - c) > eps)
            {
                sy = MathEx.Sin(y);
                cy = MathEx.Cos(y);
                cz = MathEx.Cos(b + y);
                e = 2.0 * cz * cz - 1.0;
                c = y;
                x = e * cy;
                y = e + e - 1.0;
                y = (((sy * sy * 4.0 - 3.0) * y * cz * d / 6.0 + x) *
                        d / 4.0 - cz) * sy * d + tu;
            }

            b = cu * cy * cf - su * sy;
            c = r * MathEx.Sqrt(sa * sa + b * b);
            d = su * cy + cu * sy * cf;
            double glat2 = Modlat(Atan2(d, c));
            c = cu * cy - su * sy * cf;
            x = Atan2(sy * sf, c);
            c = ((-3.0 * c2A + 4.0) * f + 4.0) * c2A * f / 16.0;
            d = ((e * cy * c + cz) * sy * c + y) * sa;
            double glon2 = Modlon(glon1 + x - (1.0 - c) * d * f);
            double baz = Modcrs(Atan2(sa, b) + MathEx.Pi);

            DirectResult outValue = new DirectResult { _lat = glat2, _lon = glon2, _crs21 = baz };
            return outValue;
        }
    }

    internal class CrsdistResult
    {

        internal double _d;
        internal double _crs12;
        internal double _crs21;
    }

    internal class DirectResult
    {

        internal double _lat;
        internal double _lon;
        internal double _crs21;
    }

}
