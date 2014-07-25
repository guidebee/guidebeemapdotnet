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
    /// The superclass for all map projections
    /// </summary>
    internal abstract class Projection
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Project a lat/long point (in degrees), producing a result in metres
        /// </summary>
        /// <param name="src">the lat/long point in degree.</param>
        /// <param name="dst">the projection coordinate on the plane in meters..</param>
        /// <returns>the projection coordinate on the plane in meters</returns>
        public GeoPoint Transform(GeoPoint src, GeoPoint dst)
        {
            double x = src.X * Dtr;
            if (projectionLongitude != 0)
            {
                x = MapMath.NormalizeLongitude(x - projectionLongitude);
            }
            Project(x, src.Y * Dtr, dst);
            dst.X = _totalScale * dst.X + falseEasting;
            dst.Y = _totalScale * dst.Y + falseNorthing;
            return dst;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Project a lat/long point, producing a result in metres
        /// </summary>
        /// <param name="src">lat/long point in radians.</param>
        /// <param name="dst">the projection coordinate on the plane in meters.</param>
        /// <returns>the projection coordinate on the plane in meters.</returns>
        public GeoPoint TransformRadians(GeoPoint src, GeoPoint dst)
        {
            double x = src.X;
            if (projectionLongitude != 0)
            {
                x = MapMath.NormalizeLongitude(x - projectionLongitude);
            }
            Project(x, src.Y, dst);
            dst.X = _totalScale * dst.X + falseEasting;
            dst.Y = _totalScale * dst.Y + falseNorthing;
            return dst;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  The method which actually does the projection. This should be overridden 
        /// for all projections.
        /// </summary>
        /// <param name="x">longitude</param>
        /// <param name="y">latitude</param>
        /// <param name="dst">The DST.</param>
        /// <returns>the projected coordinates in map</returns>
        public virtual GeoPoint Project(double x, double y, GeoPoint dst)
        {
            dst.X = x;
            dst.Y = y;
            return dst;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Project a number of lat/long points (in degrees), producing a result in metres
        /// </summary>
        /// <param name="srcPoints">the source point array of lat/long in degrees.</param>
        /// <param name="srcOffset">the start index of the source array.</param>
        /// <param name="dstPoints">the result array..</param>
        /// <param name="dstOffset">the start index of the result array..</param>
        /// <param name="numPoints">the number of points need to be transformed.</param>
        public void Transform(double[] srcPoints, int srcOffset, double[] dstPoints,
                int dstOffset, int numPoints)
        {
            GeoPoint input = new GeoPoint();
            GeoPoint output = new GeoPoint();
            for (int i = 0; i < numPoints; i++)
            {
                input.X = srcPoints[srcOffset++] * Dtr;
                input.Y = srcPoints[srcOffset++] * Dtr;
                Transform(input, output);
                dstPoints[dstOffset++] = output.X;
                dstPoints[dstOffset++] = output.Y;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Project a number of lat/long points (in radians), producing a result in metres
        /// </summary>
        /// <param name="srcPoints">the source point array of lat/long in degrees.</param>
        /// <param name="srcOffset">the start index of the source array.</param>
        /// <param name="dstPoints">the result array..</param>
        /// <param name="dstOffset">the start index of the result array..</param>
        /// <param name="numPoints">the number of points need to be transformed.</param>
        public void TransformRadians(double[] srcPoints, int srcOffset,
                double[] dstPoints, int dstOffset, int numPoints)
        {
            GeoPoint input = new GeoPoint();
            GeoPoint output = new GeoPoint();
            for (int i = 0; i < numPoints; i++)
            {
                input.X = srcPoints[srcOffset++];
                input.Y = srcPoints[srcOffset++];
                Transform(input, output);
                dstPoints[dstOffset++] = output.X;
                dstPoints[dstOffset++] = output.Y;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Inverse-project a point (in metres), producing a lat/long result in degrees
        /// </summary>
        /// <param name="src">the point in meters on the plane map.</param>
        /// <param name="dst">the lat/long result in degrees.</param>
        /// <returns> the lat/long result in degrees.</returns>
        public GeoPoint InverseTransform(GeoPoint src, GeoPoint dst)
        {
            double x = (src.X - falseEasting) / _totalScale;
            double y = (src.Y - falseNorthing) / _totalScale;
            ProjectInverse(x, y, dst);
            if (dst.X < -Math.PI)
            {
                dst.X = -Math.PI;
            }
            else if (dst.X > Math.PI)
            {
                dst.X = Math.PI;
            }
            if (projectionLongitude != 0)
            {
                dst.X = MapMath.NormalizeLongitude(dst.X + projectionLongitude);
            }
            dst.X *= Rtd;
            dst.Y *= Rtd;
            return dst;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Inverse-project a point (in metres), producing a lat/long result in radians
        /// </summary>
        /// <param name="src">the point in meters on the plane map.</param>
        /// <param name="dst">the lat/long result in degrees.</param>
        /// <returns> the lat/long result in degrees.</returns>
        public GeoPoint InverseTransformRadians(GeoPoint src, GeoPoint dst)
        {
            double x = (src.X - falseEasting) / _totalScale;
            double y = (src.Y - falseNorthing) / _totalScale;
            ProjectInverse(x, y, dst);
            if (dst.X < -Math.PI)
            {
                dst.X = -Math.PI;
            }
            else if (dst.X > Math.PI)
            {
                dst.X = Math.PI;
            }
            if (projectionLongitude != 0)
            {
                dst.X = MapMath.NormalizeLongitude(dst.X + projectionLongitude);
            }
            return dst;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The method which actually does the inverse projection. This should be 
        /// overridden for all projections.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="dst">The DST.</param>
        /// <returns> the inverse transformed coordinates</returns>
        public virtual GeoPoint ProjectInverse(double x, double y, GeoPoint dst)
        {
            dst.X = x;
            dst.Y = y;
            return dst;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Inverse-project a number of points (in metres), producing 
        /// a lat/long result in degrees
        /// </summary>
        /// <param name="srcPoints">the source point array of lat/long in degrees.</param>
        /// <param name="srcOffset">the start index of the source array.</param>
        /// <param name="dstPoints">the result array..</param>
        /// <param name="dstOffset">the start index of the result array.</param>
        /// <param name="numPoints">the number of points need to be transformed.</param>
        public void InverseTransform(double[] srcPoints, int srcOffset,
                double[] dstPoints, int dstOffset, int numPoints)
        {
            GeoPoint input = new GeoPoint();
            GeoPoint output = new GeoPoint();
            for (int i = 0; i < numPoints; i++)
            {
                input.X = srcPoints[srcOffset++];
                input.Y = srcPoints[srcOffset++];
                InverseTransform(input, output);
                dstPoints[dstOffset++] = output.X * Rtd;
                dstPoints[dstOffset++] = output.Y * Rtd;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Inverse-project a number of points (in metres), producing
        /// a lat/long result in radians
        /// </summary>
        /// <param name="srcPoints">the source point array of lat/long in degrees.</param>
        /// <param name="srcOffset">the start index of the source array.</param>
        /// <param name="dstPoints">the result array..</param>
        /// <param name="dstOffset">the start index of the result array.</param>
        /// <param name="numPoints">the number of points need to be transformed.</param>
        public void InverseTransformRadians(double[] srcPoints, int srcOffset,
                double[] dstPoints, int dstOffset, int numPoints)
        {
            GeoPoint input = new GeoPoint();
            GeoPoint output = new GeoPoint();
            for (int i = 0; i < numPoints; i++)
            {
                input.X = srcPoints[srcOffset++];
                input.Y = srcPoints[srcOffset++];
                InverseTransform(input, output);
                dstPoints[dstOffset++] = output.X;
                dstPoints[dstOffset++] = output.Y;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Finds the smallest lat/long rectangle wholly inside the given view rectangle.
        /// This is only a rough estimate.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        public GeoBounds InverseTransform(GeoBounds r)
        {
            GeoPoint input = new GeoPoint();
            GeoPoint output = new GeoPoint();
            GeoBounds bounds = null;
            if (IsRectilinear())
            {
                for (int ix = 0; ix < 2; ix++)
                {
                    double x = r.X + r.Width * ix;
                    for (int iy = 0; iy < 2; iy++)
                    {
                        double y = r.Y + r.Height * iy;
                        input.X = x;
                        input.Y = y;
                        InverseTransform(input, output);
                        if (ix == 0 && iy == 0)
                        {
                            bounds = new GeoBounds(output.X, output.Y, 0, 0);
                        }
                        else
                        {
                            if (bounds != null) bounds.Add(output.X, output.Y);
                        }
                    }
                }
            }
            else
            {
                for (int ix = 0; ix < 7; ix++)
                {
                    double x = r.X + r.Width * ix / 6;
                    for (int iy = 0; iy < 7; iy++)
                    {
                        double y = r.Y + r.Height * iy / 6;
                        input.X = x;
                        input.Y = y;
                        InverseTransform(input, output);
                        if (ix == 0 && iy == 0)
                        {
                            bounds = new GeoBounds(output.X, output.Y, 0, 0);
                        }
                        else
                        {
                            if (bounds != null) bounds.Add(output.X, output.Y);
                        }
                    }
                }
            }
            return bounds;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Transform a bounding box. This is only a rough estimate.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        public GeoBounds Transform(GeoBounds r)
        {
            GeoPoint input = new GeoPoint();
            GeoPoint output = new GeoPoint();
            GeoBounds bounds = null;
            if (IsRectilinear())
            {
                for (int ix = 0; ix < 2; ix++)
                {
                    double x = r.X + r.Width * ix;
                    for (int iy = 0; iy < 2; iy++)
                    {
                        double y = r.Y + r.Height * iy;
                        input.X = x;
                        input.Y = y;
                        Transform(input, output);
                        if (ix == 0 && iy == 0)
                        {
                            bounds = new GeoBounds(output.X, output.Y, 0, 0);
                        }
                        else
                        {
                            if (bounds != null) bounds.Add(output.X, output.Y);
                        }
                    }
                }
            }
            else
            {
                for (int ix = 0; ix < 7; ix++)
                {
                    double x = r.X + r.Width * ix / 6;
                    for (int iy = 0; iy < 7; iy++)
                    {
                        double y = r.Y + r.Height * iy / 6;
                        input.X = x;
                        input.Y = y;
                        Transform(input, output);
                        if (ix == 0 && iy == 0)
                        {
                            bounds = new GeoBounds(output.X, output.Y, 0, 0);
                        }
                        else
                        {
                            if (bounds != null) bounds.Add(output.X, output.Y);
                        }
                    }
                }
            }
            return bounds;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if this projection is conformal
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is conformal; otherwise, <c>false</c>.
        /// </returns>
        public bool IsConformal()
        {
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if this projection is equal area
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is equal area]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEqualArea()
        {
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Returns true if this projection has an inverse
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance has inverse; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool HasInverse()
        {
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if lat/long lines form a rectangular grid for this projection
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is rectilinear; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRectilinear()
        {
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if latitude lines are parallel for this projection
        /// </summary>
        /// <returns></returns>
        public bool ParallelsAreParallel()
        {
            return IsRectilinear();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if the given lat/long point is visible in this projection
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public virtual bool Inside(double x, double y)
        {
            x = NormalizeLongitude((float)(x * Dtr - projectionLongitude));
            return minLongitude <= x && x <= maxLongitude
                    && minLatitude <= y && y <= maxLatitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the name of this projection.
        /// </summary>
        /// <param name="name">The name.</param>
        public void SetName(string name)
        {
            newName = name;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            if (newName != null)
            {
                return newName;
            }
            return ToString();
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "None";
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the minimum latitude. This is only used for Shape clipping
        /// and doesn't affect projection.
        /// </summary>
        /// <param name="latitude">The min latitude.</param>
        public void SetMinLatitude(double latitude)
        {
            minLatitude = latitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the minimum latitude.
        /// </summary>
        /// <returns></returns>
        public double GetMinLatitude()
        {
            return minLatitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Set the maximum latitude. This is only used for Shape clipping
        /// and doesn't affect projection.
        /// </summary>
        /// <param name="latitude">The max latitude.</param>
        public void SetMaxLatitude(double latitude)
        {
            maxLatitude = latitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the max latitude.
        /// </summary>
        /// <returns></returns>
        public double GetMaxLatitude()
        {
            return maxLatitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the max latitude in degrees.
        /// </summary>
        /// <returns></returns>
        public double GetMaxLatitudeDegrees()
        {
            return maxLatitude * Rtd;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the min latitude in degrees.
        /// </summary>
        /// <returns></returns>
        public double GetMinLatitudeDegrees()
        {
            return minLatitude * Rtd;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the min longitude.
        /// </summary>
        /// <param name="longitude">The min longitude.</param>
        public void SetMinLongitude(double longitude)
        {
            minLongitude = longitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the min longitude.
        /// </summary>
        /// <returns></returns>
        public double GetMinLongitude()
        {
            return minLongitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the min longitude in degrees.
        /// </summary>
        /// <param name="longitude">The min longitude.</param>
        public void SetMinLongitudeDegrees(double longitude)
        {
            minLongitude = Dtr * longitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the min longitude in degrees.
        /// </summary>
        /// <returns></returns>
        public double GetMinLongitudeDegrees()
        {
            return minLongitude * Rtd;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the max longitude.
        /// </summary>
        /// <param name="longitude">The max longitude.</param>
        public void SetMaxLongitude(double longitude)
        {
            maxLongitude = longitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the max longitude.
        /// </summary>
        /// <returns></returns>
        public double GetMaxLongitude()
        {
            return maxLongitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the max longitude in degrees.
        /// </summary>
        /// <param name="longitude">The max longitude.</param>
        public void SetMaxLongitudeDegrees(double longitude)
        {
            maxLongitude = Dtr * longitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the max longitude in degrees.
        /// </summary>
        /// <returns></returns>
        public double GetMaxLongitudeDegrees()
        {
            return maxLongitude * Rtd;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the projection latitude.
        /// </summary>
        /// <param name="latitude">The projection latitude.</param>
        public void SetProjectionLatitude(double latitude)
        {
            projectionLatitude = latitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the projection latitude.
        /// </summary>
        /// <returns></returns>
        public double GetProjectionLatitude()
        {
            return projectionLatitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the projection latitude in degrees.
        /// </summary>
        /// <param name="latitude">The projection latitude.</param>
        public void SetProjectionLatitudeDegrees(double latitude)
        {
            projectionLatitude = Dtr * latitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the projection latitude in degrees.
        /// </summary>
        /// <returns></returns>
        public double GetProjectionLatitudeDegrees()
        {
            return projectionLatitude * Rtd;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the projection longitude.
        /// </summary>
        /// <param name="longitude">The projection longitude.</param>
        public void SetProjectionLongitude(double longitude)
        {
            projectionLongitude = NormalizeLongitudeRadians(longitude);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the projection longitude.
        /// </summary>
        /// <returns></returns>
        public double GetProjectionLongitude()
        {
            return projectionLongitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the projection longitude in degrees.
        /// </summary>
        /// <param name="longitude">The projection longitude.</param>
        public void SetProjectionLongitudeDegrees(double longitude)
        {
            projectionLongitude = Dtr * longitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the projection longitude in degrees.
        /// </summary>
        /// <returns></returns>
        public double GetProjectionLongitudeDegrees()
        {
            return projectionLongitude * Rtd;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the true scale latitude.
        /// </summary>
        /// <param name="latitude">The true scale latitude.</param>
        public void SetTrueScaleLatitude(double latitude)
        {
            trueScaleLatitude = latitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the true scale latitude.
        /// </summary>
        /// <returns></returns>
        public double GetTrueScaleLatitude()
        {
            return trueScaleLatitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the true scale latitude in degrees.
        /// </summary>
        /// <param name="latitude">The true scale latitude.</param>
        public void SetTrueScaleLatitudeDegrees(double latitude)
        {
            trueScaleLatitude = Dtr * latitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the true scale latitude in degrees.
        /// </summary>
        /// <returns></returns>
        public double GetTrueScaleLatitudeDegrees()
        {
            return trueScaleLatitude * Rtd;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the projection latitude1.
        /// </summary>
        /// <param name="latitude1">The projection latitude1.</param>
        public void SetProjectionLatitude1(double latitude1)
        {
            projectionLatitude1 = latitude1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the projection latitude1.
        /// </summary>
        /// <returns></returns>
        public double GetProjectionLatitude1()
        {
            return projectionLatitude1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the projection latitude1 in degrees.
        /// </summary>
        /// <param name="latitude1">The projection latitude1.</param>
        public void SetProjectionLatitude1Degrees(double latitude1)
        {
            projectionLatitude1 = Dtr * latitude1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the projection latitude1 in degrees.
        /// </summary>
        /// <returns></returns>
        public double GetProjectionLatitude1Degrees()
        {
            return projectionLatitude1 * Rtd;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the projection latitude2.
        /// </summary>
        /// <param name="latitude2">The projection latitude2.</param>
        public void SetProjectionLatitude2(double latitude2)
        {
            projectionLatitude2 = latitude2;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the projection latitude2.
        /// </summary>
        /// <returns></returns>
        public double GetProjectionLatitude2()
        {
            return projectionLatitude2;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the projection latitude2 in degrees.
        /// </summary>
        /// <param name="latitude2">The projection latitude2.</param>
        public void SetProjectionLatitude2Degrees(double latitude2)
        {
            projectionLatitude2 = Dtr * latitude2;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the projection latitude2 in degrees.
        /// </summary>
        /// <returns></returns>
        public double GetProjectionLatitude2Degrees()
        {
            return projectionLatitude2 * Rtd;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the false northing.
        /// </summary>
        /// <param name="northing">The false northing.</param>
        public void SetFalseNorthing(double northing)
        {
            falseNorthing = northing;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the false northing.
        /// </summary>
        /// <returns></returns>
        public double GetFalseNorthing()
        {
            return falseNorthing;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the false easting.
        /// </summary>
        /// <param name="easting">The false easting.</param>
        public void SetFalseEasting(double easting)
        {
            falseEasting = easting;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the false Easting in projected units.
        /// </summary>
        /// <returns></returns>
        public double GetFalseEasting()
        {
            return falseEasting;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the scale factor.
        /// </summary>
        /// <param name="factor">The scale factor.</param>
        public void SetScaleFactor(double factor)
        {
            scaleFactor = factor;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the scale factor.
        /// </summary>
        /// <returns></returns>
        public double GetScaleFactor()
        {
            return scaleFactor;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the equator radius.
        /// </summary>
        /// <returns></returns>
        public double GetEquatorRadius()
        {
            return a;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets from metres.
        /// </summary>
        /// <param name="from">From metres.</param>
        public void SetFromMetres(double from)
        {
            fromMetres = from;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets from metres.
        /// </summary>
        /// <returns></returns>
        public double GetFromMetres()
        {
            return fromMetres;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the ellipsoid.
        /// </summary>
        /// <param name="ellips">The ellipsoid.</param>
        public void SetEllipsoid(Ellipsoid ellips)
        {
            ellipsoid = ellips;
            a = ellips.EquatorRadius;
            e = ellips.Eccentricity;
            es = ellips.Eccentricity2;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the ellipsoid.
        /// </summary>
        /// <returns></returns>
        public Ellipsoid GetEllipsoid()
        {
            return ellipsoid;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initialize the projection. This should be called after setting
        /// parameters and before using the projection.
        /// This is for performance reasons as initialization may be expensive.
        /// </summary>
        public void Initialize()
        {
            spherical = e == 0.0;
            oneEs = 1 - es;
            roneEs = 1.0 / oneEs;
            _totalScale = a * fromMetres;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Normalizes the longitude.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public static float NormalizeLongitude(float angle)
        {
            if (Double.IsInfinity(angle) || Double.IsNaN(angle))
            {
                throw new ArgumentException("Infinite longitude");
            }
            while (angle > 180)
            {
                angle -= 360;
            }
            while (angle < -180)
            {
                angle += 360;
            }
            return angle;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Normalizes the longitude radians.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public static double NormalizeLongitudeRadians(double angle)
        {
            if (Double.IsInfinity(angle) || Double.IsNaN(angle))
            {
                throw new ArgumentException("Infinite longitude");
            }
            while (angle > Math.PI)
            {
                angle -= MapMath.Twopi;
            }
            while (angle < -Math.PI)
            {
                angle += MapMath.Twopi;
            }
            return angle;
        }

        /**
         * The minimum latitude of the bounds of this projection
         */
        protected double minLatitude = -Math.PI / 2;

        /**
         * The minimum longitude of the bounds of this projection.
         * This is relative to the projection centre.
         */
        protected double minLongitude = -Math.PI;

        /**
         * The maximum latitude of the bounds of this projection
         */
        protected double maxLatitude = Math.PI / 2;

        /**
         * The maximum longitude of the bounds of this projection.
         * This is relative to the projection centre.
         */
        protected double maxLongitude = Math.PI;

        /**
         * The latitude of the centre of projection
         */
        protected double projectionLatitude;

        /**
         * The longitude of the centre of projection
         */
        protected double projectionLongitude;

        /**
         * Standard parallel 1 (for projections which use it)
         */
        protected double projectionLatitude1;

        /**
         * Standard parallel 2 (for projections which use it)
         */
        protected double projectionLatitude2;

        /**
         * The projection scale factor
         */
        protected double scaleFactor;

        /**
         * The false Easting of this projection
         */
        protected double falseEasting;

        /**
         * The false Northing of this projection
         */
        protected double falseNorthing;

        /**
         * The latitude of true scale. Only used by specific projections.
         */
        protected double trueScaleLatitude;

        /**
         * The equator radius
         */
        protected double a;

        /**
         * The eccentricity
         */
        protected double e;

        /**
         * The eccentricity squared
         */
        protected double es;

        /**
         * 1-(eccentricity squared)
         */
        protected double oneEs;

        /**
         * 1/(1-(eccentricity squared))
         */
        protected double roneEs;

        /**
         * The ellipsoid used by this projection
         */
        protected Ellipsoid ellipsoid;

        /**
         * True if this projection is using a sphere (es == 0)
         */
        protected bool spherical;


        /**
         * The name of this projection
         */
        protected string newName;

        /**
         * Conversion factor from metres to whatever units the projection uses.
         */
        protected double fromMetres = 1;

        /**
         * The total scale factor = Earth radius * units
         */
        private double _totalScale;

        // Some useful constants
        protected const double Eps10 = 1e-10;
        protected const double Rtd = 180.0 / Math.PI;
        protected const double Dtr = Math.PI / 180.0;

        protected Projection()
        {
            SetEllipsoid(Ellipsoid.Sphere);
        }
    }


}
