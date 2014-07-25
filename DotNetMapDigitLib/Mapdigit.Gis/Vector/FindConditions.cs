//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 02OCT2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System.Collections;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Vector
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 02OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Defines a find condition collection when seach for records.
    /// </summary>
    public class FindConditions
    {

        /// <summary>
        /// condition is OR operation.
        /// </summary>
        public const int LogicalOr = 0;
        /// <summary>
        /// condition is AND operation.
        /// </summary>
        public const int LogicalAnd = 1;
        /// <summary>
        /// the Max matching records, default 100;
        /// </summary>
        public static int MaxMatchRecord = 100;
        /// <summary>
        /// the table field defintion.
        /// </summary>
        public DataField[] Fields;
        /**
         * the total conditions.
         */
        private readonly ArrayList _findConditions;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="FindConditions"/> class.
        /// </summary>
        public FindConditions()
        {
            _findConditions = new ArrayList();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clears the condition.
        /// </summary>
        public void ClearCondition()
        {
            _findConditions.Clear();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the condition.
        /// </summary>
        /// <param name="fieldIndex">Index of the field.</param>
        /// <param name="matchString">The match string.</param>
        public void AddCondition(int fieldIndex, string matchString)
        {
            FindCondition condition = new FindCondition(fieldIndex, matchString);
            _findConditions.Add(condition);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the conditions.
        /// </summary>
        /// <returns></returns>
        public ArrayList GetConditions()
        {
            return _findConditions;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the condition.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="matchString">The match string.</param>
        public void AddCondition(string fieldName, string matchString)
        {
            int fieldIndex = 0;
            if (Fields != null)
            {
                for (int i = 0; i < Fields.Length; i++)
                {
                    if (Fields[i].GetName().ToLower().Equals(fieldName.ToLower()))
                    {
                        fieldIndex = i;
                        break;
                    }
                }
            }
            FindCondition condition = new FindCondition(fieldIndex, matchString);
            _findConditions.Add(condition);
        }
    }

}
