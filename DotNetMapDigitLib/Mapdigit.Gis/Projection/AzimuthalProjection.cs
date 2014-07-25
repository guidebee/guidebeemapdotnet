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
    /// The superclass for all azimuthal map projections
    /// </summary>
    internal class AzimuthalProjection : Projection
    {

        /// <summary>
        /// North pole
        /// </summary>
        public const int NorthPole = 1;
        /// <summary>
        /// Sourth pole
        /// </summary>
        public const int SouthPole = 2;
        /// <summary>
        /// 
        /// </summary>
        public const int Equator = 3;
        public const int Oblique = 4;

        protected int mode;
        protected double sinphi0, cosphi0;
        private double _mapRadius = 90.0;

        public AzimuthalProjection()
            : this(MathEx.ToRadians(45.0), MathEx.ToRadians(45.0))
        {

        }

        public AzimuthalProjection(double projectionLatitude, double projectionLongitude)
        {
            base.projectionLatitude = projectionLatitude;
            base.projectionLatitude = projectionLongitude;
            Initialize();
        }

        public new void Initialize()
        {
            base.Initialize();
            if (Math.Abs(Math.Abs(projectionLatitude) - MapMath.Halfpi) < Eps10)
                mode = projectionLatitude < 0.0 ? SouthPole : NorthPole;
            else if (Math.Abs(projectionLatitude) > Eps10)
            {
                mode = Oblique;
                sinphi0 = Math.Sin(projectionLatitude);
                cosphi0 = Math.Cos(projectionLatitude);
            }
            else
                mode = Equator;
        }


        public override bool Inside(double lon, double lat)
        {
            return MapMath.GreatCircleDistance(MathEx.ToRadians(lon),
                MathEx.ToRadians(lat), projectionLongitude, projectionLatitude) < MathEx.ToRadians(_mapRadius);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the map radius.
        /// </summary>
        /// <param name="mapRadius">The map radius.</param>
        public void SetMapRadius(double mapRadius)
        {
            _mapRadius = mapRadius;
        }

        public double GetMapRadius()
        {
            return _mapRadius;
        }

    }


}
