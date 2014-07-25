//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 23SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Ajax
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Code review
    // --------   -------------------  -------------      ----------------------
    // 23SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This interface is used to monitor the read/write progress and handle the http
    /// response.
    /// </summary>
    public interface IRequestListener
    {

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Read progress notification. 
        /// </summary>
        /// <param name="context">message context, can be any object.</param>
        /// <param name="bytes">the number of bytes has been read.</param>
        /// <param name="total">total bytes to be read.Total will be zero if not available</param>
        void ReadProgress(object context, int bytes, int total);

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Write progress notification. 
        /// </summary>
        /// <param name="context">message context, can be any object.</param>
        /// <param name="bytes">the number of bytes has been written.</param>
        /// <param name="total">total bytes to be written.Total will be zero if not available .</param>
        void WriteProgress(object context, int bytes, int total);


        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handle the http response. 
        /// </summary>
        /// <param name="context">message context.</param>
        /// <param name="result">the result object..</param>
        void Done(object context, Response result);

    }
}
