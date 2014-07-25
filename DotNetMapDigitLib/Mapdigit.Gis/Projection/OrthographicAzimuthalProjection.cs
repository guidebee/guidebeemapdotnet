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
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 29SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The Orthographic Azimuthal or Globe map projection.
    /// </summary>
    internal class OrthographicAzimuthalProjection : AzimuthalProjection
    {

        public OrthographicAzimuthalProjection()
        {
            Initialize();
        }

        public override GeoPoint Project(double lam, double phi, GeoPoint xy)
        {
            double cosphi = MathEx.Cos(phi);
            double coslam = MathEx.Cos(lam);

            // Theoretically we should throw the ProjectionExceptions below, but for practical purposes
            // it's better not to as they tend to crop up a lot up due to rounding errors.
            switch (mode)
            {
                case Equator:
                    //			if (cosphi * coslam < - EPS10)
                    //				throw new ProjectionException();
                    xy.Y = MathEx.Sin(phi);
                    break;
                case Oblique:
                    double sinphi = MathEx.Sin(phi);
                    //			if (sinphi0 * (sinphi) + cosphi0 * cosphi * coslam < - EPS10)
                    //				;
                    //			   throw new ProjectionException();
                    xy.Y = cosphi0 * sinphi - sinphi0 * cosphi * coslam;
                    break;
                case NorthPole:
                    coslam = -coslam;
                    xy.Y = cosphi * coslam;
                    break;
                case SouthPole:
                    //			if (MathEx.Abs(phi - projectionLatitude) - EPS10 > MapMath.HALFPI)
                    //				throw new ProjectionException();
                    xy.Y = cosphi * coslam;
                    break;
            }
            xy.X = cosphi * MathEx.Sin(lam);
            return xy;
        }

        public override GeoPoint ProjectInverse(double x, double y, GeoPoint lp)
        {
            double rh, sinc;

            if ((sinc = (rh = MapMath.Distance(x, y))) > 1.0)
            {
                if ((sinc - 1.0) > Eps10) throw new ProjectionException();
                sinc = 1.0;
            }
            double cosc = MathEx.Sqrt(1.0 - sinc * sinc);
            if (MathEx.Abs(rh) <= Eps10)
                lp.Y = projectionLatitude;
            else switch (mode)
                {
                    case NorthPole:
                        y = -y;
                        lp.Y = MathEx.Acos(sinc);
                        break;
                    case SouthPole:
                        lp.Y = -MathEx.Acos(sinc);
                        break;
                    case Equator:
                        lp.Y = y * sinc / rh;
                        x *= sinc;
                        y = cosc * rh;
                        if (MathEx.Abs(lp.Y) >= 1.0)
                            lp.Y = lp.Y < 0.0 ? -MapMath.Halfpi : MapMath.Halfpi;
                        else
                            lp.Y = MathEx.Asin(lp.Y);
                        break;
                    case Oblique:
                        lp.Y = cosc * sinphi0 + y * sinc * cosphi0 / rh;
                        y = (cosc - sinphi0 * lp.Y) * rh;
                        x *= sinc * cosphi0;
                        if (MathEx.Abs(lp.Y) >= 1.0)
                            lp.Y = lp.Y < 0.0 ? -MapMath.Halfpi : MapMath.Halfpi;
                        else
                            lp.Y = MathEx.Asin(lp.Y);
                        break;
                }
            lp.X = (y == 0.0 && (mode == Oblique || mode == Equator)) ?
                 (x == 0.0 ? 0.0 : x < 0.0 ? -MapMath.Halfpi : MapMath.Halfpi) : MathEx.Atan2(x, y);
            return lp;
        }

        public override bool HasInverse()
        {
            return true;
        }

        public override string ToString()
        {
            return "Orthographic Azimuthal";
        }

    }

}
