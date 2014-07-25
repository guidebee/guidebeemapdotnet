//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 24SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using System.Collections.Generic;
using System.IO;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Rms
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 24SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A class representing a record store. 
    /// </summary>
    public class RecordStore
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Deletes the record store.
        /// </summary>
        /// <param name="recordStoreName">Name of the record store.</param>
        public static void DeleteRecordStore(string recordStoreName)
        {
            RecordStore recordStore = new RecordStore();
            recordStore._indexFileName = recordStore._internalPathPrefix + recordStoreName + ".idx";
            recordStore._dataFileName = recordStore._internalPathPrefix + recordStoreName + ".dat";
            if (!File.Exists(recordStore._indexFileName) || !File.Exists(recordStore._dataFileName))
            {
                throw new RecordStoreNotFoundException("Record store not found");
            }
            try
            {
                File.Delete(recordStore._indexFileName);
                File.Delete(recordStore._dataFileName);

            }
            catch (Exception e)
            {
                throw new RecordStoreException(e.Message);
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Opens the record store.
        /// </summary>
        /// <param name="recordStoreName">Name of the record store.</param>
        /// <param name="createIfNecessary">if set to <c>true</c> [create if necessary].</param>
        /// <returns></returns>
        public static RecordStore OpenRecordStore(string recordStoreName, bool createIfNecessary)
        {
            RecordStore recordStore = new RecordStore();
            recordStore._indexFileName = recordStore._internalPathPrefix + recordStoreName + ".idx";
            recordStore._dataFileName = recordStore._internalPathPrefix + recordStoreName + ".dat";
            if ((!File.Exists(recordStore._indexFileName) || !File.Exists(recordStore._dataFileName)) && !createIfNecessary)
            {
                throw new RecordStoreNotFoundException("Record store not found");
            }
            try
            {
                recordStore._indexFileStream = new FileStream(recordStore._indexFileName, FileMode.OpenOrCreate);
                recordStore._dataFileStream = new FileStream(recordStore._dataFileName, FileMode.OpenOrCreate);
                recordStore._indexFileReader = new BinaryReader(recordStore._indexFileStream);
                recordStore._indexFileWriter = new BinaryWriter(recordStore._indexFileStream);
                recordStore._dataFileWriter = new BinaryWriter(recordStore._dataFileStream);
                return recordStore;
            }
            catch (IOException e)
            {
                throw new RecordStoreException(e.Message);
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Closes the record store.
        /// </summary>
        public void CloseRecordStore()
        {
            try
            {
                if (_indexFileStream != null)
                {
                    _indexFileStream.Close();
                    _indexFileStream = null;
                }
                if (_dataFileStream != null)
                {
                    _dataFileStream.Close();
                    _dataFileStream = null;
                }

            }
            catch (Exception e)
            {
                throw new RecordStoreException(e.Message);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the num records.
        /// </summary>
        /// <returns></returns>
        public int GetNumRecords()
        {
            if (_indexFileStream == null)
            {
                throw new RecordStoreNotOpenException("not open");
            }
            try
            {
                long length = _indexFileStream.Length;
                return (int)length / RecordLength;
            }
            catch (Exception)
            {
            }
            return -1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            if (_indexFileStream == null || _dataFileStream == null)
            {
                throw new RecordStoreNotOpenException("not open");
            }
            try
            {
                return (int)(_indexFileStream.Length + _dataFileStream.Length);
            }
            catch (IOException)
            {
            }
            return -1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the next record ID.
        /// </summary>
        /// <returns></returns>
        public int GetNextRecordID()
        {
            return GetNumRecords() + 1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reads all records.
        /// </summary>
        private void ReadAllRecords()
        {
            try
            {
                if (_indexFileStream == null || _dataFileStream == null)
                {
                    throw new RecordStoreNotOpenException("not open");
                }
                _indexFileStream.Seek(0, SeekOrigin.Begin);
                _recordDataIndex.Clear();
                {
                    RecordData recordData = new RecordData
                                                {
                                                    RecordID = _indexFileReader.ReadInt32(),
                                                    BlockSize = _indexFileReader.ReadInt32(),
                                                    DataOffset = _indexFileReader.ReadInt32(),
                                                    DataLength = _indexFileReader.ReadInt32()
                                                };
                    _recordDataIndex.Add(recordData);
                }
                for (int i = 0; i < _recordDataIndex.Count; i++)
                {
                    RecordData list = _recordDataIndex[i];
                    _dataFileStream.Seek(list.DataOffset, SeekOrigin.Begin);
                    list.Data = new byte[list.BlockSize];
                    _dataFileStream.Read(list.Data, 0, list.Data.Length);
                }
            }
            catch (IOException ex)
            {
                throw new RecordStoreException(ex.Message);
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the record.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="numBytes">The num bytes.</param>
        /// <returns></returns>
        public int AddRecord(byte[] data, int offset, int numBytes)
        {
            try
            {
                int blockSize = numBytes / Blocksize;
                if (blockSize * Blocksize < numBytes)
                {
                    blockSize += 1;
                }
                RecordData recordData = new RecordData
                                            {
                                                RecordID = GetNumRecords() + 1,
                                                Data = new byte[blockSize*Blocksize]
                                            };
                recordData.BlockSize = recordData.Data.Length;
                recordData.DataOffset = (int)_dataFileStream.Length;
                recordData.DataLength = numBytes;
                Array.Copy(data, offset, recordData.Data, 0, numBytes);
                if (_indexFileStream == null || _dataFileStream == null)
                {
                    throw new RecordStoreNotOpenException("not open");
                }
                _indexFileStream.Seek((recordData.RecordID - 1) * RecordLength, SeekOrigin.Begin);
                {
                    _indexFileWriter.Write(recordData.RecordID);
                    _indexFileWriter.Write(recordData.BlockSize);
                    _indexFileWriter.Write(recordData.DataOffset);
                    _indexFileWriter.Write(recordData.DataLength);
                }
                _dataFileStream.Seek(_dataFileStream.Length, SeekOrigin.Begin);
                {
                    _dataFileWriter.Write(recordData.Data);
                }
                return recordData.RecordID;
            }
            catch (IOException ex)
            {
                throw new RecordStoreException(ex.Message);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Deletes the record.
        /// </summary>
        /// <param name="recordId">The record id.</param>
        public void DeleteRecord(int recordId)
        {
            if (recordId > GetNumRecords() || recordId < 1)
            {
                throw new InvalidRecordIDException("Record ID is out of range");
            }
            ReadAllRecords();
            try
            {
                _recordDataIndex.RemoveAt(recordId - 1);
                _indexFileStream.SetLength(0);
                for (int i = 0; i < _recordDataIndex.Count; i++)
                {
                    RecordData recordData = _recordDataIndex[i];
                    _indexFileWriter.Write(recordData.RecordID);
                    _indexFileWriter.Write(recordData.BlockSize);
                    _indexFileWriter.Write(recordData.DataOffset);
                    _indexFileWriter.Write(recordData.DataLength);
                }
                _dataFileStream.SetLength(0);
                _dataFileStream.Seek(0, SeekOrigin.Begin);
                for (int i = 0; i < _recordDataIndex.Count; i++)
                {
                    RecordData recordData = _recordDataIndex[i];
                    _dataFileWriter.Write(recordData.Data);
                }


            }
            catch (Exception e)
            {
                throw new RecordStoreException(e.Message);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the size of the record.
        /// </summary>
        /// <param name="recordId">The record id.</param>
        /// <returns></returns>
        public int GetRecordSize(int recordId)
        {
            if (recordId > GetNumRecords() || recordId < 1)
            {
                throw new InvalidRecordIDException("Record ID is out of range");
            }
            if (_indexFileStream == null || _dataFileStream == null)
            {
                throw new RecordStoreNotOpenException("not open");
            }
            try
            {
                _indexFileStream.Seek((recordId - 1) * RecordLength, SeekOrigin.Begin);

                {
                    RecordData recordData = new RecordData
                                                {
                                                    RecordID = _indexFileReader.ReadInt32(),
                                                    BlockSize = _indexFileReader.ReadInt32(),
                                                    DataOffset = _indexFileReader.ReadInt32(),
                                                    DataLength = _indexFileReader.ReadInt32()
                                                };
                    return recordData.DataLength;
                }
            }
            catch (Exception e)
            {
                throw new RecordStoreException(e.Message);
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the record.
        /// </summary>
        /// <param name="recordId">The record id.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public int GetRecord(int recordId, byte[] buffer, int offset)
        {

            if (recordId > GetNumRecords() || recordId < 1)
            {
                throw new InvalidRecordIDException("Record ID is out of range");
            }
            if (_indexFileStream == null || _dataFileStream == null)
            {
                throw new RecordStoreNotOpenException("not open");
            }
            try
            {
                _indexFileStream.Seek((recordId - 1) * RecordLength, SeekOrigin.Begin);
                RecordData recordData = new RecordData();

                {

                    recordData.RecordID = _indexFileReader.ReadInt32();
                    recordData.BlockSize = _indexFileReader.ReadInt32();
                    recordData.DataOffset = _indexFileReader.ReadInt32();
                    recordData.DataLength = _indexFileReader.ReadInt32();
                }
                _dataFileStream.Seek(recordData.DataOffset, SeekOrigin.Begin);

                {
                    byte[] tempBuffer = new byte[recordData.BlockSize];
                    _dataFileStream.Read(tempBuffer, 0, tempBuffer.Length);
                    Array.Copy(tempBuffer, 0, buffer, 0, recordData.DataLength);

                }
                return recordData.DataLength;
            }
            catch (Exception e)
            {
                throw new RecordStoreException(e.Message);
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the record.
        /// </summary>
        /// <param name="recordId">The record id.</param>
        /// <returns></returns>
        public byte[] GetRecord(int recordId)
        {
            if (recordId > GetNumRecords() || recordId < 1)
            {
                throw new InvalidRecordIDException("Record ID is out of range");
            }
            if (_indexFileStream == null || _dataFileStream == null)
            {
                throw new RecordStoreNotOpenException("not open");
            }
            try
            {
                _indexFileStream.Seek((recordId - 1) * RecordLength, SeekOrigin.Begin);
                RecordData recordData = new RecordData();

                {

                    recordData.RecordID = _indexFileReader.ReadInt32();
                    recordData.BlockSize = _indexFileReader.ReadInt32();
                    recordData.DataOffset = _indexFileReader.ReadInt32();
                    recordData.DataLength = _indexFileReader.ReadInt32();
                }
                _dataFileStream.Seek(recordData.DataOffset, SeekOrigin.Begin);

                {

                    byte[] tempBuffer = new byte[recordData.BlockSize];
                    _dataFileStream.Read(tempBuffer, 0, tempBuffer.Length);

                    return tempBuffer;

                }

            }
            catch (Exception e)
            {
                throw new RecordStoreException(e.Message);
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the record.
        /// </summary>
        /// <param name="recordId">The record id.</param>
        /// <param name="newData">The new data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="numBytes">The num bytes.</param>
        public void SetRecord(int recordId, byte[] newData, int offset, int numBytes)
        {
            if (recordId > GetNumRecords() || recordId < 1)
            {
                throw new InvalidRecordIDException("Record ID is out of range");
            }
            if (_indexFileStream == null || _dataFileStream == null)
            {
                throw new RecordStoreNotOpenException("not open");
            }
            try
            {
                _indexFileStream.Seek((recordId - 1) * RecordLength, SeekOrigin.Begin);
                RecordData recordData = new RecordData();

                {

                    recordData.RecordID = _indexFileReader.ReadInt32();
                    recordData.BlockSize = _indexFileReader.ReadInt32();
                    recordData.DataOffset = _indexFileReader.ReadInt32();
                    recordData.DataLength = _indexFileReader.ReadInt32();
                }
                if (recordData.BlockSize >= numBytes)
                {
                    _indexFileStream.Seek((recordId - 1) * RecordLength, SeekOrigin.Begin);
                    recordData.DataLength = numBytes;
                    _indexFileWriter.Write(recordData.RecordID);
                    _indexFileWriter.Write(recordData.BlockSize);
                    _indexFileWriter.Write(recordData.DataOffset);
                    _indexFileWriter.Write(recordData.DataLength);

                    _dataFileStream.Seek(recordData.DataOffset, SeekOrigin.Begin);

                    {

                        _dataFileStream.Write(newData, offset, numBytes);
                    }
                }
                else
                {
                    ReadAllRecords();
                    try
                    {

                        int blockSize = numBytes / Blocksize;
                        if (blockSize * Blocksize < numBytes)
                        {
                            blockSize += 1;
                        }
                        _recordDataIndex[recordId - 1].Data = new byte[blockSize * Blocksize];
                        Array.Copy(newData, offset, _recordDataIndex[recordId - 1].Data, 0, numBytes);
                        _recordDataIndex[recordId - 1].BlockSize = blockSize * Blocksize;
                        _recordDataIndex[recordId - 1].DataLength = numBytes;
                        for (int i = recordId; i < GetNumRecords(); i++)
                        {
                            _recordDataIndex[i].DataOffset = _recordDataIndex[i - 1].DataOffset + _recordDataIndex[i - 1].BlockSize;
                        }


                        _indexFileStream.SetLength(0);
                        _indexFileStream.Seek(0, SeekOrigin.Begin);

                        for (int i = 0; i < _recordDataIndex.Count; i++)
                        {
                            RecordData list = _recordDataIndex[i];
                            _indexFileWriter.Write(list.RecordID);
                            _indexFileWriter.Write(list.BlockSize);
                            _indexFileWriter.Write(list.DataOffset);
                            _indexFileWriter.Write(list.DataLength);
                        }



                        _dataFileStream.SetLength(0);
                        _dataFileStream.Seek(0, SeekOrigin.Begin);

                        for (int i = 0; i < _recordDataIndex.Count; i++)
                        {
                            RecordData list = _recordDataIndex[i];
                            _dataFileStream.Write(list.Data, 0, list.Data.Length);
                        }


                    }
                    catch (Exception e)
                    {
                        throw new RecordStoreException(e.Message);
                    }

                }

            }
            catch (Exception e)
            {
                throw new RecordStoreException(e.Message);
            }
        }

        private const int RecordLength = 16;
        private readonly string _internalPathPrefix = "./";
        private const int Blocksize = 256;
        private string _indexFileName;
        private string _dataFileName;
        private FileStream _indexFileStream;
        private FileStream _dataFileStream;
        private BinaryReader _indexFileReader;
        private BinaryWriter _indexFileWriter;
        private BinaryWriter _dataFileWriter;
        private readonly List<RecordData> _recordDataIndex = new List<RecordData>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordStore"/> class.
        /// </summary>
        private RecordStore()
        {

            _internalPathPrefix = Path.GetTempPath();
            if (_internalPathPrefix.Length > 0)
            {
                if (_internalPathPrefix[_internalPathPrefix.Length - 1] != '\\')
                {
                    _internalPathPrefix += '\\';
                }

            }
            else
            {
                _internalPathPrefix = "." + '\\';
            }
            _internalPathPrefix += "guidebee" + '\\';

            if (!Directory.Exists(_internalPathPrefix))
            {
                Directory.CreateDirectory(_internalPathPrefix);
            }

        }
    }

    class RecordData
    {

        public int RecordID;
        public int BlockSize;
        public int DataLength;
        public int DataOffset;
        public byte[] Data;
    }
}
