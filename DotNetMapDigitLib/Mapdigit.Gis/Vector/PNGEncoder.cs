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

using System;
using System.IO;

//--------------------------------- PACKAGE ------------------------------------

namespace Mapdigit.Gis.Vector
{
    /// <summary>
    ///  PNG Encoder for J2ME
    /// </summary>
    internal static class PngEncoder
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a PNG stored in a byte array from the supplied values.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="rgb">The RGB.</param>
        /// <returns>a byte array containing PNG data</returns>
        public static byte[] GetPngrgb(int width, int height, int[] rgb)
        {
            PngEncoderInternal pngEncoderInternal = new PngEncoderInternal(rgb, width, height, true,
                                                                           PngEncoderInternal.FilterNone, 5);
            return pngEncoderInternal.Encode(true);
        }
    }

    /// <summary>
    /// Interface to compute a data checksum used by checked input/output streams.
    /// A data checksum can be updated by one byte or with a byte array. After each
    /// update the value of the current checksum can be returned by calling
    /// <code>getValue</code>. The complete checksum object can also be reset
    /// so it can be used again with new data.
    /// </summary>
    interface IChecksum
    {
        /// <summary>
        /// Returns the data checksum computed so far.
        /// </summary>
        long Value
        {
            get;
        }

        /// <summary>
        /// Resets the data checksum as if no update was ever called.
        /// </summary>
        void Reset();

        /// <summary>
        /// Adds one byte to the data checksum.
        /// </summary>
        /// <param name = "value">
        /// the data value to add. The high byte of the int is ignored.
        /// </param>
        void Update(int value);

        /// <summary>
        /// Updates the data checksum with the bytes taken from the array.
        /// </summary>
        /// <param name="buffer">
        /// buffer an array of bytes
        /// </param>
        void Update(byte[] buffer);

        /// <summary>
        /// Adds the byte array to the data checksum.
        /// </summary>
        /// <param name = "buffer">
        /// The buffer which contains the data
        /// </param>
        /// <param name = "offset">
        /// The offset in the buffer where the data starts
        /// </param>
        /// <param name = "count">
        /// the number of data bytes to add.
        /// </param>
        void Update(byte[] buffer, int offset, int count);
    }

    /// <summary>
    /// Computes Adler32 checksum for a stream of data. An Adler32
    /// checksum is not as reliable as a CRC32 checksum, but a lot faster to
    /// compute.
    /// 
    /// The specification for Adler32 may be found in RFC 1950.
    /// ZLIB Compressed Data Format Specification version 3.3)
    /// 
    /// 
    /// From that document:
    /// 
    ///      "ADLER32 (Adler-32 checksum)
    ///       This contains a checksum value of the uncompressed data
    ///       (excluding any dictionary data) computed according to Adler-32
    ///       algorithm. This algorithm is a 32-bit extension and improvement
    ///       of the Fletcher algorithm, used in the ITU-T X.224 / ISO 8073
    ///       standard.
    /// 
    ///       Adler-32 is composed of two sums accumulated per byte: s1 is
    ///       the sum of all bytes, s2 is the sum of all s1 values. Both sums
    ///       are done modulo 65521. s1 is initialized to 1, s2 to zero.  The
    ///       Adler-32 checksum is stored as s2*65536 + s1 in most-
    ///       significant-byte first (network) order."
    /// 
    ///  "8.2. The Adler-32 algorithm
    /// 
    ///    The Adler-32 algorithm is much faster than the CRC32 algorithm yet
    ///    still provides an extremely low probability of undetected errors.
    /// 
    ///    The modulo on unsigned long accumulators can be delayed for 5552
    ///    bytes, so the modulo operation time is negligible.  If the bytes
    ///    are a, b, c, the second sum is 3a + 2b + c + 3, and so is position
    ///    and order sensitive, unlike the first sum, which is just a
    ///    checksum.  That 65521 is prime is important to avoid a possible
    ///    large class of two-byte errors that leave the check unchanged.
    ///    (The Fletcher checksum uses 255, which is not prime and which also
    ///    makes the Fletcher check insensitive to single byte changes 0 -
    ///    255.)
    /// 
    ///    The sum s1 is initialized to 1 instead of zero to make the length
    ///    of the sequence part of s2, so that the length does not have to be
    ///    checked separately. (Any sequence of zeroes has a Fletcher
    ///    checksum of zero.)"
    /// </summary>
    internal sealed class Adler32 : IChecksum
    {
        /// <summary>
        /// largest prime smaller than 65536
        /// </summary>
        const uint BASE = 65521;

        /// <summary>
        /// Returns the Adler32 data checksum computed so far.
        /// </summary>
        public long Value
        {
            get
            {
                return _checksum;
            }
        }

        /// <summary>
        /// Creates a new instance of the Adler32 class.
        /// The checksum starts off with a value of 1.
        /// </summary>
        public Adler32()
        {
            Reset();
        }

        /// <summary>
        /// Resets the Adler32 checksum to the initial value.
        /// </summary>
        public void Reset()
        {
            _checksum = 1;
        }

        /// <summary>
        /// Updates the checksum with a byte value.
        /// </summary>
        /// <param name="value">
        /// The data value to add. The high byte of the int is ignored.
        /// </param>
        public void Update(int value)
        {
            // We could make a length 1 byte array and call update again, but I
            // would rather not have that overhead
            uint s1 = _checksum & 0xFFFF;
            uint s2 = _checksum >> 16;

            s1 = (s1 + ((uint)value & 0xFF)) % BASE;
            s2 = (s1 + s2) % BASE;

            _checksum = (s2 << 16) + s1;
        }

        /// <summary>
        /// Updates the checksum with an array of bytes.
        /// </summary>
        /// <param name="buffer">
        /// The source of the data to update with.
        /// </param>
        public void Update(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            Update(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Updates the checksum with the bytes taken from the array.
        /// </summary>
        /// <param name="buffer">
        /// an array of bytes
        /// </param>
        /// <param name="offset">
        /// the start of the data used for this update
        /// </param>
        /// <param name="count">
        /// the number of bytes to use for this update
        /// </param>
        public void Update(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (offset < 0)
            {

                throw new ArgumentOutOfRangeException("offset");

            }

            if (count < 0)
            {

                throw new ArgumentOutOfRangeException("count");

            }

            if (offset >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException("offset");

            }

            if (offset + count > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            //(By Per Bothner)
            uint s1 = _checksum & 0xFFFF;
            uint s2 = _checksum >> 16;

            while (count > 0)
            {
                // We can defer the modulo operation:
                // s1 maximally grows from 65521 to 65521 + 255 * 3800
                // s2 maximally grows by 3800 * median(s1) = 2090079800 < 2^31
                int n = 3800;
                if (n > count)
                {
                    n = count;
                }
                count -= n;
                while (--n >= 0)
                {
                    s1 = s1 + (uint)(buffer[offset++] & 0xff);
                    s2 = s2 + s1;
                }
                s1 %= BASE;
                s2 %= BASE;
            }

            _checksum = (s2 << 16) | s1;
        }

        #region Instance Fields
        uint _checksum;
        #endregion
    }

    /// <summary>
    /// Generate a table for a byte-wise 32-bit CRC calculation on the polynomial:
    /// x^32+x^26+x^23+x^22+x^16+x^12+x^11+x^10+x^8+x^7+x^5+x^4+x^2+x+1.
    ///
    /// Polynomials over GF(2) are represented in binary, one bit per coefficient,
    /// with the lowest powers in the most significant bit.  Then adding polynomials
    /// is just exclusive-or, and multiplying a polynomial by x is a right shift by
    /// one.  If we call the above polynomial p, and represent a byte as the
    /// polynomial q, also with the lowest power in the most significant bit (so the
    /// byte 0xb1 is the polynomial x^7+x^3+x+1), then the CRC is (q*x^32) mod p,
    /// where a mod b means the remainder after dividing a by b.
    ///
    /// This calculation is done using the shift-register method of multiplying and
    /// taking the remainder.  The register is initialized to zero, and for each
    /// incoming bit, x^32 is added mod p to the register if the bit is a one (where
    /// x^32 mod p is p+x^32 = x^26+...+1), and the register is multiplied mod p by
    /// x (which is shifting right by one and adding x^32 mod p if the bit shifted
    /// out is a one).  We start with the highest power (least significant bit) of
    /// q and repeat for all eight bits of q.
    ///
    /// The table is simply the CRC of all possible eight bit values.  This is all
    /// the information needed to generate CRC's on data a byte at a time for all
    /// combinations of CRC register values and incoming bytes.
    /// </summary>
    internal sealed class Crc32 : IChecksum
    {
        const uint CrcSeed = 0xFFFFFFFF;

        readonly static uint[] CrcTable = new uint[] {
                                                         0x00000000, 0x77073096, 0xEE0E612C, 0x990951BA, 0x076DC419,
                                                         0x706AF48F, 0xE963A535, 0x9E6495A3, 0x0EDB8832, 0x79DCB8A4,
                                                         0xE0D5E91E, 0x97D2D988, 0x09B64C2B, 0x7EB17CBD, 0xE7B82D07,
                                                         0x90BF1D91, 0x1DB71064, 0x6AB020F2, 0xF3B97148, 0x84BE41DE,
                                                         0x1ADAD47D, 0x6DDDE4EB, 0xF4D4B551, 0x83D385C7, 0x136C9856,
                                                         0x646BA8C0, 0xFD62F97A, 0x8A65C9EC, 0x14015C4F, 0x63066CD9,
                                                         0xFA0F3D63, 0x8D080DF5, 0x3B6E20C8, 0x4C69105E, 0xD56041E4,
                                                         0xA2677172, 0x3C03E4D1, 0x4B04D447, 0xD20D85FD, 0xA50AB56B,
                                                         0x35B5A8FA, 0x42B2986C, 0xDBBBC9D6, 0xACBCF940, 0x32D86CE3,
                                                         0x45DF5C75, 0xDCD60DCF, 0xABD13D59, 0x26D930AC, 0x51DE003A,
                                                         0xC8D75180, 0xBFD06116, 0x21B4F4B5, 0x56B3C423, 0xCFBA9599,
                                                         0xB8BDA50F, 0x2802B89E, 0x5F058808, 0xC60CD9B2, 0xB10BE924,
                                                         0x2F6F7C87, 0x58684C11, 0xC1611DAB, 0xB6662D3D, 0x76DC4190,
                                                         0x01DB7106, 0x98D220BC, 0xEFD5102A, 0x71B18589, 0x06B6B51F,
                                                         0x9FBFE4A5, 0xE8B8D433, 0x7807C9A2, 0x0F00F934, 0x9609A88E,
                                                         0xE10E9818, 0x7F6A0DBB, 0x086D3D2D, 0x91646C97, 0xE6635C01,
                                                         0x6B6B51F4, 0x1C6C6162, 0x856530D8, 0xF262004E, 0x6C0695ED,
                                                         0x1B01A57B, 0x8208F4C1, 0xF50FC457, 0x65B0D9C6, 0x12B7E950,
                                                         0x8BBEB8EA, 0xFCB9887C, 0x62DD1DDF, 0x15DA2D49, 0x8CD37CF3,
                                                         0xFBD44C65, 0x4DB26158, 0x3AB551CE, 0xA3BC0074, 0xD4BB30E2,
                                                         0x4ADFA541, 0x3DD895D7, 0xA4D1C46D, 0xD3D6F4FB, 0x4369E96A,
                                                         0x346ED9FC, 0xAD678846, 0xDA60B8D0, 0x44042D73, 0x33031DE5,
                                                         0xAA0A4C5F, 0xDD0D7CC9, 0x5005713C, 0x270241AA, 0xBE0B1010,
                                                         0xC90C2086, 0x5768B525, 0x206F85B3, 0xB966D409, 0xCE61E49F,
                                                         0x5EDEF90E, 0x29D9C998, 0xB0D09822, 0xC7D7A8B4, 0x59B33D17,
                                                         0x2EB40D81, 0xB7BD5C3B, 0xC0BA6CAD, 0xEDB88320, 0x9ABFB3B6,
                                                         0x03B6E20C, 0x74B1D29A, 0xEAD54739, 0x9DD277AF, 0x04DB2615,
                                                         0x73DC1683, 0xE3630B12, 0x94643B84, 0x0D6D6A3E, 0x7A6A5AA8,
                                                         0xE40ECF0B, 0x9309FF9D, 0x0A00AE27, 0x7D079EB1, 0xF00F9344,
                                                         0x8708A3D2, 0x1E01F268, 0x6906C2FE, 0xF762575D, 0x806567CB,
                                                         0x196C3671, 0x6E6B06E7, 0xFED41B76, 0x89D32BE0, 0x10DA7A5A,
                                                         0x67DD4ACC, 0xF9B9DF6F, 0x8EBEEFF9, 0x17B7BE43, 0x60B08ED5,
                                                         0xD6D6A3E8, 0xA1D1937E, 0x38D8C2C4, 0x4FDFF252, 0xD1BB67F1,
                                                         0xA6BC5767, 0x3FB506DD, 0x48B2364B, 0xD80D2BDA, 0xAF0A1B4C,
                                                         0x36034AF6, 0x41047A60, 0xDF60EFC3, 0xA867DF55, 0x316E8EEF,
                                                         0x4669BE79, 0xCB61B38C, 0xBC66831A, 0x256FD2A0, 0x5268E236,
                                                         0xCC0C7795, 0xBB0B4703, 0x220216B9, 0x5505262F, 0xC5BA3BBE,
                                                         0xB2BD0B28, 0x2BB45A92, 0x5CB36A04, 0xC2D7FFA7, 0xB5D0CF31,
                                                         0x2CD99E8B, 0x5BDEAE1D, 0x9B64C2B0, 0xEC63F226, 0x756AA39C,
                                                         0x026D930A, 0x9C0906A9, 0xEB0E363F, 0x72076785, 0x05005713,
                                                         0x95BF4A82, 0xE2B87A14, 0x7BB12BAE, 0x0CB61B38, 0x92D28E9B,
                                                         0xE5D5BE0D, 0x7CDCEFB7, 0x0BDBDF21, 0x86D3D2D4, 0xF1D4E242,
                                                         0x68DDB3F8, 0x1FDA836E, 0x81BE16CD, 0xF6B9265B, 0x6FB077E1,
                                                         0x18B74777, 0x88085AE6, 0xFF0F6A70, 0x66063BCA, 0x11010B5C,
                                                         0x8F659EFF, 0xF862AE69, 0x616BFFD3, 0x166CCF45, 0xA00AE278,
                                                         0xD70DD2EE, 0x4E048354, 0x3903B3C2, 0xA7672661, 0xD06016F7,
                                                         0x4969474D, 0x3E6E77DB, 0xAED16A4A, 0xD9D65ADC, 0x40DF0B66,
                                                         0x37D83BF0, 0xA9BCAE53, 0xDEBB9EC5, 0x47B2CF7F, 0x30B5FFE9,
                                                         0xBDBDF21C, 0xCABAC28A, 0x53B39330, 0x24B4A3A6, 0xBAD03605,
                                                         0xCDD70693, 0x54DE5729, 0x23D967BF, 0xB3667A2E, 0xC4614AB8,
                                                         0x5D681B02, 0x2A6F2B94, 0xB40BBE37, 0xC30C8EA1, 0x5A05DF1B,
                                                         0x2D02EF8D
                                                     };

        internal static uint ComputeCrc32(uint oldCrc, byte value)
        {
            return CrcTable[(oldCrc ^ value) & 0xFF] ^ (oldCrc >> 8);
        }

        /// <summary>
        /// The crc data checksum so far.
        /// </summary>
        uint _crc;

        /// <summary>
        /// Returns the CRC32 data checksum computed so far.
        /// </summary>
        public long Value
        {
            get
            {
                return _crc;
            }
            set
            {
                _crc = (uint)value;
            }
        }

        /// <summary>
        /// Resets the CRC32 data checksum as if no update was ever called.
        /// </summary>
        public void Reset()
        {
            _crc = 0;
        }

        /// <summary>
        /// Updates the checksum with the int bval.
        /// </summary>
        /// <param name = "value">
        /// the byte is taken as the lower 8 bits of value
        /// </param>
        public void Update(int value)
        {
            _crc ^= CrcSeed;
            _crc = CrcTable[(_crc ^ value) & 0xFF] ^ (_crc >> 8);
            _crc ^= CrcSeed;
        }

        /// <summary>
        /// Updates the checksum with the bytes taken from the array.
        /// </summary>
        /// <param name="buffer">
        /// buffer an array of bytes
        /// </param>
        public void Update(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            Update(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Adds the byte array to the data checksum.
        /// </summary>
        /// <param name = "buffer">
        /// The buffer which contains the data
        /// </param>
        /// <param name = "offset">
        /// The offset in the buffer where the data starts
        /// </param>
        /// <param name = "count">
        /// The number of data bytes to update the CRC with.
        /// </param>
        public void Update(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (count < 0)
            {

                throw new ArgumentOutOfRangeException("count");
            }

            if (offset < 0 || offset + count > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            _crc ^= CrcSeed;

            while (--count >= 0)
            {
                _crc = CrcTable[(_crc ^ buffer[offset++]) & 0xFF] ^ (_crc >> 8);
            }

            _crc ^= CrcSeed;
        }
    }

    /// <summary>
    /// This class contains constants used for deflation.
    /// </summary>
    internal class DeflaterConstants
    {
        /// <summary>
        /// Set to true to enable debugging
        /// </summary>
        public const bool Debugging = false;

        /// <summary>
        /// Written to Zip file to identify a stored block
        /// </summary>
        public const int StoredBlock = 0;

        /// <summary>
        /// Identifies static tree in Zip file
        /// </summary>
        public const int StaticTrees = 1;

        /// <summary>
        /// Identifies dynamic tree in Zip file
        /// </summary>
        public const int DynTrees = 2;

        /// <summary>
        /// Header flag indicating a preset dictionary for deflation
        /// </summary>
        public const int PresetDict = 0x20;

        /// <summary>
        /// Sets internal buffer sizes for Huffman encoding
        /// </summary>
        public const int DefaultMemLevel = 8;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int MaxMatch = 258;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int MinMatch = 3;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int MaxWbits = 15;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int WSize = 1 << MaxWbits;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int Wmask = WSize - 1;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int HashBits = DefaultMemLevel + 7;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int HashSize = 1 << HashBits;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int HashMask = HashSize - 1;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int HashShift = (HashBits + MinMatch - 1) / MinMatch;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int MinLookahead = MaxMatch + MinMatch + 1;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int MaxDist = WSize - MinLookahead;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int PendingBufSize = 1 << (DefaultMemLevel + 8);

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public static int MaxBlockSize = Math.Min(65535, PendingBufSize - 5);

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int DeflateStored = 0;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int DeflateFast = 1;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public const int DeflateSlow = 2;

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public static int[] GoodLength = { 0, 4, 4, 4, 4, 8, 8, 8, 32, 32 };

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public static int[] MaxLazy = { 0, 4, 5, 6, 4, 16, 16, 32, 128, 258 };

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public static int[] NiceLength = { 0, 8, 16, 32, 16, 32, 128, 128, 258, 258 };

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public static int[] MaxChain = { 0, 4, 8, 32, 16, 32, 128, 256, 1024, 4096 };

        /// <summary>
        /// Internal compression engine constant
        /// </summary>		
        public static int[] ComprFunc = { 0, 1, 1, 1, 1, 2, 2, 2, 2, 2 };

    }

    /// <summary>
    /// Strategies for deflater
    /// </summary>
    internal enum DeflateStrategy
    {
        /// <summary>
        /// The default strategy
        /// </summary>
        Default = 0,

        /// <summary>
        /// This strategy will only allow longer string repetitions.  It is
        /// useful for random data with a small character set.
        /// </summary>
        Filtered = 1,


        /// <summary>
        /// This strategy will not look for string repetitions at all.  It
        /// only encodes with Huffman trees (which means, that more common
        /// characters get a smaller encoding.
        /// </summary>
        HuffmanOnly = 2
    }

    /// <summary>
    /// Low level compression engine for deflate algorithm which uses a 32K sliding window
    /// with secondary compression from Huffman/Shannon-Fano codes.
    /// </summary>
    internal class DeflaterEngine : DeflaterConstants
    {
        #region Constants
        const int TooFar = 4096;
        #endregion

        #region Constructors
        /// <summary>
        /// Construct instance with pending buffer
        /// </summary>
        /// <param name="pending">
        /// Pending buffer to use
        /// </param>>
        public DeflaterEngine(DeflaterPending pending)
        {
            _pending = pending;
            _huffman = new DeflaterHuffman(pending);
            _adler = new Adler32();

            _window = new byte[2 * WSize];
            _head = new short[HashSize];
            _prev = new short[WSize];

            // We start at index 1, to avoid an implementation deficiency, that
            // we cannot build a repeat pattern at index 0.
            _blockStart = _strstart = 1;
        }

        #endregion

        /// <summary>
        /// Deflate drives actual compression of data
        /// </summary>
        /// <param name="flush">True to flush input buffers</param>
        /// <param name="finish">Finish deflation with the current input.</param>
        /// <returns>Returns true if progress has been made.</returns>
        public bool Deflate(bool flush, bool finish)
        {
            bool progress;
            do
            {
                FillWindow();
                bool canFlush = flush && (_inputOff == _inputEnd);

                switch (_compressionFunction)
                {
                    case DeflateStored:
                        progress = PerformDeflateStored(canFlush, finish);
                        break;
                    case DeflateFast:
                        progress = PerformDeflateFast(canFlush, finish);
                        break;
                    case DeflateSlow:
                        progress = PerformDeflateSlow(canFlush, finish);
                        break;
                    default:
                        throw new InvalidOperationException("unknown compressionFunction");
                }
            } while (_pending.IsFlushed && progress); // repeat while we have no pending output and progress was made
            return progress;
        }

        /// <summary>
        /// Sets input data to be deflated.  Should only be called when <code>NeedsInput()</code>
        /// returns true
        /// </summary>
        /// <param name="buffer">The buffer containing input data.</param>
        /// <param name="offset">The offset of the first byte of data.</param>
        /// <param name="count">The number of bytes of data to use as input.</param>
        public void SetInput(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            if (_inputOff < _inputEnd)
            {
                throw new InvalidOperationException("Old input was not completely processed");
            }

            int end = offset + count;

            /* We want to throw an ArrayIndexOutOfBoundsException early.  The
            * check is very tricky: it also handles integer wrap around.
            */
            if ((offset > end) || (end > buffer.Length))
            {
                throw new ArgumentOutOfRangeException("count");
            }

            _inputBuf = buffer;
            _inputOff = offset;
            _inputEnd = end;
        }

        /// <summary>
        /// Determines if more <see cref="SetInput">input</see> is needed.
        /// </summary>		
        /// <returns>Return true if input is needed via <see cref="SetInput">SetInput</see></returns>
        public bool NeedsInput()
        {
            return (_inputEnd == _inputOff);
        }

        /// <summary>
        /// Set compression dictionary
        /// </summary>
        /// <param name="buffer">The buffer containing the dictionary data</param>
        /// <param name="offset">The offset in the buffer for the first byte of data</param>
        /// <param name="length">The length of the dictionary data.</param>
        public void SetDictionary(byte[] buffer, int offset, int length)
        {
            _adler.Update(buffer, offset, length);
            if (length < MinMatch)
            {
                return;
            }

            if (length > MaxDist)
            {
                offset += length - MaxDist;
                length = MaxDist;
            }

            Array.Copy(buffer, offset, _window, _strstart, length);

            UpdateHash();
            --length;
            while (--length > 0)
            {
                InsertString();
                _strstart++;
            }
            _strstart += 2;
            _blockStart = _strstart;
        }

        /// <summary>
        /// Reset internal state
        /// </summary>		
        public void Reset()
        {
            _huffman.Reset();
            _adler.Reset();
            _blockStart = _strstart = 1;
            _lookahead = 0;
            _totalIn = 0;
            _prevAvailable = false;
            _matchLen = MinMatch - 1;

            for (int i = 0; i < HashSize; i++)
            {
                _head[i] = 0;
            }

            for (int i = 0; i < WSize; i++)
            {
                _prev[i] = 0;
            }
        }

        /// <summary>
        /// Reset Adler checksum
        /// </summary>		
        public void ResetAdler()
        {
            _adler.Reset();
        }

        /// <summary>
        /// Get current value of Adler checksum
        /// </summary>		
        public int Adler
        {
            get
            {
                return unchecked((int)_adler.Value);
            }
        }

        /// <summary>
        /// Total data processed
        /// </summary>		
        public long TotalIn
        {
            get
            {
                return _totalIn;
            }
        }

        /// <summary>
        /// Get/set the <see cref="DeflateStrategy">deflate strategy</see>
        /// </summary>		
        public DeflateStrategy Strategy
        {
            get
            {
                return _strategy;
            }
            set
            {
                _strategy = value;
            }
        }

        /// <summary>
        /// Set the deflate level (0-9)
        /// </summary>
        /// <param name="level">The value to set the level to.</param>
        public void SetLevel(int level)
        {
            if ((level < 0) || (level > 9))
            {
                throw new ArgumentOutOfRangeException("level");
            }

            _goodLength = GoodLength[level];
            _maxLazy = MaxLazy[level];
            _niceLength = NiceLength[level];
            _maxChain = MaxChain[level];

            if (ComprFunc[level] != _compressionFunction)
            {

                switch (_compressionFunction)
                {
                    case DeflateStored:
                        if (_strstart > _blockStart)
                        {
                            _huffman.FlushStoredBlock(_window, _blockStart,
                                                      _strstart - _blockStart, false);
                            _blockStart = _strstart;
                        }
                        UpdateHash();
                        break;

                    case DeflateFast:
                        if (_strstart > _blockStart)
                        {
                            _huffman.FlushBlock(_window, _blockStart, _strstart - _blockStart,
                                                false);
                            _blockStart = _strstart;
                        }
                        break;

                    case DeflateSlow:
                        if (_prevAvailable)
                        {
                            _huffman.TallyLit(_window[_strstart - 1] & 0xff);
                        }
                        if (_strstart > _blockStart)
                        {
                            _huffman.FlushBlock(_window, _blockStart, _strstart - _blockStart, false);
                            _blockStart = _strstart;
                        }
                        _prevAvailable = false;
                        _matchLen = MinMatch - 1;
                        break;
                }
                _compressionFunction = ComprFunc[level];
            }
        }

        /// <summary>
        /// Fill the window
        /// </summary>
        public void FillWindow()
        {
            /* If the window is almost full and there is insufficient lookahead,
             * move the upper half to the lower one to make room in the upper half.
             */
            if (_strstart >= WSize + MaxDist)
            {
                SlideWindow();
            }

            /* If there is not enough lookahead, but still some input left,
             * read in the input
             */
            while (_lookahead < MinLookahead && _inputOff < _inputEnd)
            {
                int more = 2 * WSize - _lookahead - _strstart;

                if (more > _inputEnd - _inputOff)
                {
                    more = _inputEnd - _inputOff;
                }

                Array.Copy(_inputBuf, _inputOff, _window, _strstart + _lookahead, more);
                _adler.Update(_inputBuf, _inputOff, more);

                _inputOff += more;
                _totalIn += more;
                _lookahead += more;
            }

            if (_lookahead >= MinMatch)
            {
                UpdateHash();
            }
        }

        void UpdateHash()
        {

            _insH = (_window[_strstart] << HashShift) ^ _window[_strstart + 1];
        }

        /// <summary>
        /// Inserts the current string in the head hash and returns the previous
        /// value for this hash.
        /// </summary>
        /// <returns>The previous hash value</returns>
        int InsertString()
        {
            short match;
            int hash = ((_insH << HashShift) ^ _window[_strstart + (MinMatch - 1)]) & HashMask;

            _prev[_strstart & Wmask] = match = _head[hash];
            _head[hash] = unchecked((short)_strstart);
            _insH = hash;
            return match & 0xffff;
        }

        void SlideWindow()
        {
            Array.Copy(_window, WSize, _window, 0, WSize);
            _matchStart -= WSize;
            _strstart -= WSize;
            _blockStart -= WSize;

            // Slide the hash table (could be avoided with 32 bit values
            // at the expense of memory usage).
            for (int i = 0; i < HashSize; ++i)
            {
                int m = _head[i] & 0xffff;
                _head[i] = (short)(m >= WSize ? (m - WSize) : 0);
            }

            // Slide the prev table.
            for (int i = 0; i < WSize; i++)
            {
                int m = _prev[i] & 0xffff;
                _prev[i] = (short)(m >= WSize ? (m - WSize) : 0);
            }
        }

        /// <summary>
        /// Find the best (longest) string in the window matching the 
        /// string starting at strstart.
        ///
        /// Preconditions:
        /// <code>
        /// strstart + MaxMatch &lt;= window.length.</code>
        /// </summary>
        /// <param name="curMatch"></param>
        /// <returns>True if a match greater than the minimum length is found</returns>
        bool FindLongestMatch(int curMatch)
        {
            int chainLength = _maxChain;
            int length = _niceLength;
            short[] prev1 = _prev;
            int scan = _strstart;
            int bestEnd = _strstart + _matchLen;
            int bestLen = Math.Max(_matchLen, MinMatch - 1);

            int limit = Math.Max(_strstart - MaxDist, 0);

            int strend = _strstart + MaxMatch - 1;
            byte scanEnd1 = _window[bestEnd - 1];
            byte scanEnd = _window[bestEnd];

            // Do not waste too much time if we already have a good match:
            if (bestLen >= _goodLength)
            {
                chainLength >>= 2;
            }

            /* Do not look for matches beyond the end of the input. This is necessary
            * to make deflate deterministic.
            */
            if (length > _lookahead)
            {
                length = _lookahead;
            }

#if DebugDeflation

			if (DeflaterConstants.DEBUGGING && (strstart > 2 * WSIZE - MIN_LOOKAHEAD))
			{
				throw new InvalidOperationException("need lookahead");
			}
#endif

            do
            {

#if DebugDeflation

				if (DeflaterConstants.DEBUGGING && (curMatch >= strstart) )
				{
					throw new InvalidOperationException("no future");
				}
#endif
                if (_window[curMatch + bestLen] != scanEnd ||
                    _window[curMatch + bestLen - 1] != scanEnd1 ||
                    _window[curMatch] != _window[scan] ||
                    _window[curMatch + 1] != _window[scan + 1])
                {
                    continue;
                }

                int match = curMatch + 2;
                scan += 2;

                /* We check for insufficient lookahead only every 8th comparison;
                * the 256th check will be made at strstart + 258.
                */
                while (
                    _window[++scan] == _window[++match] &&
                    _window[++scan] == _window[++match] &&
                    _window[++scan] == _window[++match] &&
                    _window[++scan] == _window[++match] &&
                    _window[++scan] == _window[++match] &&
                    _window[++scan] == _window[++match] &&
                    _window[++scan] == _window[++match] &&
                    _window[++scan] == _window[++match] &&
                    (scan < strend))
                {
                    // Do nothing
                }

                if (scan > bestEnd)
                {
#if DebugDeflation
					if (DeflaterConstants.DEBUGGING && (ins_h == 0) )
						Console.Error.WriteLine("Found match: " + curMatch + "-" + (scan - strstart));
#endif
                    _matchStart = curMatch;
                    bestEnd = scan;
                    bestLen = scan - _strstart;

                    if (bestLen >= length)
                    {
                        break;
                    }

                    scanEnd1 = _window[bestEnd - 1];
                    scanEnd = _window[bestEnd];
                }
                scan = _strstart;
            } while ((curMatch = (prev1[curMatch & Wmask] & 0xffff)) > limit && --chainLength != 0);

            _matchLen = Math.Min(bestLen, _lookahead);
            return _matchLen >= MinMatch;
        }

        bool PerformDeflateStored(bool flush, bool finish)
        {
            if (!flush && (_lookahead == 0))
            {
                return false;
            }

            _strstart += _lookahead;
            _lookahead = 0;

            int storedLength = _strstart - _blockStart;

            if ((storedLength >= MaxBlockSize) || // Block is full
                (_blockStart < WSize && storedLength >= MaxDist) ||   // Block may move out of window
                flush)
            {
                bool lastBlock = finish;
                if (storedLength > MaxBlockSize)
                {
                    storedLength = MaxBlockSize;
                    lastBlock = false;
                }



                _huffman.FlushStoredBlock(_window, _blockStart, storedLength, lastBlock);
                _blockStart += storedLength;
                return !lastBlock;
            }
            return true;
        }

        bool PerformDeflateFast(bool flush, bool finish)
        {
            if (_lookahead < MinLookahead && !flush)
            {
                return false;
            }

            while (_lookahead >= MinLookahead || flush)
            {
                if (_lookahead == 0)
                {
                    // We are flushing everything
                    _huffman.FlushBlock(_window, _blockStart, _strstart - _blockStart, finish);
                    _blockStart = _strstart;
                    return false;
                }

                if (_strstart > 2 * WSize - MinLookahead)
                {
                    /* slide window, as FindLongestMatch needs this.
                     * This should only happen when flushing and the window
                     * is almost full.
                     */
                    SlideWindow();
                }

                int hashHead;
                if (_lookahead >= MinMatch &&
                    (hashHead = InsertString()) != 0 &&
                    _strategy != DeflateStrategy.HuffmanOnly &&
                    _strstart - hashHead <= MaxDist &&
                    FindLongestMatch(hashHead))
                {
                    // longestMatch sets matchStart and matchLen

                    bool full = _huffman.TallyDist(_strstart - _matchStart, _matchLen);

                    _lookahead -= _matchLen;
                    if (_matchLen <= _maxLazy && _lookahead >= MinMatch)
                    {
                        while (--_matchLen > 0)
                        {
                            ++_strstart;
                            InsertString();
                        }
                        ++_strstart;
                    }
                    else
                    {
                        _strstart += _matchLen;
                        if (_lookahead >= MinMatch - 1)
                        {
                            UpdateHash();
                        }
                    }
                    _matchLen = MinMatch - 1;
                    if (!full)
                    {
                        continue;
                    }
                }
                else
                {
                    // No match found
                    _huffman.TallyLit(_window[_strstart] & 0xff);
                    ++_strstart;
                    --_lookahead;
                }

                if (_huffman.IsFull())
                {
                    bool lastBlock = finish && (_lookahead == 0);
                    _huffman.FlushBlock(_window, _blockStart, _strstart - _blockStart, lastBlock);
                    _blockStart = _strstart;
                    return !lastBlock;
                }
            }
            return true;
        }

        bool PerformDeflateSlow(bool flush, bool finish)
        {
            if (_lookahead < MinLookahead && !flush)
            {
                return false;
            }

            while (_lookahead >= MinLookahead || flush)
            {
                if (_lookahead == 0)
                {
                    if (_prevAvailable)
                    {
                        _huffman.TallyLit(_window[_strstart - 1] & 0xff);
                    }
                    _prevAvailable = false;

                    // We are flushing everything
#if DebugDeflation
					if (DeflaterConstants.DEBUGGING && !flush) 
					{
						throw new SharpZipBaseException("Not flushing, but no lookahead");
					}
#endif
                    _huffman.FlushBlock(_window, _blockStart, _strstart - _blockStart,
                                        finish);
                    _blockStart = _strstart;
                    return false;
                }

                if (_strstart >= 2 * WSize - MinLookahead)
                {
                    /* slide window, as FindLongestMatch needs this.
                     * This should only happen when flushing and the window
                     * is almost full.
                     */
                    SlideWindow();
                }

                int prevMatch = _matchStart;
                int prevLen = _matchLen;
                if (_lookahead >= MinMatch)
                {

                    int hashHead = InsertString();

                    if (_strategy != DeflateStrategy.HuffmanOnly &&
                        hashHead != 0 &&
                        _strstart - hashHead <= MaxDist &&
                        FindLongestMatch(hashHead))
                    {

                        // longestMatch sets matchStart and matchLen

                        // Discard match if too small and too far away
                        if (_matchLen <= 5 && (_strategy == DeflateStrategy.Filtered || (_matchLen == MinMatch && _strstart - _matchStart > TooFar)))
                        {
                            _matchLen = MinMatch - 1;
                        }
                    }
                }

                // previous match was better
                if ((prevLen >= MinMatch) && (_matchLen <= prevLen))
                {
#if DebugDeflation
					if (DeflaterConstants.DEBUGGING) 
					{
					   for (int i = 0 ; i < matchLen; i++) {
						  if (window[strstart-1+i] != window[prevMatch + i])
							 throw new SharpZipBaseException();
						}
					}
#endif
                    _huffman.TallyDist(_strstart - 1 - prevMatch, prevLen);
                    prevLen -= 2;
                    do
                    {
                        _strstart++;
                        _lookahead--;
                        if (_lookahead >= MinMatch)
                        {
                            InsertString();
                        }
                    } while (--prevLen > 0);

                    _strstart++;
                    _lookahead--;
                    _prevAvailable = false;
                    _matchLen = MinMatch - 1;
                }
                else
                {
                    if (_prevAvailable)
                    {
                        _huffman.TallyLit(_window[_strstart - 1] & 0xff);
                    }
                    _prevAvailable = true;
                    _strstart++;
                    _lookahead--;
                }

                if (_huffman.IsFull())
                {
                    int len = _strstart - _blockStart;
                    if (_prevAvailable)
                    {
                        len--;
                    }
                    bool lastBlock = (finish && (_lookahead == 0) && !_prevAvailable);
                    _huffman.FlushBlock(_window, _blockStart, len, lastBlock);
                    _blockStart += len;
                    return !lastBlock;
                }
            }
            return true;
        }

        #region Instance Fields

        // Hash index of string to be inserted
        int _insH;

        /// <summary>
        /// Hashtable, hashing three characters to an index for window, so
        /// that window[index]..window[index+2] have this hash code.  
        /// Note that the array should really be unsigned short, so you need
        /// to and the values with 0xffff.
        /// </summary>
        readonly short[] _head;

        /// <summary>
        /// <code>prev[index &amp; Wmask]</code> points to the previous index that has the
        /// same hash code as the string starting at index.  This way 
        /// entries with the same hash code are in a linked list.
        /// Note that the array should really be unsigned short, so you need
        /// to and the values with 0xffff.
        /// </summary>
        readonly short[] _prev;

        int _matchStart;
        // Length of best match
        int _matchLen;
        // Set if previous match exists
        bool _prevAvailable;
        int _blockStart;

        /// <summary>
        /// Points to the current character in the window.
        /// </summary>
        int _strstart;

        /// <summary>
        /// lookahead is the number of characters starting at strstart in
        /// window that are valid.
        /// So window[strstart] until window[strstart+lookahead-1] are valid
        /// characters.
        /// </summary>
        int _lookahead;

        /// <summary>
        /// This array contains the part of the uncompressed stream that 
        /// is of relevance.  The current character is indexed by strstart.
        /// </summary>
        readonly byte[] _window;

        DeflateStrategy _strategy;
        int _maxChain, _maxLazy, _niceLength, _goodLength;

        /// <summary>
        /// The current compression function.
        /// </summary>
        int _compressionFunction;

        /// <summary>
        /// The input data for compression.
        /// </summary>
        byte[] _inputBuf;

        /// <summary>
        /// The total bytes of input read.
        /// </summary>
        long _totalIn;

        /// <summary>
        /// The offset into inputBuf, where input data starts.
        /// </summary>
        int _inputOff;

        /// <summary>
        /// The end offset of the input data.
        /// </summary>
        int _inputEnd;

        readonly DeflaterPending _pending;
        readonly DeflaterHuffman _huffman;

        /// <summary>
        /// The adler checksum
        /// </summary>
        readonly Adler32 _adler;
        #endregion
    }

    /// <summary>
    /// This is the Deflater class.  The deflater class compresses input
    /// with the deflate algorithm described in RFC 1951.  It has several
    /// compression levels and three different strategies described below.
    ///
    /// This class is <i>not</i> thread safe.  This is inherent in the API, due
    /// to the split of deflate and setInput.
    /// 
    /// author of the original java version : Jochen Hoenicke
    /// </summary>
    internal class Deflater
    {
        #region Deflater Documentation
        /*
		* The Deflater can do the following state transitions:
		*
		* (1) -> InitState   ----> INIT_FINISHING_STATE ---.
		*        /  | (2)      (5)                          |
		*       /   v          (5)                          |
		*   (3)| SetdictState ---> SETDICT_FINISHING_STATE |(3)
		*       \   | (3)                 |        ,--------'
		*        |  |                     | (3)   /
		*        v  v          (5)        v      v
		* (1) -> BusyState   ----> FinishingState
		*                                | (6)
		*                                v
		*                           FinishedState
		*    \_____________________________________/
		*                    | (7)
		*                    v
		*               ClosedState
		*
		* (1) If we should produce a header we start in InitState, otherwise
		*     we start in BusyState.
		* (2) A dictionary may be set only when we are in InitState, then
		*     we change the state as indicated.
		* (3) Whether a dictionary is set or not, on the first call of deflate
		*     we change to BusyState.
		* (4) -- intentionally left blank -- :)
		* (5) FinishingState is entered, when flush() is called to indicate that
		*     there is no more INPUT.  There are also states indicating, that
		*     the header wasn't written yet.
		* (6) FinishedState is entered, when everything has been flushed to the
		*     internal pending output buffer.
		* (7) At any time (7)
		*
		*/
        #endregion
        #region Public Constants
        /// <summary>
        /// The best and slowest compression level.  This tries to find very
        /// long and distant string repetitions.
        /// </summary>
        public const int BestCompression = 9;

        /// <summary>
        /// The worst but fastest compression level.
        /// </summary>
        public const int BestSpeed = 1;

        /// <summary>
        /// The default compression level.
        /// </summary>
        public const int DefaultCompression = -1;

        /// <summary>
        /// This level won't compress at all but output uncompressed blocks.
        /// </summary>
        public const int NoCompression = 0;

        /// <summary>
        /// The compression method.  This is the only method supported so far.
        /// There is no need to use this constant at all.
        /// </summary>
        public const int Deflated = 8;
        #endregion
        #region Local Constants
        private const int IsSetdict = 0x01;
        private const int IsFlushing = 0x04;
        private const int IsFinishing = 0x08;

        private const int InitState = 0x00;
        private const int SetdictState = 0x01;
        private const int BusyState = 0x10;
        private const int FlushingState = 0x14;
        private const int FinishingState = 0x1c;
        private const int FinishedState = 0x1e;
        private const int ClosedState = 0x7f;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new deflater with default compression level.
        /// </summary>
        public Deflater()
            : this(DefaultCompression, false)
        {

        }

        /// <summary>
        /// Creates a new deflater with given compression level.
        /// </summary>
        /// <param name="level">
        /// the compression level, a value between NoCompression
        /// and BestCompression, or DEFAULT_COMPRESSION.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">if lvl is out of range.</exception>
        public Deflater(int level)
            : this(level, false)
        {

        }

        /// <summary>
        /// Creates a new deflater with given compression level.
        /// </summary>
        /// <param name="level">
        /// the compression level, a value between NoCompression
        /// and BestCompression.
        /// </param>
        /// <param name="noZlibHeaderOrFooter">
        /// true, if we should suppress the Zlib/RFC1950 header at the
        /// beginning and the adler checksum at the end of the output.  This is
        /// useful for the GZIP/PKZIP formats.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">if lvl is out of range.</exception>
        public Deflater(int level, bool noZlibHeaderOrFooter)
        {
            if (level == DefaultCompression)
            {
                level = 6;
            }
            else if (level < NoCompression || level > BestCompression)
            {
                throw new ArgumentOutOfRangeException("level");
            }

            _pending = new DeflaterPending();
            _engine = new DeflaterEngine(_pending);
            _noZlibHeaderOrFooter = noZlibHeaderOrFooter;
            SetStrategy(DeflateStrategy.Default);
            SetLevel(level);
            Reset();
        }
        #endregion

        /// <summary>
        /// Resets the deflater.  The deflater acts afterwards as if it was
        /// just created with the same compression level and strategy as it
        /// had before.
        /// </summary>
        public void Reset()
        {
            _state = (_noZlibHeaderOrFooter ? BusyState : InitState);
            _totalOut = 0;
            _pending.Reset();
            _engine.Reset();
        }

        /// <summary>
        /// Gets the current adler checksum of the data that was processed so far.
        /// </summary>
        public int Adler
        {
            get
            {
                return _engine.Adler;
            }
        }

        /// <summary>
        /// Gets the number of input bytes processed so far.
        /// </summary>
        public long TotalIn
        {
            get
            {
                return _engine.TotalIn;
            }
        }

        /// <summary>
        /// Gets the number of output bytes so far.
        /// </summary>
        public long TotalOut
        {
            get
            {
                return _totalOut;
            }
        }

        /// <summary>
        /// Flushes the current input block.  Further calls to deflate() will
        /// produce enough output to inflate everything in the current input
        /// block.  This is not part of Sun's JDK so I have made it package
        /// private.  It is used by DeflaterOutputStream to implement
        /// flush().
        /// </summary>
        public void Flush()
        {
            _state |= IsFlushing;
        }

        /// <summary>
        /// Finishes the deflater with the current input block.  It is an error
        /// to give more input after this method was called.  This method must
        /// be called to force all bytes to be flushed.
        /// </summary>
        public void Finish()
        {
            _state |= (IsFlushing | IsFinishing);
        }

        /// <summary>
        /// Returns true if the stream was finished and no more output bytes
        /// are available.
        /// </summary>
        public bool IsFinished
        {
            get
            {
                return (_state == FinishedState) && _pending.IsFlushed;
            }
        }

        /// <summary>
        /// Returns true, if the input buffer is empty.
        /// You should then call setInput(). 
        /// NOTE: This method can also return true when the stream
        /// was finished.
        /// </summary>
        public bool IsNeedingInput
        {
            get
            {
                return _engine.NeedsInput();
            }
        }

        /// <summary>
        /// Sets the data which should be compressed next.  This should be only
        /// called when needsInput indicates that more input is needed.
        /// If you call setInput when needsInput() returns false, the
        /// previous input that is still pending will be thrown away.
        /// The given byte array should not be changed, before needsInput() returns
        /// true again.
        /// This call is equivalent to <code>setInput(input, 0, input.length)</code>.
        /// </summary>
        /// <param name="input">
        /// the buffer containing the input data.
        /// </param>
        /// <exception cref="System.InvalidOperationException">
        /// if the buffer was finished() or ended().
        /// </exception>
        public void SetInput(byte[] input)
        {
            SetInput(input, 0, input.Length);
        }

        /// <summary>
        /// Sets the data which should be compressed next.  This should be
        /// only called when needsInput indicates that more input is needed.
        /// The given byte array should not be changed, before needsInput() returns
        /// true again.
        /// </summary>
        /// <param name="input">
        /// the buffer containing the input data.
        /// </param>
        /// <param name="offset">
        /// the start of the data.
        /// </param>
        /// <param name="count">
        /// the number of data bytes of input.
        /// </param>
        /// <exception cref="System.InvalidOperationException">
        /// if the buffer was Finish()ed or if previous input is still pending.
        /// </exception>
        public void SetInput(byte[] input, int offset, int count)
        {
            if ((_state & IsFinishing) != 0)
            {
                throw new InvalidOperationException("Finish() already called");
            }
            _engine.SetInput(input, offset, count);
        }

        /// <summary>
        /// Sets the compression level.  There is no guarantee of the exact
        /// position of the change, but if you call this when needsInput is
        /// true the change of compression level will occur somewhere near
        /// before the end of the so far given input.
        /// </summary>
        /// <param name="level">
        /// the new compression level.
        /// </param>
        public void SetLevel(int level)
        {
            if (level == DefaultCompression)
            {
                level = 6;
            }
            else if (level < NoCompression || level > BestCompression)
            {
                throw new ArgumentOutOfRangeException("level");
            }

            if (_level != level)
            {
                _level = level;
                _engine.SetLevel(level);
            }
        }

        /// <summary>
        /// Get current compression level
        /// </summary>
        /// <returns>Returns the current compression level</returns>
        public int GetLevel()
        {
            return _level;
        }

        /// <summary>
        /// Sets the compression strategy. Strategy is one of
        /// DEFAULT_STRATEGY, HUFFMAN_ONLY and FILTERED.  For the exact
        /// position where the strategy is changed, the same as for
        /// SetLevel() applies.
        /// </summary>
        /// <param name="strategy">
        /// The new compression strategy.
        /// </param>
        public void SetStrategy(DeflateStrategy strategy)
        {
            _engine.Strategy = strategy;
        }

        /// <summary>
        /// Deflates the current input block with to the given array.
        /// </summary>
        /// <param name="output">
        /// The buffer where compressed data is stored
        /// </param>
        /// <returns>
        /// The number of compressed bytes added to the output, or 0 if either
        /// IsNeedingInput() or IsFinished returns true or length is zero.
        /// </returns>
        public int Deflate(byte[] output)
        {
            return Deflate(output, 0, output.Length);
        }

        /// <summary>
        /// Deflates the current input block to the given array.
        /// </summary>
        /// <param name="output">
        /// Buffer to store the compressed data.
        /// </param>
        /// <param name="offset">
        /// Offset into the output array.
        /// </param>
        /// <param name="length">
        /// The maximum number of bytes that may be stored.
        /// </param>
        /// <returns>
        /// The number of compressed bytes added to the output, or 0 if either
        /// needsInput() or finished() returns true or length is zero.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// If Finish() was previously called.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If offset or length don't match the array length.
        /// </exception>
        public int Deflate(byte[] output, int offset, int length)
        {
            int origLength = length;

            if (_state == ClosedState)
            {
                throw new InvalidOperationException("Deflater closed");
            }

            if (_state < BusyState)
            {
                // output header
                int header = (Deflated +
                              ((DeflaterConstants.MaxWbits - 8) << 4)) << 8;
                int levelFlags = (_level - 1) >> 1;
                if (levelFlags < 0 || levelFlags > 3)
                {
                    levelFlags = 3;
                }
                header |= levelFlags << 6;
                if ((_state & IsSetdict) != 0)
                {
                    // Dictionary was set
                    header |= DeflaterConstants.PresetDict;
                }
                header += 31 - (header % 31);

                _pending.WriteShortMsb(header);
                if ((_state & IsSetdict) != 0)
                {
                    int chksum = _engine.Adler;
                    _engine.ResetAdler();
                    _pending.WriteShortMsb(chksum >> 16);
                    _pending.WriteShortMsb(chksum & 0xffff);
                }

                _state = BusyState | (_state & (IsFlushing | IsFinishing));
            }

            for (; ; )
            {
                int count = _pending.Flush(output, offset, length);
                offset += count;
                _totalOut += count;
                length -= count;

                if (length == 0 || _state == FinishedState)
                {
                    break;
                }

                if (!_engine.Deflate((_state & IsFlushing) != 0, (_state & IsFinishing) != 0))
                {
                    if (_state == BusyState)
                    {
                        // We need more input now
                        return origLength - length;
                    }
                    if (_state == FlushingState)
                    {
                        if (_level != NoCompression)
                        {
                            /* We have to supply some lookahead.  8 bit lookahead
                             * is needed by the zlib inflater, and we must fill
                             * the next byte, so that all bits are flushed.
                             */
                            int neededbits = 8 + ((-_pending.BitCount) & 7);
                            while (neededbits > 0)
                            {
                                /* write a static tree block consisting solely of
                                 * an EOF:
                                 */
                                _pending.WriteBits(2, 10);
                                neededbits -= 10;
                            }
                        }
                        _state = BusyState;
                    }
                    else if (_state == FinishingState)
                    {
                        _pending.AlignToByte();

                        // Compressed data is complete.  Write footer information if required.
                        if (!_noZlibHeaderOrFooter)
                        {
                            int adler = _engine.Adler;
                            _pending.WriteShortMsb(adler >> 16);
                            _pending.WriteShortMsb(adler & 0xffff);
                        }
                        _state = FinishedState;
                    }
                }
            }
            return origLength - length;
        }

        /// <summary>
        /// Sets the dictionary which should be used in the deflate process.
        /// This call is equivalent to <code>setDictionary(dict, 0, dict.Length)</code>.
        /// </summary>
        /// <param name="dictionary">
        /// the dictionary.
        /// </param>
        /// <exception cref="System.InvalidOperationException">
        /// if SetInput () or Deflate () were already called or another dictionary was already set.
        /// </exception>
        public void SetDictionary(byte[] dictionary)
        {
            SetDictionary(dictionary, 0, dictionary.Length);
        }

        /// <summary>
        /// Sets the dictionary which should be used in the deflate process.
        /// The dictionary is a byte array containing strings that are
        /// likely to occur in the data which should be compressed.  The
        /// dictionary is not stored in the compressed output, only a
        /// checksum.  To decompress the output you need to supply the same
        /// dictionary again.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary data
        /// </param>
        /// <param name="index">
        /// The index where dictionary information commences.
        /// </param>
        /// <param name="count">
        /// The number of bytes in the dictionary.
        /// </param>
        /// <exception cref="System.InvalidOperationException">
        /// If SetInput () or Deflate() were already called or another dictionary was already set.
        /// </exception>
        public void SetDictionary(byte[] dictionary, int index, int count)
        {
            if (_state != InitState)
            {
                throw new InvalidOperationException();
            }

            _state = SetdictState;
            _engine.SetDictionary(dictionary, index, count);
        }

        #region Instance Fields
        /// <summary>
        /// Compression level.
        /// </summary>
        int _level;

        /// <summary>
        /// If true no Zlib/RFC1950 headers or footers are generated
        /// </summary>
        readonly bool _noZlibHeaderOrFooter;

        /// <summary>
        /// The current state.
        /// </summary>
        int _state;

        /// <summary>
        /// The total bytes of output written.
        /// </summary>
        long _totalOut;

        /// <summary>
        /// The pending output.
        /// </summary>
        readonly DeflaterPending _pending;

        /// <summary>
        /// The deflater engine.
        /// </summary>
        readonly DeflaterEngine _engine;
        #endregion
    }

    /// <summary>
    /// This is the DeflaterHuffman class.
    /// 
    /// This class is <i>not</i> thread safe.  This is inherent in the API, due
    /// to the split of Deflate and SetInput.
    /// 
    /// author of the original java version : Jochen Hoenicke
    /// </summary>
    internal class DeflaterHuffman
    {
        const int Bufsize = 1 << (DeflaterConstants.DefaultMemLevel + 6);
        const int LiteralNum = 286;

        // Number of distance codes
        const int DistNum = 30;
        // Number of codes used to transfer bit lengths
        const int BitlenNum = 19;

        // repeat previous bit length 3-6 times (2 bits of repeat count)
        const int Rep36 = 16;
        // repeat a zero length 3-10 times  (3 bits of repeat count)
        const int Rep310 = 17;
        // repeat a zero length 11-138 times  (7 bits of repeat count)
        const int Rep11138 = 18;

        const int EofSymbol = 256;

        // The lengths of the bit length codes are sent in order of decreasing
        // probability, to avoid transmitting the lengths for unused bit length codes.
        static readonly int[] BlOrder = { 16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15 };

        static readonly byte[] Bit4Reverse = {
                                                 0,
                                                 8,
                                                 4,
                                                 12,
                                                 2,
                                                 10,
                                                 6,
                                                 14,
                                                 1,
                                                 9,
                                                 5,
                                                 13,
                                                 3,
                                                 11,
                                                 7,
                                                 15
                                             };

        static readonly short[] StaticLCodes;
        static readonly byte[] StaticLLength;
        static readonly short[] StaticDCodes;
        static readonly byte[] StaticDLength;

        class Tree
        {
            #region Instance Fields
            public readonly short[] Freqs;

            public byte[] Length;

            private readonly int _minNumCodes;

            public int NumCodes;

            short[] _codes;
            readonly int[] _blCounts;
            readonly int _maxLength;
            readonly DeflaterHuffman _dh;
            #endregion

            #region Constructors
            public Tree(DeflaterHuffman dh, int elems, int minCodes, int maxLength)
            {
                _dh = dh;
                _minNumCodes = minCodes;
                _maxLength = maxLength;
                Freqs = new short[elems];
                _blCounts = new int[maxLength];
            }

            #endregion

            /// <summary>
            /// Resets the internal state of the tree
            /// </summary>
            public void Reset()
            {
                for (int i = 0; i < Freqs.Length; i++)
                {
                    Freqs[i] = 0;
                }
                _codes = null;
                Length = null;
            }

            public void WriteSymbol(int code)
            {

                _dh.Pending.WriteBits(_codes[code] & 0xffff, Length[code]);
            }

            /// <summary>
            /// Set static codes and length
            /// </summary>
            /// <param name="staticCodes">new codes</param>
            /// <param name="staticLengths">length for new codes</param>
            public void SetStaticCodes(short[] staticCodes, byte[] staticLengths)
            {
                _codes = staticCodes;
                Length = staticLengths;
            }

            /// <summary>
            /// Build dynamic codes and lengths
            /// </summary>
            public void BuildCodes()
            {
                int[] nextCode = new int[_maxLength];
                int code = 0;

                _codes = new short[Freqs.Length];

                for (int bits = 0; bits < _maxLength; bits++)
                {
                    nextCode[bits] = code;
                    code += _blCounts[bits] << (15 - bits);
                }

                for (int i = 0; i < NumCodes; i++)
                {
                    int bits = Length[i];
                    if (bits > 0)
                    {

                        _codes[i] = BitReverse(nextCode[bits - 1]);
                        nextCode[bits - 1] += 1 << (16 - bits);
                    }
                }
            }

            public void BuildTree()
            {
                int numSymbols = Freqs.Length;

                /* heap is a priority queue, sorted by frequency, least frequent
                * nodes first.  The heap is a binary tree, with the property, that
                * the parent node is smaller than both child nodes.  This assures
                * that the smallest node is the first parent.
                *
                * The binary tree is encoded in an array:  0 is root node and
                * the nodes 2*n+1, 2*n+2 are the child nodes of node n.
                */
                int[] heap = new int[numSymbols];
                int heapLen = 0;
                int maxCode = 0;
                for (int n = 0; n < numSymbols; n++)
                {
                    int freq = Freqs[n];
                    if (freq != 0)
                    {
                        // Insert n into heap
                        int pos = heapLen++;
                        int ppos;
                        while (pos > 0 && Freqs[heap[ppos = (pos - 1) / 2]] > freq)
                        {
                            heap[pos] = heap[ppos];
                            pos = ppos;
                        }
                        heap[pos] = n;

                        maxCode = n;
                    }
                }

                /* We could encode a single literal with 0 bits but then we
                * don't see the literals.  Therefore we force at least two
                * literals to avoid this case.  We don't care about order in
                * this case, both literals get a 1 bit code.
                */
                while (heapLen < 2)
                {
                    int node = maxCode < 2 ? ++maxCode : 0;
                    heap[heapLen++] = node;
                }

                NumCodes = Math.Max(maxCode + 1, _minNumCodes);

                int numLeafs = heapLen;
                int[] childs = new int[4 * heapLen - 2];
                int[] values = new int[2 * heapLen - 1];
                int numNodes = numLeafs;
                for (int i = 0; i < heapLen; i++)
                {
                    int node = heap[i];
                    childs[2 * i] = node;
                    childs[2 * i + 1] = -1;
                    values[i] = Freqs[node] << 8;
                    heap[i] = i;
                }

                /* Construct the Huffman tree by repeatedly combining the least two
                * frequent nodes.
                */
                do
                {
                    int first = heap[0];
                    int last = heap[--heapLen];

                    // Propagate the hole to the leafs of the heap
                    int ppos = 0;
                    int path = 1;

                    while (path < heapLen)
                    {
                        if (path + 1 < heapLen && values[heap[path]] > values[heap[path + 1]])
                        {
                            path++;
                        }

                        heap[ppos] = heap[path];
                        ppos = path;
                        path = path * 2 + 1;
                    }

                    /* Now propagate the last element down along path.  Normally
                    * it shouldn't go too deep.
                    */
                    int lastVal = values[last];
                    while ((path = ppos) > 0 && values[heap[ppos = (path - 1) / 2]] > lastVal)
                    {
                        heap[path] = heap[ppos];
                    }
                    heap[path] = last;


                    int second = heap[0];

                    // Create a new node father of first and second
                    last = numNodes++;
                    childs[2 * last] = first;
                    childs[2 * last + 1] = second;
                    int mindepth = Math.Min(values[first] & 0xff, values[second] & 0xff);
                    values[last] = lastVal = values[first] + values[second] - mindepth + 1;

                    // Again, propagate the hole to the leafs
                    ppos = 0;
                    path = 1;

                    while (path < heapLen)
                    {
                        if (path + 1 < heapLen && values[heap[path]] > values[heap[path + 1]])
                        {
                            path++;
                        }

                        heap[ppos] = heap[path];
                        ppos = path;
                        path = ppos * 2 + 1;
                    }

                    // Now propagate the new element down along path
                    while ((path = ppos) > 0 && values[heap[ppos = (path - 1) / 2]] > lastVal)
                    {
                        heap[path] = heap[ppos];
                    }
                    heap[path] = last;
                } while (heapLen > 1);

                if (heap[0] != childs.Length / 2 - 1)
                {
                    throw new Exception("Heap invariant violated");
                }

                BuildLength(childs);
            }

            /// <summary>
            /// Get encoded length
            /// </summary>
            /// <returns>Encoded length, the sum of frequencies * lengths</returns>
            public int GetEncodedLength()
            {
                int len = 0;
                for (int i = 0; i < Freqs.Length; i++)
                {
                    len += Freqs[i] * Length[i];
                }
                return len;
            }

            /// <summary>
            /// Scan a literal or distance tree to determine the frequencies of the codes
            /// in the bit length tree.
            /// </summary>
            public void CalcBlFreq(Tree blTree)
            {
                int curlen = -1;             /* length of current code */

                int i = 0;
                while (i < NumCodes)
                {
                    int count = 1;                   /* repeat count of the current code */
                    int nextlen = Length[i];
                    int maxCount;               /* max repeat count */
                    int minCount;               /* min repeat count */
                    if (nextlen == 0)
                    {
                        maxCount = 138;
                        minCount = 3;
                    }
                    else
                    {
                        maxCount = 6;
                        minCount = 3;
                        if (curlen != nextlen)
                        {
                            blTree.Freqs[nextlen]++;
                            count = 0;
                        }
                    }
                    curlen = nextlen;
                    i++;

                    while (i < NumCodes && curlen == Length[i])
                    {
                        i++;
                        if (++count >= maxCount)
                        {
                            break;
                        }
                    }

                    if (count < minCount)
                    {
                        blTree.Freqs[curlen] += (short)count;
                    }
                    else if (curlen != 0)
                    {
                        blTree.Freqs[Rep36]++;
                    }
                    else if (count <= 10)
                    {
                        blTree.Freqs[Rep310]++;
                    }
                    else
                    {
                        blTree.Freqs[Rep11138]++;
                    }
                }
            }

            /// <summary>
            /// Write tree values
            /// </summary>
            /// <param name="blTree">Tree to write</param>
            public void WriteTree(Tree blTree)
            {
                int max_count;               // max repeat count
                int min_count;               // min repeat count
                int count;                   // repeat count of the current code
                int curlen = -1;             // length of current code

                int i = 0;
                while (i < NumCodes)
                {
                    count = 1;
                    int nextlen = Length[i];
                    if (nextlen == 0)
                    {
                        max_count = 138;
                        min_count = 3;
                    }
                    else
                    {
                        max_count = 6;
                        min_count = 3;
                        if (curlen != nextlen)
                        {
                            blTree.WriteSymbol(nextlen);
                            count = 0;
                        }
                    }
                    curlen = nextlen;
                    i++;

                    while (i < NumCodes && curlen == Length[i])
                    {
                        i++;
                        if (++count >= max_count)
                        {
                            break;
                        }
                    }

                    if (count < min_count)
                    {
                        while (count-- > 0)
                        {
                            blTree.WriteSymbol(curlen);
                        }
                    }
                    else if (curlen != 0)
                    {
                        blTree.WriteSymbol(Rep36);
                        _dh.Pending.WriteBits(count - 3, 2);
                    }
                    else if (count <= 10)
                    {
                        blTree.WriteSymbol(Rep310);
                        _dh.Pending.WriteBits(count - 3, 3);
                    }
                    else
                    {
                        blTree.WriteSymbol(Rep11138);
                        _dh.Pending.WriteBits(count - 11, 7);
                    }
                }
            }

            void BuildLength(int[] childs)
            {
                Length = new byte[Freqs.Length];
                int numNodes = childs.Length / 2;
                int numLeafs = (numNodes + 1) / 2;
                int overflow = 0;

                for (int i = 0; i < _maxLength; i++)
                {
                    _blCounts[i] = 0;
                }

                // First calculate optimal bit lengths
                int[] lengths = new int[numNodes];
                lengths[numNodes - 1] = 0;

                for (int i = numNodes - 1; i >= 0; i--)
                {
                    if (childs[2 * i + 1] != -1)
                    {
                        int bitLength = lengths[i] + 1;
                        if (bitLength > _maxLength)
                        {
                            bitLength = _maxLength;
                            overflow++;
                        }
                        lengths[childs[2 * i]] = lengths[childs[2 * i + 1]] = bitLength;
                    }
                    else
                    {
                        // A leaf node
                        int bitLength = lengths[i];
                        _blCounts[bitLength - 1]++;
                        Length[childs[2 * i]] = (byte)lengths[i];
                    }
                }


                if (overflow == 0)
                {
                    return;
                }

                int incrBitLen = _maxLength - 1;
                do
                {
                    // Find the first bit length which could increase:
                    while (_blCounts[--incrBitLen] == 0)
                    {
                    }

                    // Move this node one down and remove a corresponding
                    // number of overflow nodes.
                    do
                    {
                        _blCounts[incrBitLen]--;
                        _blCounts[++incrBitLen]++;
                        overflow -= 1 << (_maxLength - 1 - incrBitLen);
                    } while (overflow > 0 && incrBitLen < _maxLength - 1);
                } while (overflow > 0);

                /* We may have overshot above.  Move some nodes from maxLength to
                * maxLength-1 in that case.
                */
                _blCounts[_maxLength - 1] += overflow;
                _blCounts[_maxLength - 2] -= overflow;

                /* Now recompute all bit lengths, scanning in increasing
                * frequency.  It is simpler to reconstruct all lengths instead of
                * fixing only the wrong ones. This idea is taken from 'ar'
                * written by Haruhiko Okumura.
                *
                * The nodes were inserted with decreasing frequency into the childs
                * array.
                */
                int nodePtr = 2 * numLeafs;
                for (int bits = _maxLength; bits != 0; bits--)
                {
                    int n = _blCounts[bits - 1];
                    while (n > 0)
                    {
                        int childPtr = 2 * childs[nodePtr++];
                        if (childs[childPtr + 1] == -1)
                        {
                            // We found another leaf
                            Length[childs[childPtr]] = (byte)bits;
                            n--;
                        }
                    }
                }

            }

        }

        #region Instance Fields
        /// <summary>
        /// Pending buffer to use
        /// </summary>
        public DeflaterPending Pending;

        readonly Tree _literalTree;
        readonly Tree _distTree;
        readonly Tree _blTree;

        // Buffer for distances
        readonly short[] _dBuf;
        readonly byte[] _lBuf;
        int _lastLit;
        int _extraBits;
        #endregion

        static DeflaterHuffman()
        {
            // See RFC 1951 3.2.6
            // Literal codes
            StaticLCodes = new short[LiteralNum];
            StaticLLength = new byte[LiteralNum];

            int i = 0;
            while (i < 144)
            {
                StaticLCodes[i] = BitReverse((0x030 + i) << 8);
                StaticLLength[i++] = 8;
            }

            while (i < 256)
            {
                StaticLCodes[i] = BitReverse((0x190 - 144 + i) << 7);
                StaticLLength[i++] = 9;
            }

            while (i < 280)
            {
                StaticLCodes[i] = BitReverse((0x000 - 256 + i) << 9);
                StaticLLength[i++] = 7;
            }

            while (i < LiteralNum)
            {
                StaticLCodes[i] = BitReverse((0x0c0 - 280 + i) << 8);
                StaticLLength[i++] = 8;
            }

            // Distance codes
            StaticDCodes = new short[DistNum];
            StaticDLength = new byte[DistNum];
            for (i = 0; i < DistNum; i++)
            {
                StaticDCodes[i] = BitReverse(i << 11);
                StaticDLength[i] = 5;
            }
        }

        /// <summary>
        /// Construct instance with pending buffer
        /// </summary>
        /// <param name="pending">Pending buffer to use</param>
        public DeflaterHuffman(DeflaterPending pending)
        {
            Pending = pending;

            _literalTree = new Tree(this, LiteralNum, 257, 15);
            _distTree = new Tree(this, DistNum, 1, 15);
            _blTree = new Tree(this, BitlenNum, 4, 7);

            _dBuf = new short[Bufsize];
            _lBuf = new byte[Bufsize];
        }

        /// <summary>
        /// Reset internal state
        /// </summary>		
        public void Reset()
        {
            _lastLit = 0;
            _extraBits = 0;
            _literalTree.Reset();
            _distTree.Reset();
            _blTree.Reset();
        }

        /// <summary>
        /// Write all trees to pending buffer
        /// </summary>
        /// <param name="blTreeCodes">The number/rank of treecodes to send.</param>
        public void SendAllTrees(int blTreeCodes)
        {
            _blTree.BuildCodes();
            _literalTree.BuildCodes();
            _distTree.BuildCodes();
            Pending.WriteBits(_literalTree.NumCodes - 257, 5);
            Pending.WriteBits(_distTree.NumCodes - 1, 5);
            Pending.WriteBits(blTreeCodes - 4, 4);
            for (int rank = 0; rank < blTreeCodes; rank++)
            {
                Pending.WriteBits(_blTree.Length[BlOrder[rank]], 3);
            }
            _literalTree.WriteTree(_blTree);
            _distTree.WriteTree(_blTree);

#if DebugDeflation
			if (DeflaterConstants.DEBUGGING) {
				blTree.CheckEmpty();
			}
#endif
        }

        /// <summary>
        /// Compress current buffer writing data to pending buffer
        /// </summary>
        public void CompressBlock()
        {
            for (int i = 0; i < _lastLit; i++)
            {
                int litlen = _lBuf[i] & 0xff;
                int dist = _dBuf[i];
                if (dist-- != 0)
                {
                    //					if (DeflaterConstants.Debugging) {
                    //						Console.Write("["+(dist+1)+","+(litlen+3)+"]: ");
                    //					}

                    int lc = Lcode(litlen);
                    _literalTree.WriteSymbol(lc);

                    int bits = (lc - 261) / 4;
                    if (bits > 0 && bits <= 5)
                    {
                        Pending.WriteBits(litlen & ((1 << bits) - 1), bits);
                    }

                    int dc = Dcode(dist);
                    _distTree.WriteSymbol(dc);

                    bits = dc / 2 - 1;
                    if (bits > 0)
                    {
                        Pending.WriteBits(dist & ((1 << bits) - 1), bits);
                    }
                }
                else
                {
                    //					if (DeflaterConstants.Debugging) {
                    //						if (litlen > 32 && litlen < 127) {
                    //							Console.Write("("+(char)litlen+"): ");
                    //						} else {
                    //							Console.Write("{"+litlen+"}: ");
                    //						}
                    //					}
                    _literalTree.WriteSymbol(litlen);
                }
            }

#if DebugDeflation
			if (DeflaterConstants.DEBUGGING) {
				Console.Write("EOF: ");
			}
#endif
            _literalTree.WriteSymbol(EofSymbol);

#if DebugDeflation
			if (DeflaterConstants.DEBUGGING) {
				literalTree.CheckEmpty();
				distTree.CheckEmpty();
			}
#endif
        }

        /// <summary>
        /// Flush block to output with no compression
        /// </summary>
        /// <param name="stored">Data to write</param>
        /// <param name="storedOffset">Index of first byte to write</param>
        /// <param name="storedLength">Count of bytes to write</param>
        /// <param name="lastBlock">True if this is the last block</param>
        public void FlushStoredBlock(byte[] stored, int storedOffset, int storedLength, bool lastBlock)
        {
#if DebugDeflation
            //			if (DeflaterConstants.Debugging) {
            //				//Console.WriteLine("Flushing stored block "+ storedLength);
            //			}
#endif
            Pending.WriteBits((DeflaterConstants.StoredBlock << 1) + (lastBlock ? 1 : 0), 3);
            Pending.AlignToByte();
            Pending.WriteShort(storedLength);
            Pending.WriteShort(~storedLength);
            Pending.WriteBlock(stored, storedOffset, storedLength);
            Reset();
        }

        /// <summary>
        /// Flush block to output with compression
        /// </summary>		
        /// <param name="stored">Data to flush</param>
        /// <param name="storedOffset">Index of first byte to flush</param>
        /// <param name="storedLength">Count of bytes to flush</param>
        /// <param name="lastBlock">True if this is the last block</param>
        public void FlushBlock(byte[] stored, int storedOffset, int storedLength, bool lastBlock)
        {
            _literalTree.Freqs[EofSymbol]++;

            // Build trees
            _literalTree.BuildTree();
            _distTree.BuildTree();

            // Calculate bitlen frequency
            _literalTree.CalcBlFreq(_blTree);
            _distTree.CalcBlFreq(_blTree);

            // Build bitlen tree
            _blTree.BuildTree();

            int blTreeCodes = 4;
            for (int i = 18; i > blTreeCodes; i--)
            {
                if (_blTree.Length[BlOrder[i]] > 0)
                {
                    blTreeCodes = i + 1;
                }
            }
            int optLen = 14 + blTreeCodes * 3 + _blTree.GetEncodedLength() +
                         _literalTree.GetEncodedLength() + _distTree.GetEncodedLength() +
                         _extraBits;

            int staticLen = _extraBits;
            for (int i = 0; i < LiteralNum; i++)
            {
                staticLen += _literalTree.Freqs[i] * StaticLLength[i];
            }
            for (int i = 0; i < DistNum; i++)
            {
                staticLen += _distTree.Freqs[i] * StaticDLength[i];
            }
            if (optLen >= staticLen)
            {
                // Force static trees
                optLen = staticLen;
            }

            if (storedOffset >= 0 && storedLength + 4 < optLen >> 3)
            {
                // Store Block

                //				if (DeflaterConstants.Debugging) {
                //					//Console.WriteLine("Storing, since " + storedLength + " < " + opt_len
                //					                  + " <= " + static_len);
                //				}
                FlushStoredBlock(stored, storedOffset, storedLength, lastBlock);
            }
            else if (optLen == staticLen)
            {
                // Encode with static tree
                Pending.WriteBits((DeflaterConstants.StaticTrees << 1) + (lastBlock ? 1 : 0), 3);
                _literalTree.SetStaticCodes(StaticLCodes, StaticLLength);
                _distTree.SetStaticCodes(StaticDCodes, StaticDLength);
                CompressBlock();
                Reset();
            }
            else
            {
                // Encode with dynamic tree
                Pending.WriteBits((DeflaterConstants.DynTrees << 1) + (lastBlock ? 1 : 0), 3);
                SendAllTrees(blTreeCodes);
                CompressBlock();
                Reset();
            }
        }

        /// <summary>
        /// Get value indicating if internal buffer is full
        /// </summary>
        /// <returns>true if buffer is full</returns>
        public bool IsFull()
        {
            return _lastLit >= Bufsize;
        }

        /// <summary>
        /// Add literal to buffer
        /// </summary>
        /// <param name="literal">Literal value to add to buffer.</param>
        /// <returns>Value indicating internal buffer is full</returns>
        public bool TallyLit(int literal)
        {
            //			if (DeflaterConstants.Debugging) {
            //				if (lit > 32 && lit < 127) {
            //					//Console.WriteLine("("+(char)lit+")");
            //				} else {
            //					//Console.WriteLine("{"+lit+"}");
            //				}
            //			}
            _dBuf[_lastLit] = 0;
            _lBuf[_lastLit++] = (byte)literal;
            _literalTree.Freqs[literal]++;
            return IsFull();
        }

        /// <summary>
        /// Add distance code and length to literal and distance trees
        /// </summary>
        /// <param name="distance">Distance code</param>
        /// <param name="length">Length</param>
        /// <returns>Value indicating if internal buffer is full</returns>
        public bool TallyDist(int distance, int length)
        {
            //			if (DeflaterConstants.Debugging) {
            //				//Console.WriteLine("[" + distance + "," + length + "]");
            //			}

            _dBuf[_lastLit] = (short)distance;
            _lBuf[_lastLit++] = (byte)(length - 3);

            int lc = Lcode(length - 3);
            _literalTree.Freqs[lc]++;
            if (lc >= 265 && lc < 285)
            {
                _extraBits += (lc - 261) / 4;
            }

            int dc = Dcode(distance - 1);
            _distTree.Freqs[dc]++;
            if (dc >= 4)
            {
                _extraBits += dc / 2 - 1;
            }
            return IsFull();
        }


        /// <summary>
        /// Reverse the bits of a 16 bit value.
        /// </summary>
        /// <param name="toReverse">Value to reverse bits</param>
        /// <returns>Value with bits reversed</returns>
        public static short BitReverse(int toReverse)
        {
            return (short)(Bit4Reverse[toReverse & 0xF] << 12 |
                           Bit4Reverse[(toReverse >> 4) & 0xF] << 8 |
                           Bit4Reverse[(toReverse >> 8) & 0xF] << 4 |
                           Bit4Reverse[toReverse >> 12]);
        }

        static int Lcode(int length)
        {
            if (length == 255)
            {
                return 285;
            }

            int code = 257;
            while (length >= 8)
            {
                code += 4;
                length >>= 1;
            }
            return code + length;
        }

        static int Dcode(int distance)
        {
            int code = 0;
            while (distance >= 4)
            {
                code += 2;
                distance >>= 1;
            }
            return code + distance;
        }
    }

    /// <summary>
    /// A special stream deflating or compressing the bytes that are
    /// written to it.  It uses a Deflater to perform actual deflating.<br/>
    /// Authors of the original java version : Tom Tromey, Jochen Hoenicke 
    /// </summary>
    internal class DeflaterOutputStream : Stream
    {
        #region Constructors
        /// <summary>
        /// Creates a new DeflaterOutputStream with a default Deflater and default buffer size.
        /// </summary>
        /// <param name="baseOutputStream">
        /// the output stream where deflated output should be written.
        /// </param>
        public DeflaterOutputStream(Stream baseOutputStream)
            : this(baseOutputStream, new Deflater(), 512)
        {
        }

        /// <summary>
        /// Creates a new DeflaterOutputStream with the given Deflater and
        /// default buffer size.
        /// </summary>
        /// <param name="baseOutputStream">
        /// the output stream where deflated output should be written.
        /// </param>
        /// <param name="deflater">
        /// the underlying deflater.
        /// </param>
        public DeflaterOutputStream(Stream baseOutputStream, Deflater deflater)
            : this(baseOutputStream, deflater, 512)
        {
        }

        /// <summary>
        /// Creates a new DeflaterOutputStream with the given Deflater and
        /// buffer size.
        /// </summary>
        /// <param name="baseOutputStream">
        /// The output stream where deflated output is written.
        /// </param>
        /// <param name="deflater">
        /// The underlying deflater to use
        /// </param>
        /// <param name="bufferSize">
        /// The buffer size to use when deflating
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// bufsize is less than or equal to zero.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// baseOutputStream does not support writing
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// deflater instance is null
        /// </exception>
        public DeflaterOutputStream(Stream baseOutputStream, Deflater deflater, int bufferSize)
        {
            if (baseOutputStream == null)
            {
                throw new ArgumentNullException("baseOutputStream");
            }

            if (baseOutputStream.CanWrite == false)
            {
                throw new ArgumentException("Must support writing", "baseOutputStream");
            }

            if (deflater == null)
            {
                throw new ArgumentNullException("deflater");
            }

            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }

            _baseOutputStream = baseOutputStream;
            _buffer = new byte[bufferSize];
            _deflater = deflater;
        }
        #endregion

        #region Public API
        /// <summary>
        /// Finishes the stream by calling finish() on the deflater. 
        /// </summary>
        public virtual void Finish()
        {
            _deflater.Finish();
            while (!_deflater.IsFinished)
            {
                int len = _deflater.Deflate(_buffer, 0, _buffer.Length);
                if (len <= 0)
                {
                    break;
                }

                if (_keys != null)
                {
                    EncryptBlock(_buffer, 0, len);
                }

                _baseOutputStream.Write(_buffer, 0, len);
            }

            if (!_deflater.IsFinished)
            {
                throw new Exception("Can't deflate all input?");
            }

            _baseOutputStream.Flush();


            if (_keys != null)
            {
                _keys = null;
            }

        }

        /// <summary>
        /// Get/set flag indicating ownership of the underlying stream.
        /// When the flag is true <see cref="Close"></see> will close the underlying stream also.
        /// </summary>
        public bool IsStreamOwner
        {
            get { return _isStreamOwner; }
            set { _isStreamOwner = value; }
        }

        ///	<summary>
        /// Allows client to determine if an entry can be patched after its added
        /// </summary>
        public bool CanPatchEntries
        {
            get
            {
                return _baseOutputStream.CanSeek;
            }
        }

        #endregion

        #region Encryption

        string _password;

        uint[] _keys;

        /// <summary>
        /// Get/set the password used for encryption.
        /// </summary>
        /// <remarks>When set to null or if the password is empty no encryption is performed</remarks>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if ((value != null) && (value.Length == 0))
                {
                    _password = null;
                }
                else
                {
                    _password = value;
                }
            }
        }

        /// <summary>
        /// Encrypt a block of data
        /// </summary>
        /// <param name="buffer">
        /// Data to encrypt.  NOTE the original contents of the buffer are lost
        /// </param>
        /// <param name="offset">
        /// Offset of first byte in buffer to encrypt
        /// </param>
        /// <param name="length">
        /// Number of bytes in buffer to encrypt
        /// </param>
        protected void EncryptBlock(byte[] buffer, int offset, int length)
        {
            for (int i = offset; i < offset + length; ++i)
            {
                byte oldbyte = buffer[i];
                buffer[i] ^= EncryptByte();
                UpdateKeys(oldbyte);
            }
        }

        /// <summary>
        /// Encrypt a single byte 
        /// </summary>
        /// <returns>
        /// The encrypted value
        /// </returns>
        protected byte EncryptByte()
        {
            uint temp = ((_keys[2] & 0xFFFF) | 2);
            return (byte)((temp * (temp ^ 1)) >> 8);
        }

        /// <summary>
        /// Update encryption keys 
        /// </summary>		
        protected void UpdateKeys(byte ch)
        {
            _keys[0] = Crc32.ComputeCrc32(_keys[0], ch);
            _keys[1] = _keys[1] + (byte)_keys[0];
            _keys[1] = _keys[1] * 134775813 + 1;
            _keys[2] = Crc32.ComputeCrc32(_keys[2], (byte)(_keys[1] >> 24));
        }

        #endregion

        #region Deflation Support
        /// <summary>
        /// Deflates everything in the input buffers.  This will call
        /// <code>def.deflate()</code> until all bytes from the input buffers
        /// are processed.
        /// </summary>
        protected void Deflate()
        {
            while (!_deflater.IsNeedingInput)
            {
                int deflateCount = _deflater.Deflate(_buffer, 0, _buffer.Length);

                if (deflateCount <= 0)
                {
                    break;
                }

                if (_keys != null)
                {
                    EncryptBlock(_buffer, 0, deflateCount);
                }

                _baseOutputStream.Write(_buffer, 0, deflateCount);
            }

            if (!_deflater.IsNeedingInput)
            {
                throw new Exception("DeflaterOutputStream can't deflate all input?");
            }
        }
        #endregion

        #region Stream Overrides
        /// <summary>
        /// Gets value indicating stream can be read from
        /// </summary>
        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating if seeking is supported for this stream
        /// This property always returns false
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Get value indicating if this stream supports writing
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return _baseOutputStream.CanWrite;
            }
        }

        /// <summary>
        /// Get current length of stream
        /// </summary>
        public override long Length
        {
            get
            {
                return _baseOutputStream.Length;
            }
        }

        /// <summary>
        /// Gets the current position within the stream.
        /// </summary>
        /// <exception cref="NotSupportedException">Any attempt to set position</exception>
        public override long Position
        {
            get
            {
                return _baseOutputStream.Position;
            }
            set
            {
                throw new NotSupportedException("Position property not supported");
            }
        }

        /// <summary>
        /// Sets the current position of this stream to the given value. Not supported by this class!
        /// </summary>
        /// <param name="offset">The offset relative to the <paramref name="origin"/> to seek.</param>
        /// <param name="origin">The <see cref="SeekOrigin"/> to seek from.</param>
        /// <returns>The new position in the stream.</returns>
        /// <exception cref="NotSupportedException">Any access</exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("DeflaterOutputStream Seek not supported");
        }

        /// <summary>
        /// Sets the length of this stream to the given value. Not supported by this class!
        /// </summary>
        /// <param name="value">The new stream length.</param>
        /// <exception cref="NotSupportedException">Any access</exception>
        public override void SetLength(long value)
        {
            throw new NotSupportedException("DeflaterOutputStream SetLength not supported");
        }

        /// <summary>
        /// Read a byte from stream advancing position by one
        /// </summary>
        /// <returns>The byte read cast to an int.  THe value is -1 if at the end of the stream.</returns>
        /// <exception cref="NotSupportedException">Any access</exception>
        public override int ReadByte()
        {
            throw new NotSupportedException("DeflaterOutputStream ReadByte not supported");
        }

        /// <summary>
        /// Read a block of bytes from stream
        /// </summary>
        /// <param name="buffer">The buffer to store read data in.</param>
        /// <param name="offset">The offset to start storing at.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <returns>The actual number of bytes read.  Zero if end of stream is detected.</returns>
        /// <exception cref="NotSupportedException">Any access</exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("DeflaterOutputStream Read not supported");
        }

        /// <summary>
        /// Asynchronous reads are not supported a NotSupportedException is always thrown
        /// </summary>
        /// <param name="buffer">The buffer to read into.</param>
        /// <param name="offset">The offset to start storing data at.</param>
        /// <param name="count">The number of bytes to read</param>
        /// <param name="callback">The async callback to use.</param>
        /// <param name="state">The state to use.</param>
        /// <returns>Returns an <see cref="IAsyncResult"/></returns>
        /// <exception cref="NotSupportedException">Any access</exception>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException("DeflaterOutputStream BeginRead not currently supported");
        }

        /// <summary>
        /// Asynchronous writes arent supported, a NotSupportedException is always thrown
        /// </summary>
        /// <param name="buffer">The buffer to write.</param>
        /// <param name="offset">The offset to begin writing at.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <param name="callback">The <see cref="AsyncCallback"/> to use.</param>
        /// <param name="state">The state object.</param>
        /// <returns>Returns an IAsyncResult.</returns>
        /// <exception cref="NotSupportedException">Any access</exception>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException("BeginWrite is not supported");
        }

        /// <summary>
        /// Flushes the stream by calling <see cref="DeflaterOutputStream.Flush">Flush</see> on the deflater and then
        /// on the underlying stream.  This ensures that all bytes are flushed.
        /// </summary>
        public override void Flush()
        {
            _deflater.Flush();
            Deflate();
            _baseOutputStream.Flush();
        }

        /// <summary>
        /// Calls <see cref="Finish"/> and closes the underlying
        /// stream when <see cref="IsStreamOwner"></see> is true.
        /// </summary>
        public override void Close()
        {
            if (!_isClosed)
            {
                _isClosed = true;

                try
                {
                    Finish();

                    _keys = null;
                }
                finally
                {
                    if (_isStreamOwner)
                    {
                        _baseOutputStream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Writes a single byte to the compressed output stream.
        /// </summary>
        /// <param name="value">
        /// The byte value.
        /// </param>
        public override void WriteByte(byte value)
        {
            byte[] b = new byte[1];
            b[0] = value;
            Write(b, 0, 1);
        }

        /// <summary>
        /// Writes bytes from an array to the compressed stream.
        /// </summary>
        /// <param name="buffer">
        /// The byte array
        /// </param>
        /// <param name="offset">
        /// The offset into the byte array where to start.
        /// </param>
        /// <param name="count">
        /// The number of bytes to write.
        /// </param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            _deflater.SetInput(buffer, offset, count);
            Deflate();
        }
        #endregion

        #region Instance Fields
        /// <summary>
        /// This buffer is used temporarily to retrieve the bytes from the
        /// deflater and write them to the underlying output stream.
        /// </summary>
        readonly byte[] _buffer;

        /// <summary>
        /// The deflater which is used to deflate the stream.
        /// </summary>
        readonly Deflater _deflater;

        /// <summary>
        /// Base stream the deflater depends on.
        /// </summary>
        readonly Stream _baseOutputStream;

        bool _isClosed;

        bool _isStreamOwner = true;
        #endregion
    }

    /// <summary>
    /// This class stores the pending output of the Deflater.
    /// 
    /// author of the original java version : Jochen Hoenicke
    /// </summary>
    internal class DeflaterPending : PendingBuffer
    {
        /// <summary>
        /// Construct instance with default buffer size
        /// </summary>
        public DeflaterPending()
            : base(DeflaterConstants.PendingBufSize)
        {
        }
    }

    /// <summary>
    /// This class is general purpose class for writing data to a buffer.
    /// 
    /// It allows you to write bits as well as bytes
    /// Based on DeflaterPending.java
    /// 
    /// author of the original java version : Jochen Hoenicke
    /// </summary>
    internal class PendingBuffer
    {
        #region Instance Fields
        /// <summary>
        /// Internal work buffer
        /// </summary>
        readonly byte[] _buffer;

        int _start;
        int _end;

        uint _bits;
        int _bitCount;
        #endregion

        #region Constructors
        /// <summary>
        /// construct instance using default buffer size of 4096
        /// </summary>
        public PendingBuffer()
            : this(4096)
        {
        }

        /// <summary>
        /// construct instance using specified buffer size
        /// </summary>
        /// <param name="bufferSize">
        /// size to use for internal buffer
        /// </param>
        public PendingBuffer(int bufferSize)
        {
            _buffer = new byte[bufferSize];
        }

        #endregion

        /// <summary>
        /// Clear internal state/buffers
        /// </summary>
        public void Reset()
        {
            _start = _end = _bitCount = 0;
        }

        /// <summary>
        /// Write a byte to buffer
        /// </summary>
        /// <param name="value">
        /// The value to write
        /// </param>
        public void WriteByte(int value)
        {
#if DebugDeflation
			if (DeflaterConstants.DEBUGGING && (start != 0) )
			{
				throw new SharpZipBaseException("Debug check: start != 0");
			}
#endif
            _buffer[_end++] = unchecked((byte)value);
        }

        /// <summary>
        /// Write a short value to buffer LSB first
        /// </summary>
        /// <param name="value">
        /// The value to write.
        /// </param>
        public void WriteShort(int value)
        {
#if DebugDeflation
			if (DeflaterConstants.DEBUGGING && (start != 0) )
			{
				throw new SharpZipBaseException("Debug check: start != 0");
			}
#endif
            _buffer[_end++] = unchecked((byte)value);
            _buffer[_end++] = unchecked((byte)(value >> 8));
        }

        /// <summary>
        /// write an integer LSB first
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteInt(int value)
        {

            _buffer[_end++] = unchecked((byte)value);
            _buffer[_end++] = unchecked((byte)(value >> 8));
            _buffer[_end++] = unchecked((byte)(value >> 16));
            _buffer[_end++] = unchecked((byte)(value >> 24));
        }

        /// <summary>
        /// Write a block of data to buffer
        /// </summary>
        /// <param name="block">data to write</param>
        /// <param name="offset">offset of first byte to write</param>
        /// <param name="length">number of bytes to write</param>
        public void WriteBlock(byte[] block, int offset, int length)
        {

            Array.Copy(block, offset, _buffer, _end, length);
            _end += length;
        }

        /// <summary>
        /// The number of bits written to the buffer
        /// </summary>
        public int BitCount
        {
            get
            {
                return _bitCount;
            }
        }

        /// <summary>
        /// Align internal buffer on a byte boundary
        /// </summary>
        public void AlignToByte()
        {

            if (_bitCount > 0)
            {
                _buffer[_end++] = unchecked((byte)_bits);
                if (_bitCount > 8)
                {
                    _buffer[_end++] = unchecked((byte)(_bits >> 8));
                }
            }
            _bits = 0;
            _bitCount = 0;
        }

        /// <summary>
        /// Write bits to internal buffer
        /// </summary>
        /// <param name="b">source of bits</param>
        /// <param name="count">number of bits to write</param>
        public void WriteBits(int b, int count)
        {

            _bits |= (uint)(b << _bitCount);
            _bitCount += count;
            if (_bitCount >= 16)
            {
                _buffer[_end++] = unchecked((byte)_bits);
                _buffer[_end++] = unchecked((byte)(_bits >> 8));
                _bits >>= 16;
                _bitCount -= 16;
            }
        }

        /// <summary>
        /// Write a short value to internal buffer most significant byte first
        /// </summary>
        /// <param name="s">value to write</param>
        public void WriteShortMsb(int s)
        {

            _buffer[_end++] = unchecked((byte)(s >> 8));
            _buffer[_end++] = unchecked((byte)s);
        }

        /// <summary>
        /// Indicates if buffer has been flushed
        /// </summary>
        public bool IsFlushed
        {
            get
            {
                return _end == 0;
            }
        }

        /// <summary>
        /// Flushes the pending buffer into the given output array.  If the
        /// output array is to small, only a partial flush is done.
        /// </summary>
        /// <param name="output">The output array.</param>
        /// <param name="offset">The offset into output array.</param>
        /// <param name="length">The maximum number of bytes to store.</param>
        /// <returns>The number of bytes flushed.</returns>
        public int Flush(byte[] output, int offset, int length)
        {
            if (_bitCount >= 8)
            {
                _buffer[_end++] = unchecked((byte)_bits);
                _bits >>= 8;
                _bitCount -= 8;
            }

            if (length > _end - _start)
            {
                length = _end - _start;
                Array.Copy(_buffer, _start, output, offset, length);
                _start = 0;
                _end = 0;
            }
            else
            {
                Array.Copy(_buffer, _start, output, offset, length);
                _start += length;
            }
            return length;
        }

        /// <summary>
        /// Convert internal buffer to byte array.
        /// Buffer is empty on completion
        /// </summary>
        /// <returns>
        /// The internal buffer contents converted to a byte array.
        /// </returns>
        public byte[] ToByteArray()
        {
            byte[] result = new byte[_end - _start];
            Array.Copy(_buffer, _start, result, 0, result.Length);
            _start = 0;
            _end = 0;
            return result;
        }
    }

    class PngEncoderInternal
    {
        /** Constant specifying that alpha channel should be encoded. */
        public const bool EncodeAlpha = true;

        /** Constant specifying that alpha channel should not be encoded. */
        public const bool NoAlpha = false;

        /** Constants for filter (NONE) */
        public const int FilterNone = 0;

        /** Constants for filter (SUB) */
        public const int FilterSub = 1;

        /** Constants for filter (UP) */
        public const int FilterUp = 2;

        /** Constants for filter (LAST) */
        public const int FilterLast = 2;

        /** IHDR tag. */
        static readonly byte[] IHDR = new byte[] { 73, 72, 68, 82 };

        /** IDAT tag. */
        static readonly byte[] IDAT = new byte[] { 73, 68, 65, 84 };

        /** IEND tag. */
        static readonly byte[] IEND = new byte[] { 73, 69, 78, 68 };

        /** The png bytes. */
        byte[] _pngBytes;

        /** The prior row. */
        byte[] _priorRow;

        /** The left bytes. */
        byte[] _leftBytes;

        /** The width. */
        readonly int _width;
        readonly int _height;

        /** The byte position. */
        int _bytePos, _maxPos;

        /** CRC. */
        readonly Crc32 _crc = new Crc32();

        /** The CRC value. */
        long _crcValue;

        /** Encode alpha? */
        readonly bool _encodeAlpha;

        /** The filter type. */
        readonly int _filter;

        /** The bytes-per-pixel. */
        int _bytesPerPixel;

        /** The compression level. */
        readonly int _compressionLevel;

        /** PixelData array to encode */
        readonly int[] _pixelData;

        /**
         * Class constructor specifying Image source to encode, whether to encode alpha, filter to use,
         * and compression level.
         *
         * @param pixel_data A Java Image object
         * @param encodeAlpha Encode the alpha channel? false=no; true=yes
         * @param whichFilter 0=none, 1=sub, 2=up
         * @param compLevel 0..9
         * @see java.awt.Image
         */
        public PngEncoderInternal(int[] pixelData, int width, int height, bool encodeAlpha, int whichFilter, int compLevel)
        {
            _pixelData = pixelData;
            _width = width;
            _height = height;
            _encodeAlpha = encodeAlpha;

            _filter = FilterNone;
            if (whichFilter <= FilterLast)
            {
                _filter = whichFilter;
            }

            if (compLevel >= 0 && compLevel <= 9)
            {
                _compressionLevel = compLevel;
            }
        }

        /**
         * Creates an array of bytes that is the PNG equivalent of the current image, specifying
         * whether to encode alpha or not.
         *
         * @param encodeAlpha boolean false=no alpha, true=encode alpha
         * @return an array of bytes, or null if there was a problem
         */
        public byte[] Encode(bool encodeAlpha)
        {
            byte[] pngIdBytes = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

            /*
             * start with an array that is big enough to hold all the pixels
             * (plus filter bytes), and an extra 200 bytes for header info
             */
            _pngBytes = new byte[((_width + 1) * _height * 3) + 200];

            /*
             * keep track of largest byte written to the array
             */
            _maxPos = 0;

            _bytePos = WriteBytes(pngIdBytes, 0);
            //hdrPos = bytePos;
            WriteHeader();
            //dataPos = bytePos;
            if (WriteImageData())
            {
                WriteEnd();
                _pngBytes = ResizeByteArray(_pngBytes, _maxPos);
            }
            else
            {
                _pngBytes = null;
            }
            return _pngBytes;
        }

        /**
         * Creates an array of bytes that is the PNG equivalent of the current image.
         * Alpha encoding is determined by its setting in the constructor.
         *
         * @return an array of bytes, or null if there was a problem
         */
        public byte[] PngEncode()
        {
            return Encode(_encodeAlpha);
        }

        /**
         * Increase or decrease the length of a byte array.
         *
         * @param array The original array.
         * @param newLength The length you wish the new array to have.
         * @return Array of newly desired length. If shorter than the
         *         original, the trailing elements are truncated.
         */
        protected byte[] ResizeByteArray(byte[] array, int newLength)
        {
            byte[] newArray = new byte[newLength];
            int oldLength = array.Length;

            Array.Copy(array, 0, newArray, 0, Math.Min(oldLength, newLength));
            return newArray;
        }

        /**
         * Write an array of bytes into the pngBytes array.
         * Note: This routine has the side effect of updating
         * maxPos, the largest element written in the array.
         * The array is resized by 1000 bytes or the length
         * of the data to be written, whichever is larger.
         *
         * @param data The data to be written into pngBytes.
         * @param offset The starting point to write to.
         * @return The next place to be written to in the pngBytes array.
         */
        protected int WriteBytes(byte[] data, int offset)
        {
            _maxPos = Math.Max(_maxPos, offset + data.Length);
            if (data.Length + offset > _pngBytes.Length)
                _pngBytes = ResizeByteArray(_pngBytes, _pngBytes.Length + Math.Max(1000, data.Length));

            Array.Copy(data, 0, _pngBytes, offset, data.Length);
            return offset + data.Length;
        }

        /**
         * Write an array of bytes into the pngBytes array, specifying number of bytes to write.
         * Note: This routine has the side effect of updating
         * maxPos, the largest element written in the array.
         * The array is resized by 1000 bytes or the length
         * of the data to be written, whichever is larger.
         *
         * @param data The data to be written into pngBytes.
         * @param nBytes The number of bytes to be written.
         * @param offset The starting point to write to.
         * @return The next place to be written to in the pngBytes array.
         */
        protected int WriteBytes(byte[] data, int nBytes, int offset)
        {
            _maxPos = Math.Max(_maxPos, offset + nBytes);
            if (nBytes + offset > _pngBytes.Length)
                _pngBytes = ResizeByteArray(_pngBytes, _pngBytes.Length + Math.Max(1000, nBytes));

            Array.Copy(data, 0, _pngBytes, offset, nBytes);
            return offset + nBytes;
        }

        /**
         * Write a two-byte integer into the pngBytes array at a given position.
         *
         * @param n The integer to be written into pngBytes.
         * @param offset The starting point to write to.
         * @return The next place to be written to in the pngBytes array.
         */
        protected int WriteInt2(int n, int offset)
        {
            byte[] temp = { (byte)((n >> 8) & 0xff), (byte)(n & 0xff) };

            return WriteBytes(temp, offset);
        }

        /**
         * Write a four-byte integer into the pngBytes array at a given position.
         *
         * @param n The integer to be written into pngBytes.
         * @param offset The starting point to write to.
         * @return The next place to be written to in the pngBytes array.
         */
        protected int WriteInt4(int n, int offset)
        {
            byte[] temp = {(byte) ((n >> 24) & 0xff),
                           (byte) ((n >> 16) & 0xff),
                           (byte) ((n >> 8) & 0xff),
                           (byte) (n & 0xff)};

            return WriteBytes(temp, offset);
        }

        /**
         * Write a single byte into the pngBytes array at a given position.
         *
         * @param b The integer to be written into pngBytes.
         * @param offset The starting point to write to.
         * @return The next place to be written to in the pngBytes array.
         */
        protected int WriteByte(int b, int offset)
        {
            byte[] temp = { (byte)b };

            return WriteBytes(temp, offset);
        }

        /**
         * Write a PNG "IHDR" chunk into the pngBytes array.
         */
        protected void WriteHeader()
        {
            int startPos = _bytePos = WriteInt4(13, _bytePos);

            _bytePos = WriteBytes(IHDR, _bytePos);
            _bytePos = WriteInt4(_width, _bytePos);
            _bytePos = WriteInt4(_height, _bytePos);
            _bytePos = WriteByte(8, _bytePos); // bit depth
            _bytePos = WriteByte((_encodeAlpha) ? 6 : 2, _bytePos); // direct model
            _bytePos = WriteByte(0, _bytePos); // compression method
            _bytePos = WriteByte(0, _bytePos); // filter method
            _bytePos = WriteByte(0, _bytePos); // no interlace

            _crc.Reset();
            _crc.Update(_pngBytes, startPos, _bytePos - startPos);
            _crcValue = _crc.Value;

            _bytePos = WriteInt4((int)_crcValue, _bytePos);
        }

        /**
         * Perform "sub" filtering on the given row.
         * Uses temporary array leftBytes to store the original values
         * of the previous pixels.  The array is 16 bytes long, which
         * will easily hold two-byte samples plus two-byte alpha.
         *
         * @param pixels The array holding the scan lines being built
         * @param startPos Starting position within pixels of bytes to be filtered.
         * @param width Width of a scanline in pixels.
         */
        protected void DoFilterSub(byte[] pixels, int startPos, int newWidth)
        {
            int i;
            int offset = _bytesPerPixel;
            int actualStart = startPos + offset;
            int nBytes = newWidth * _bytesPerPixel;
            int leftInsert = offset;
            int leftExtract = 0;

            for (i = actualStart; i < startPos + nBytes; i++)
            {
                _leftBytes[leftInsert] = pixels[i];
                pixels[i] = (byte)((pixels[i] - _leftBytes[leftExtract]) % 256);
                leftInsert = (leftInsert + 1) % 0x0f;
                leftExtract = (leftExtract + 1) % 0x0f;
            }
        }

        /**
         * Perform "up" filtering on the given row.
         * Side effect: refills the prior row with current row
         *
         * @param pixels The array holding the scan lines being built
         * @param startPos Starting position within pixels of bytes to be filtered.
         * @param width Width of a scanline in pixels.
         */
        protected void DoFilterUp(byte[] pixels, int startPos, int width)
        {
            int i, nBytes;
            byte currentByte;

            nBytes = width * _bytesPerPixel;

            for (i = 0; i < nBytes; i++)
            {
                currentByte = pixels[startPos + i];
                pixels[startPos + i] = (byte)((pixels[startPos + i] - _priorRow[i]) % 256);
                _priorRow[i] = currentByte;
            }
        }

        /**
         * Write the image data into the pngBytes array.
         * This will write one or more PNG "IDAT" chunks. In order
         * to conserve memory, this method grabs as many rows as will
         * fit into 32K bytes, or the whole image; whichever is less.
         *
         *
         * @return true if no errors; false if error grabbing pixels
         */
        protected bool WriteImageData()
        {
            int rowsLeft = _height;  // number of rows remaining to write
            int startRow = 0;       // starting row to process this time through
            int nRows;              // how many rows to grab at a time

            byte[] scanLines;       // the scan lines to be compressed
            int scanPos;            // where we are in the scan lines
            int startPos;           // where this line's actual pixels start (used for filtering)

            byte[] compressedLines; // the resultant compressed lines
            int nCompressed;        // how big is the compressed area?

            //int depth;              // color depth ( handle only 8 or 32 )

            _bytesPerPixel = (_encodeAlpha) ? 4 : 3;

            Deflater scrunch = new Deflater(_compressionLevel);
            MemoryStream outBytes = new MemoryStream(1024);

            DeflaterOutputStream compBytes = new DeflaterOutputStream(outBytes, scrunch);
            try
            {
                while (rowsLeft > 0)
                {
                    nRows = Math.Min(32767 / (_width * (_bytesPerPixel + 1)), rowsLeft);
                    nRows = Math.Max(nRows, 1);

                    int[] pixels = new int[_width * nRows];
                    Array.Copy(_pixelData, _width * startRow, pixels, 0, _width * nRows);

                    /*
                     * Create a data chunk. scanLines adds "nRows" for
                     * the filter bytes.
                     */
                    scanLines = new byte[_width * nRows * _bytesPerPixel + nRows];

                    if (_filter == FilterSub)
                    {
                        _leftBytes = new byte[16];
                    }
                    if (_filter == FilterUp)
                    {
                        _priorRow = new byte[_width * _bytesPerPixel];
                    }

                    scanPos = 0;
                    startPos = 1;
                    for (int i = 0; i < _width * nRows; i++)
                    {
                        if (i % _width == 0)
                        {
                            scanLines[scanPos++] = (byte)_filter;
                            startPos = scanPos;
                        }
                        scanLines[scanPos++] = (byte)((pixels[i] >> 16) & 0xff);
                        scanLines[scanPos++] = (byte)((pixels[i] >> 8) & 0xff);
                        scanLines[scanPos++] = (byte)((pixels[i]) & 0xff);
                        if (_encodeAlpha)
                        {
                            scanLines[scanPos++] = (byte)((pixels[i] >> 24) & 0xff);
                        }
                        if ((i % _width == _width - 1) && (_filter != FilterNone))
                        {
                            if (_filter == FilterSub)
                            {
                                DoFilterSub(scanLines, startPos, _width);
                            }
                            if (_filter == FilterUp)
                            {
                                DoFilterUp(scanLines, startPos, _width);
                            }
                        }
                    }

                    /*
                     * Write these lines to the output area
                     */
                    compBytes.Write(scanLines, 0, scanPos);

                    startRow += nRows;
                    rowsLeft -= nRows;
                }
                compBytes.Close();

                /*
                 * Write the compressed bytes
                 */
                compressedLines = outBytes.ToArray();
                nCompressed = compressedLines.Length;

                _crc.Reset();
                _bytePos = WriteInt4(nCompressed, _bytePos);
                _bytePos = WriteBytes(IDAT, _bytePos);
                _crc.Update(IDAT);
                _bytePos = WriteBytes(compressedLines, nCompressed, _bytePos);
                _crc.Update(compressedLines, 0, nCompressed);

                _crcValue = _crc.Value;
                _bytePos = WriteInt4((int)_crcValue, _bytePos);
                scrunch.Finish();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /**
         * Write a PNG "IEND" chunk into the pngBytes array.
         */
        protected void WriteEnd()
        {
            _bytePos = WriteInt4(0, _bytePos);
            _bytePos = WriteBytes(IEND, _bytePos);
            _crc.Reset();
            _crc.Update(IEND);
            _crcValue = _crc.Value;
            _bytePos = WriteInt4((int)_crcValue, _bytePos);
        }
    }
}