//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 26SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System.IO;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Util
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 26SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// read big-endian data format.mainly be used as a data adapter for
    /// read java-formated data.
    /// </summary>
    public class DataReader
    {

        ///<summary>
        /// is the data format java or windows( big endian or little endian).
        ///</summary>
        public static bool IsNet;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Read a double value.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <returns>a double value</returns>
        public static double ReadDouble(BinaryReader reader)
        {
            double ret;
            if (!IsNet)
            {

                byte[] buffer = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    buffer[7 - i] = reader.ReadByte();
                }
                MemoryStream bais = new MemoryStream(buffer);
                BinaryReader dis = new BinaryReader(bais);

                ret = dis.ReadDouble();
                dis.Close();
                bais.Close();
            }
            else
            {
                ret = reader.ReadDouble();
            }
            return ret;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reads the long.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>a long value</returns>
        public static long ReadLong(BinaryReader reader)
        {
            long ret;
            if (!IsNet)
            {
                byte[] buffer = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    buffer[7 - i] = reader.ReadByte();
                }
                MemoryStream bais = new MemoryStream(buffer);
                BinaryReader dis = new BinaryReader(bais);
                ret = dis.ReadInt64();
                dis.Close();
                bais.Close();
            }
            else
            {
                ret = reader.ReadInt64();
            }

            return ret;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reads the int.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>a int value</returns>
        public static int ReadInt(BinaryReader reader)
        {
            int ret;
            if (!IsNet)
            {
                byte[] buffer = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    buffer[3 - i] = reader.ReadByte();
                }
                MemoryStream bais = new MemoryStream(buffer);
                BinaryReader dis = new BinaryReader(bais);
                ret = dis.ReadInt32();
                dis.Close();
                bais.Close();
            }
            else
            {
                ret = reader.ReadInt32();
            }
            return ret;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reads the short.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>a short value</returns>
        public static short ReadShort(BinaryReader reader)
        {
            short ret;
            if (!IsNet)
            {
                byte[] buffer = new byte[2];
                for (int i = 0; i < 2; i++)
                {
                    buffer[1 - i] = reader.ReadByte();
                }
                MemoryStream bais = new MemoryStream(buffer);
                BinaryReader dis = new BinaryReader(bais);
                ret = dis.ReadInt16();
                dis.Close();
                bais.Close();
            }
            else
            {
                ret = reader.ReadInt16();
            }
            return ret;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>a string value</returns>
        public static string ReadString(BinaryReader reader)
        {
            string retStr;
            if (!IsNet)
            {
                short len = ReadShort(reader);
                byte[] buffer = reader.ReadBytes(len);
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                Write7BitEncodedInt(len, bw);
                bw.Write(buffer);
                BinaryReader bd = new BinaryReader(ms);
                ms.Seek(0, SeekOrigin.Begin);
                retStr = bd.ReadString();
                bd.Close();
                bw.Close();
                ms.Close();

            }
            else
            {
                retStr = reader.ReadString();
            }
            return retStr;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Seeks the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="offset">The offset.</param>
        public static void Seek(BinaryReader reader, long offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Write7s the bit encoded int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="writer">The writer.</param>
        protected static void Write7BitEncodedInt(int value, BinaryWriter writer)
        {
            // Write out an int 7 bits at a time. The high bit of the byte,
            // when on, tells reader to continue reading more bytes.
            uint v = (uint)value; // support negative numbers
            while (v >= 0x80)
            {
                writer.Write((byte)(v | 0x80));
                v >>= 7;
            }
            writer.Write((byte)v);
        }
    }


    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 26SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// write big-endian data format.mainly be used as a data adapter for
    /// write java-formated data.
    /// </summary>
    public class DataWriter
    {

        ///<summary>
        ///  is the data format java or windows( big endian or little endian).
        ///</summary>
        public static bool IsNet;
       

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// write the long.
        /// </summary>
        /// <param name="writer">The write.</param>
        /// <param name="value">the long value</param>
        /// <returns>a long value</returns>
        public static void WriteLong(BinaryWriter writer,long value)
        {
            if (!IsNet)
            {
                
                MemoryStream bais = new MemoryStream();
                BinaryWriter dis = new BinaryWriter(bais);
                dis.Write(value);
                byte[] buffer = bais.GetBuffer();
                for (int i = 0; i < 8; i++)
                {
                    writer.Write(buffer[7 - i]);
                }
                dis.Close();
                bais.Close();
            }
            else
            {
                writer.Write(value);
            }


        }

        
    }

}
