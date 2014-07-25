//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 20SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Ajax
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 20SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This class encodes a user name and password in the format (base 64) that 
    /// HTTP Basic Authorization requires.
    /// </summary>
    public class BasicAuth
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Encode a name/password pair appropriate to use in an HTTP header for 
        /// Basic Authentication.
        /// </summary>
        /// <param name="name">the user's name.</param>
        /// <param name="passwd">the user's password.</param>
        /// <returns>the base64 encoded name:password</returns>
        public static string Encode(string name,string passwd)
        {
            var input = (name + ":" + passwd).ToCharArray();
            var output = new char[((input.Length / 3) + 1) * 4];
            var ridx = 0;

            /**
             * Loop through input with 3-byte stride. For
             * each 'chunk' of 3-bytes, create a 24-bit
             * value, then extract four 6-bit indices.
             * Use these indices to extract the base-64
             * encoding for this 6-bit 'character'
             */
            for (int i = 0; i < input.Length; i += 3)
            {
                int left = input.Length - i;

                // have at least three bytes of data left
                int chunk;
                if (left > 2)
                {
                    chunk = (input[i] << 16) |
                            (input[i + 1] << 8) |
                             input[i + 2];
                    output[ridx++] = CvtTable[(chunk & 0xFC0000) >> 18];
                    output[ridx++] = CvtTable[(chunk & 0x3F000) >> 12];
                    output[ridx++] = CvtTable[(chunk & 0xFC0) >> 6];
                    output[ridx++] = CvtTable[(chunk & 0x3F)];
                }
                else if (left == 2)
                {
                    // down to 2 bytes. pad with 1 '='
                    chunk = (input[i] << 16) |
                            (input[i + 1] << 8);
                    output[ridx++] = CvtTable[(chunk & 0xFC0000) >> 18];
                    output[ridx++] = CvtTable[(chunk & 0x3F000) >> 12];
                    output[ridx++] = CvtTable[(chunk & 0xFC0) >> 6];
                    output[ridx++] = '=';
                }
                else
                {
                    // down to 1 byte. pad with 2 '='
                    chunk = input[i] << 16;
                    output[ridx++] = CvtTable[(chunk & 0xFC0000) >> 18];
                    output[ridx++] = CvtTable[(chunk & 0x3F000) >> 12];
                    output[ridx++] = '=';
                    output[ridx++] = '=';
                }
            }
            return new string(output);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuth"/> class.
        /// </summary>
        private BasicAuth() { }

        // conversion table
        private static readonly char[] CvtTable = {
        'A', 'B', 'C', 'D', 'E',
        'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O',
        'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y',
        'Z',
        'a', 'b', 'c', 'd', 'e',
        'f', 'g', 'h', 'i', 'j',
        'k', 'l', 'm', 'n', 'o',
        'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y',
        'z',
        '0', '1', '2', '3', '4',
        '5', '6', '7', '8', '9',
        '+', '/'
    };

    }
}
