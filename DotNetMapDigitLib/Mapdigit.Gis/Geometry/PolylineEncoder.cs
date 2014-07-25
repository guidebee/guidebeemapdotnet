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
using System.Collections;
using System.Text;
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
    /// PolylineEncoder encode/decode google encoded polyline string.
    /// </summary>
    internal class PolylineEncoder
    {

        /**
         * total levels
         */
        private readonly int _numLevels = 18;
        /**
         *
         */
        private readonly int _zoomFactor = 2;
        private readonly double _verySmall = 0.00001;
        private readonly bool _forceEndpoints = true;
        private readonly double[] _zoomLevelBreaks;

        // constructor
        public PolylineEncoder(int numLevels, int zoomFactor, double verySmall,
                bool forceEndpoints)
        {

            _numLevels = numLevels;
            _zoomFactor = zoomFactor;
            _verySmall = verySmall;
            _forceEndpoints = forceEndpoints;

            _zoomLevelBreaks = new double[numLevels];

            for (int i = 0; i < numLevels; i++)
            {
                _zoomLevelBreaks[i] = verySmall * MathEx.Pow(_zoomFactor, numLevels - i - 1);
            }
        }

        public PolylineEncoder()
        {
            _zoomLevelBreaks = new double[_numLevels];
            for (int i = 0; i < _numLevels; i++)
            {
                _zoomLevelBreaks[i] = _verySmall * MathEx.Pow(_zoomFactor, _numLevels - i - 1);
            }
        }

        public bool ForceEndpoints
        {
            get { return _forceEndpoints; }
        }

        public GeoLatLngBounds Bounds { get; set; }

        public static int[] DecodeLevel(string levels)
        {
            int len = levels.Length;
            int[] ret = new int[len];
            for (int i = 0; i < len; i++)
            {
                char ch = levels[i];
                ret[i] = ch - '?';
            }

            return ret;
        }

        private static int Floor1E5(double coordinate)
        {
            return (int)MathEx.Floor(coordinate * 1e5);
        }

        private static string EncodeSignedNumber(int num)
        {
            int sgnNum = num << 1;
            if (num < 0)
            {
                sgnNum = ~(sgnNum);
            }
            return (EncodeNumber(sgnNum));
        }

        private static string EncodeNumber(int num)
        {

            StringBuilder encodeString = new StringBuilder();

            while (num >= 0x20)
            {
                int nextValue = (0x20 | (num & 0x1f)) + 63;
                encodeString.Append((char)(nextValue));
                num >>= 5;
            }

            num += 63;
            encodeString.Append((char)(num));

            return encodeString.ToString();
        }



        public static ArrayList CreateDecodings(string polyline)
        {
            char[] encoded = polyline.ToCharArray();
            int len = encoded.Length;
            int index = 0;
            int lat = 0;
            int lng = 0;
            ArrayList array = new ArrayList();
            while (index < len)
            {
                int shift = 0;
                int result = 0;
                int a;
                int b;
                do
                {
                    a = encoded[index];
                    b = a - 63;
                    result |= (b & 0x1f) << shift;
                    shift += 5;
                    index += 1;
                } while (b >= 0x20);

                int dlat = ((result & 1) == 1 ? ~(result >> 1) : (result >> 1));
                lat += dlat;
                shift = 0;
                result = 0;


                do
                {
                    a = encoded[index];
                    b = a - 63;
                    result |= (b & 0x1f) << shift;
                    shift += 5;
                    index += 1;
                } while (b >= 0x20);

                int dlng = ((result & 1) == 1 ? ~(result >> 1) : (result >> 1));
                lng += dlng;

                array.Add(new GeoLatLng(lat * 1e-5, lng * 1e-5));
            }

            return array;
        }

        public static string[] CreateEncodings(GeoLatLng[] track, int level, int step)
        {

            string[] resultMap = new string[2];
            StringBuilder encodedPoints = new StringBuilder();
            StringBuilder encodedLevels = new StringBuilder();
            int plat = 0;
            int plng = 0;
            int listSize = track.Length;

            GeoLatLng trackpoint;

            for (int i = 0; i < listSize; i += step)
            {
                trackpoint = track[i];

                int late5 = Floor1E5(trackpoint.Latitude);
                int lnge5 = Floor1E5(trackpoint.Longitude);

                int dlat = late5 - plat;
                int dlng = lnge5 - plng;

                plat = late5;
                plng = lnge5;

                encodedPoints.Append(EncodeSignedNumber(dlat)).Append(
                        EncodeSignedNumber(dlng));
                encodedLevels.Append(EncodeNumber(level));

            }

            resultMap[0] = encodedPoints.ToString();
            resultMap[1] = encodedLevels.ToString();

            return resultMap;
        }
    }


}
