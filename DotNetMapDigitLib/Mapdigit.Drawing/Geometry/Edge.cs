//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 25SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Drawing.Geometry
{
    internal class Edge
    {
        private readonly Curve _curve;
        private readonly int _ctag;
        private int _etag;
        private double _activey;
        private int _equivalence;

        public Edge(Curve c, int ctag)
            : this(c, ctag, AreaOp.EtagIgnore)
        {
        }

        public Edge(Curve c, int ctag, int etag)
        {
            _curve = c;
            _ctag = ctag;
            _etag = etag;
        }

        public Curve GetCurve()
        {
            return _curve;
        }

        public int GetCurveTag()
        {
            return _ctag;
        }

        public int GetEdgeTag()
        {
            return _etag;
        }

        public void SetEdgeTag(int etag)
        {
            _etag = etag;
        }

        public int GetEquivalence()
        {
            return _equivalence;
        }

        public void SetEquivalence(int eq)
        {
            _equivalence = eq;
        }

        private Edge _lastEdge;
        private int _lastResult;
        private double _lastLimit;

        public int CompareTo(Edge other, double[] yrange)
        {
            if (other == _lastEdge && yrange[0] < _lastLimit)
            {
                if (yrange[1] > _lastLimit)
                {
                    yrange[1] = _lastLimit;
                }
                return _lastResult;
            }
            if (this == other._lastEdge && yrange[0] < other._lastLimit)
            {
                if (yrange[1] > other._lastLimit)
                {
                    yrange[1] = other._lastLimit;
                }
                return 0 - other._lastResult;
            }
            //long start = System.currentTimeMillis();
            int ret = _curve.CompareTo(other._curve, yrange);
            //long end = System.currentTimeMillis();
            /*
                System.out.println("compare: "+
                ((System.identityHashCode(this) <
                System.identityHashCode(other))
                ? this+" to "+other
                : other+" to "+this)+
                " == "+ret+" at "+yrange[1]+
                " in "+(end-start)+"ms");
                 */
            _lastEdge = other;
            _lastLimit = yrange[1];
            _lastResult = ret;
            return ret;
        }

        public void Record(double yend, int etag)
        {
            _activey = yend;
            _etag = etag;
        }

        public bool IsActiveFor(double y, int etag)
        {
            return (_etag == etag && _activey >= y);
        }

        public override string ToString()
        {
            return ("Edge[" + _curve +
                    ", " +
                    (_ctag == AreaOp.CtagLeft ? "L" : "R") +
                    ", " +
                    (_etag == AreaOp.EtagEnter ? "I" : (_etag == AreaOp.EtagExit ? "O" : "N")) +
                    "]");
        }
    }
}