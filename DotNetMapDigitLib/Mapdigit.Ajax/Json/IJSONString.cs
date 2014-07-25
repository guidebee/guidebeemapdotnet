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
namespace Mapdigit.Ajax.Json
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 23SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The JSONString interface allows a toJSONString() 
    /// method so that a class can change the behavior of 
    /// JSONObject.toString(), JSONArray.toString(),
    /// and JSONWriter.value(Object). The 
    /// toJSONString method will be used instead of the default behavior 
    /// of using the Object's toString() method and quoting the result.
    /// </summary>
    interface IJSONString
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Toes the JSON string.
        /// </summary>
        /// <returns></returns>
        string ToJSONString();
    }
}
