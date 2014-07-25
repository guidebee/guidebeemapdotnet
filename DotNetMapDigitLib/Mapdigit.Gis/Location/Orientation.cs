//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 28SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Location
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 28SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The Orientation class represents the physical orientation of the terminal.
    /// Orientation is described by azimuth to north (the horizontal pointing 
    /// direction), pitch (the vertical elevation angle) and roll (the rotation of 
    /// the terminal around its own longitudinal axis). 
    /// </summary>
    public class Orientation
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new Orientation object with the compass azimuth, pitch and 
        /// roll parameters specified.
        /// </summary>
        /// <remarks>
        /// The values are expressed in degress using floating point values.
        /// If the pitch or roll is undefined, the parameter shall be given as 
        /// Float.NaN.
        /// </remarks>
        /// <param name="azimuth">The compass azimuth relative to true or magnetic north. 
        ///         Valid range: [0.0, 360.0). </param>
        /// <param name="isMagnetic">a boolean stating whether the compass azimuth is 
        ///        given as relative to the magnetic field of the Earth (=true) or to
        ///        true north and gravity (=false)</param>
        /// <param name="pitch">pitch the pitch of the terminal in degrees, valid range: 
        ///        [-90.0, 90.0]</param>
        /// <param name="roll">roll the roll of the terminal in degrees, valid range: 
        ///         [-180.0, 180.0)</param>
        public Orientation(float azimuth, bool isMagnetic, float pitch, float roll)
        {
            _pitch = pitch;
            _azimuth = azimuth;
            _isMagnetic = isMagnetic;
            _roll = roll;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the terminal's horizontal compass azimuth in degrees relative 
        /// to either magnetic or true north. 
        /// </summary>
        /// <remarks>
        /// The value is always in the range 
        /// [0.0, 360.0) degrees. The isOrientationMagnetic() method indicates 
        /// whether the returned azimuth is relative to true north or magnetic north.
        /// </remarks>
        public float CompassAzimuth
        {
            get { return _azimuth; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the terminal's tilt in degrees defined as an angle in the 
        /// vertical plane orthogonal to the ground, and through the longitudinal 
        /// axis of the terminal. 
        /// </summary>
        /// <remarks>
        /// The value is always in the range [-90.0, 90.0] 
        /// degrees. A negative value means that the top of the terminal screen is 
        /// pointing towards the ground. 
        /// </remarks>
        public float Pitch
        {
            get { return _pitch; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the terminal's rotation in degrees around its own longitudinal 
        /// axis. The value is always in the range [-180.0, 180.0) degrees. 
        /// </summary>
        /// <remarks>
        /// A negative value means that the terminal is orientated anti-clockwise 
        /// from its default orientation, looking from direction of the bottom of 
        /// the screen. 
        /// </remarks>
        public float Roll
        {
            get { return _roll; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a boolean value that indicates whether this Orientation is 
        /// relative to the magnetic field of the Earth or relative to true north 
        /// and gravity. 
        /// </summary>
        /// <remarks>
        /// If this method returns true, the compass azimuth and pitch 
        /// are relative to the magnetic field of the Earth. If this method returns 
        /// false, the compass azimuth isrelative to true north and pitch is relative
        /// to gravity. 
        /// </remarks>
        /// <returns>
        /// 	true if this Orientation is relative to the magnetic field of 
        /// the Earth, false if this Orientation is relative to true north and 
        /// gravity.
        /// </returns>
        public bool IsOrientationMagnetic()
        {
            return _isMagnetic;
        }

        private readonly float _azimuth;
        private readonly bool _isMagnetic;
        private readonly float _pitch;
        private readonly float _roll;
    }
}
