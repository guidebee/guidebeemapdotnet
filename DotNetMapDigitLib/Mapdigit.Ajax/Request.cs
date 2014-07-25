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
using System.IO;
using System.Text;
using System.Threading;
using Mapdigit.Ajax.Json;
using Mapdigit.Network;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Ajax
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 24SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// With Requst object, application can issue a asynchronous http requst to a 
    /// server and Requst handles the message in a seperate thread.
    /// </summary>
    public sealed class Request
    {

        /**
         * UTF-8 encoding. (Charset value)
         */
        public const string Utf8Charset = "utf-8";

        /**
         * ISO-8859-1 encoding.(Charset value)
         */
        public const string Iso8859Charset = "iso-8859-1";

        /**
         * GB2312 encoding.(Charset value)
         */
        public const string Gb2312Charset = "gb2312";

        /**
         * total bytes downloaded
         */
        public static long TotaldownloadedBytes;

        
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Issue a synchronous GET requst.
        /// </summary>
        /// <param name="url">Http request url</param>
        /// <param name="inputArgs">argument of for the url.</param>
        /// <param name="httpArgs">extra http header.</param>
        /// <param name="listener">RequestLister used to handle the sync http response</param>
        /// <returns>http response object</returns>
        public static Response Get(string url,
                 Arg[] inputArgs,
                 Arg[] httpArgs,
                 IRequestListener listener)
        {
            return Sync(HttpConnection.Get, url, inputArgs, httpArgs,
                    listener, null);
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Issue a asynchronous GET requst.
        /// </summary>
        /// <param name="url">Http request url.</param>
        /// <param name="inputArgs">argument of for the url.</param>
        /// <param name="httpArgs">extra http header.</param>
        /// <param name="listener">RequestLister used to handle the async http response.</param>
        /// <param name="context">message context ,wiil pass as the same in done().</param>
        public static void Get(
                 string url,
                 Arg[] inputArgs,
                 Arg[] httpArgs,
                 IRequestListener listener,
                 object context)
        {

            Async(HttpConnection.Get, url, inputArgs, httpArgs, listener, null,
                    context);
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Issue a synchronous POST requst.
        /// </summary>
        /// <param name="url">Http request url</param>
        /// <param name="inputArgs">argument of for the url</param>
        /// <param name="httpArgs">extra http header</param>
        /// <param name="listener">RequestLister used to handle the sync http response.</param>
        /// <param name="multiPart">message body.</param>
        /// <returns>http response object</returns>
        public static Response Post(string url,
                 Arg[] inputArgs,
                 Arg[] httpArgs,
                 IRequestListener listener,
                 PostData multiPart)
        {

            return Sync(HttpConnection.Post, url, inputArgs, httpArgs, listener,
                    multiPart);
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Issue an asynchronous POST requst.
        /// </summary>
        /// <param name="url">Http request url.</param>
        /// <param name="inputArgs">argument of for the url</param>
        /// <param name="httpArgs">extra http header.</param>
        /// <param name="listener">RequestLister used to handle the async http response</param>
        /// <param name="multiPart">message body.</param>
        /// <param name="context">message context ,will pass as the same in done()</param>
        public static void Post(
                 string url,
                 Arg[] inputArgs,
                 Arg[] httpArgs,
                 IRequestListener listener,
                 PostData multiPart,
                 object context)
        {

            Async(HttpConnection.Post, url, inputArgs, httpArgs, listener,
                    multiPart, context);
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Cancels this http requst.
        /// </summary>
        public void Cancel()
        {
            _interrupted = true;
            try
            {
                Thread.Sleep(3000);
            }catch(Exception)
            {
            }
           
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Start a thread to process this http request/response, application shall
        /// not call this function directly.
        /// </summary>
        private void Run()
        {
            Response response = new Response();
            try
            {
                DoHttp(response);
            }
            catch (OutOfMemoryException ex)
            {
                GC.Collect();
                response._ex = ex;
            }
            catch (Exception ex)
            {

                response._ex = ex;
            }
            finally
            {
                if (_listener != null)
                {
                    try
                    {
                        _listener.Done(_context, response);
                    }
                    catch (Exception th)
                    {
                        Console.WriteLine(th.StackTrace);
                    }
                }
            }
        }

        private const int BufferSize = 1024;
        private object _context;
        private string _url;
        private string _method;
        private Arg[] _httpArgs;
        private Arg[] _inputArgs;
        private PostData _multiPart;
        private IRequestListener _listener;
        private Thread _thread;
        private volatile bool _interrupted;
        private int _totalToSend;
        private int _totalToReceive;
        private int _sent;
        private int _received;
        private readonly UTF8Encoding _utf8 = new UTF8Encoding();


        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// make a sync request.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="inputArgs">The input args.</param>
        /// <param name="httpArgs">The HTTP args.</param>
        /// <param name="listener">The listener.</param>
        /// <param name="multiPart">The multi part.</param>
        /// <returns></returns>
        private static Response Sync(
                 string method,
                 string url,
                 Arg[] inputArgs,
                 Arg[] httpArgs,
                 IRequestListener listener,
                 PostData multiPart)
        {

            var request = new Request
                              {
                                  _method = method,
                                  _url = url,
                                  _httpArgs = httpArgs,
                                  _inputArgs = inputArgs,
                                  _multiPart = multiPart,
                                  _listener = listener
                              };

            var response = new Response();
            request.DoHttp(response);
            return response;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// make async http request.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="inputArgs">The input args.</param>
        /// <param name="httpArgs">The HTTP args.</param>
        /// <param name="listener">The listener.</param>
        /// <param name="multiPart">The multi part.</param>
        /// <param name="context">The context.</param>
        private static void Async(
                 string method,
                 string url,
                 Arg[] inputArgs,
                 Arg[] httpArgs,
                 IRequestListener listener,
                 PostData multiPart,
                 object context)
        {

            var request = new Request
                              {
                                  _method = method,
                                  _context = context,
                                  _listener = listener,
                                  _url = url,
                                  _httpArgs = httpArgs,
                                  _inputArgs = inputArgs,
                                  _multiPart = multiPart
                              };

            request._thread = new Thread(request.Run);
            request._thread.Start();
        }

        private Request()
        {
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// data may be large, send in chunks while reporting progress and checking
        /// for interruption
        /// </summary>
        /// <param name="os">The os.</param>
        /// <param name="data">The data.</param>
        private void Write(Stream os, byte[] data)
        {

            if (_interrupted)
            {
                return;
            }

            // optimization if a small amount of data is being sent
            if (data.Length <= BufferSize)
            {
                os.Write(data, 0, data.Length);
                _sent += data.Length;
                if (_listener != null)
                {
                    try
                    {
                        _listener.WriteProgress(_context, _sent, _totalToSend);
                    }
                    catch(Exception th)
                    {
                        Console.WriteLine(th.StackTrace);
                    }
                }
            }
            else
            {
                var offset = 0;
                int length;
                do
                {
                    length = Math.Min(BufferSize, data.Length - offset);
                    if (length > 0)
                    {
                        os.Write(data, offset, length);
                        offset += length;
                        _sent += length;
                        if (_listener != null)
                        {
                            try
                            {
                                _listener.WriteProgress(_context, _sent, _totalToSend);
                            }
                            catch (Exception th)
                            {
                                Console.WriteLine(th.StackTrace);
                            }
                        }
                    }
                } while (!_interrupted && length > 0);
            }
        }


        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// do an http request.
        /// </summary>
        /// <param name="response">The response.</param>
        private void DoHttp(Response response)
        {

            var args = new StringBuilder();
            if (_inputArgs != null)
            {
                if (_inputArgs.Length > 0)
                {
                    for (var i = 0; i < _inputArgs.Length; i++)
                    {
                        if (_inputArgs[i] != null)
                        {
                            args.Append(Encode(_inputArgs[i].GetKey()));
                            args.Append("=");
                            args.Append(Encode(_inputArgs[i].GetValue()));
                            if (i + 1 < _inputArgs.Length &&
                                    _inputArgs[i + 1] != null)
                            {
                                args.Append("&");
                            }
                        }
                    }
                }
            }

            var location = new StringBuilder(_url);
            if (HttpConnection.Get.Equals(_method) && args.Length > 0)
            {
                location.Append("?");
                location.Append(args.ToString());
            }

            HttpConnection conn = null;
            try
            {
                conn = Connector.Open(location.ToString());
                conn.SetRequestMethod(_method);
                if (_httpArgs != null)
                {
                    for (int i = 0; i < _httpArgs.Length; i++)
                    {
                        if (_httpArgs[i] != null)
                        {
                            string value = _httpArgs[i].GetValue();
                            if (value != null)
                            {
                                conn.SetRequestProperty(_httpArgs[i].GetKey(), value);
                            }
                        }
                    }
                }

                if (_interrupted)
                {
                    return;
                }

                if (HttpConnection.Post.Equals(_method))
                {
                    Stream os = null;
                    try
                    {
                        os = conn.OpenOutputStream();
                        WritePostData(args, os);
                    }
                    finally
                    {
                        if (os != null)
                        {
                            try
                            {
                                os.Close();
                            }
                            catch (IOException )
                            {
                            }
                        }
                    }
                }

                if (_interrupted)
                {
                    return;
                }

                response._responseCode = conn.GetResponseCode();
                CopyResponseHeaders(conn, response);

                if (response._responseCode != HttpConnection.HttpOk)
                {
                    // TODO: handle redirects
                    return;
                }

                if (_interrupted)
                {
                    return;
                }

                ProcessContentType(conn, response);
                ReadResponse(conn, response);
            }
            catch (OutOfMemoryException )
            {
                GC.Collect();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// write post data.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <param name="os">The os.</param>
        private void WritePostData(StringBuilder args, Stream os)
        {
            if (_multiPart != null)
            {
                var multipartBoundaryBits = _utf8.GetBytes(_multiPart.GetBoundary());
                var newline = _utf8.GetBytes("\r\n");
                var dashdash = _utf8.GetBytes("--");

                // estimate totalBytesToSend
                var parts = _multiPart.GetParts();
                for (var i = 0; i < parts.Length; i++)
                {
                    var headers = parts[i].GetHeaders();
                    for (var j = 0; j < headers.Length; j++)
                    {
                        _totalToSend += _utf8.GetBytes(headers[j].GetKey()).Length;
                        _totalToSend += _utf8.GetBytes(headers[j].GetValue()).Length;
                        _totalToSend += multipartBoundaryBits.Length + dashdash.Length + 3 * newline.Length;
                    }
                    _totalToSend += parts[i].GetData().Length;
                }
                // closing boundary marker
                _totalToSend += multipartBoundaryBits.Length + 2 * dashdash.Length + 2 * newline.Length;

                for (var i = 0; i < parts.Length && !_interrupted; i++)
                {
                    Write(os, newline);
                    Write(os, dashdash);
                    Write(os, multipartBoundaryBits);
                    Write(os, newline);

                    var wroteAtleastOneHeader = false;
                    var headers = parts[i].GetHeaders();
                    for (var j = 0; j < headers.Length; j++)
                    {
                        Write(os, _utf8.GetBytes(headers[j].GetKey() + ": " + headers[j].GetValue()));
                        Write(os, newline);
                        wroteAtleastOneHeader = true;
                    }
                    if (wroteAtleastOneHeader)
                    {
                        Write(os, newline);
                    }

                    Write(os, parts[i].GetData());
                }

                // closing boundary marker
                Write(os, newline);
                Write(os, dashdash);
                Write(os, multipartBoundaryBits);
                Write(os, dashdash);
                Write(os, newline);
            }
            else if (_inputArgs != null)
            {
                var argBytes = _utf8.GetBytes(args.ToString());
                _totalToSend = argBytes.Length;
                Write(os, argBytes);
            }
            else
            {
                throw new IOException("No data to POST -" +
                        " either input args or multipart must be non-null");
            }
        }


        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reads the response.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="response">The response.</param>
        private void ReadResponse(HttpConnection conn,
                 Response response)
        {
            int thisTotalToReceive = 0;
            _totalToReceive = conn.GetHeaderFieldInt(Arg.ContentLength, 0);

            byte[] cbuf = new byte[BufferSize];
            MemoryStream bos = null;
            Stream inputStream = null;
            try
            {
                inputStream = conn.OpenInputStream();
                bos = new MemoryStream();
                int nread;
                while ((nread = inputStream.Read(cbuf, 0, BufferSize)) > 0 && !_interrupted)
                {
                    bos.Write(cbuf, 0, nread);
                    _received += nread;
                    thisTotalToReceive += nread;
                    if (_listener != null)
                    {
                        try
                        {
                            _listener.ReadProgress(_context, _received, _totalToReceive);
                        }
                        catch (Exception th)
                        {
                            Console.WriteLine(th.StackTrace);
                        }
                    }
                }
            }
            finally
            {
                if (inputStream != null)
                {
                    inputStream.Close();
                }
                if (bos != null)
                {
                    bos.Close();
                }
                TotaldownloadedBytes += thisTotalToReceive;
            }

            if (_interrupted)
            {
                return;
            }

            response._rawArray = bos.ToArray();
            string content = _utf8.GetString(response._rawArray, 0, response._rawArray.Length);
            response._rawContent = content;
            try
            {
                response._result = Result.FromContent(content, response._contentType);
            }
            catch (JSONException e)
            {
                throw new IOException(e.Message);
            }
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// copy the response header.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="response">The response.</param>
        private static void CopyResponseHeaders(HttpConnection conn, Response response)
        {
            // pass 1 - count the number of headers
            int headerCount = 0;
            try
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    string key = conn.GetHeaderFieldKey(i);
                    string val = conn.GetHeaderField(i);
                    if (key == null || val == null)
                    {
                        break;
                    }
                    headerCount++;
                }
            }catch(Exception )
            {
            }

            response._headers = new Arg[headerCount];

            // pass 2 - now copy the headers
            for (int i = 0; i < headerCount; i++)
            {
                string key = conn.GetHeaderFieldKey(i);
                string val = conn.GetHeaderField(i);
                if (key == null || val == null)
                {
                    break;
                }
                response._headers[i] = new Arg(key, val);
            }

        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Processes the type of the content.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="response">The response.</param>
        private static void ProcessContentType(HttpConnection conn, Response response)
        {

            response._contentType = conn.getHeaderField(Arg.ContentType);
            if (response._contentType == null)
            {
                // assume UTF-8 and XML if not specified
                response._contentType = Result.ApplicationXmlContentType;
                response._charset = Utf8Charset;
                return;
            }
            var semi = response._contentType.IndexOf(';');
            if (semi >= 0)
            {
                response._charset = response._contentType.Substring(semi + 1).Trim();
                var eq = response._charset.IndexOf('=');
                if (eq < 0)
                {
                    throw new IOException("Missing charset value: " + response._charset);
                }
                response._charset = Unquote(response._charset.Substring(eq + 1).Trim());
                response._contentType = response._contentType.Substring(0, semi).Trim();
            }
            if (response._charset != null)
            {
                var charset = response._charset.ToLower();
                if (!(charset.StartsWith(Utf8Charset) ||
                        charset.EndsWith(Utf8Charset) ||
                        charset.StartsWith(Iso8859Charset) ||
                        charset.EndsWith(Iso8859Charset) ||
                        charset.StartsWith(Gb2312Charset) ||
                        charset.EndsWith(Gb2312Charset)))
                {
                    throw new IOException("Unsupported charset: " + response._charset);
                }
            }

        }

        private static string Unquote(string str)
        {
            if (str.StartsWith("\"") && str.StartsWith("\"") ||
                    str.StartsWith("'") && str.EndsWith("'"))
            {
                return str.Substring(1, str.Length - 1);
            }
            return str;
        }


        private static string Encode(string str)
        {
            if (str == null)
            {
                return null;
            }
            return str.Replace(' ', '+');
        }
    }
}
