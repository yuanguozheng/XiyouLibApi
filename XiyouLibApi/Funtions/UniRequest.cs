/*
 * Open Web API for Xi'an University Of Posts & Telecommunications Library.
 * 
 * Copyright (C) 2014 Xiyou Mobile Application Club Microsoft Technology Team All Rights Reserved.
 * 
 * File Description: Class for sending request to server and receiving response from server.
 * 
 * Created By Yuan Guozheng 2014/6/1.
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using XiyouLibApi.Models;
using XiyouLibApi.Exceptions;

namespace XiyouLibApi.Funtions
{
    /// <summary>
    /// Delegate handler, callback for asynchronous result.
    /// </summary>
    /// <param name="Result">Reseponse result.</param>
    public delegate void RequestFinishedHandler(BasedReturned Result);
    public class UniRequest
    {
        #region //Data Varibles

        private Dictionary<string, string> Params = new Dictionary<string, string>();

        #endregion

        #region //Attributions

        /// <summary>
        /// Get or set the type of request content encoding, default: UTF-8.
        /// </summary>
        public string RequestCodeType { get; set; }

        /// <summary>
        /// Get or set the type of response content encoding, default: UTF-8.
        /// </summary>
        public string ResponseCodeType { get; set; }

        /// <summary>
        /// Get or set whethere operations use Sync or Async, default: false.
        /// </summary>
        public bool IsAsync { get; set; }

        /// <summary>
        /// Get or set whethere response content is bytes, default: false.
        /// </summary>
        public bool ReturnBytes { get; set; }

        public enum RequestType { GET, POST }

        #endregion

        #region //Varibles for entire class

        /// <summary>
        /// Use for http request.
        /// </summary>
        HttpWebRequest req;

        /// <summary>
        /// An object of delegate handler.
        /// </summary>
        public static RequestFinishedHandler OnRequestFinished;

        /// <summary>
        /// Set += & -= ways for handler.
        /// </summary>
        public event RequestFinishedHandler RequsetFinished
        {
            add
            {
                OnRequestFinished += new RequestFinishedHandler(value);
            }
            remove
            {
                OnRequestFinished -= new RequestFinishedHandler(value);
            }
        }

        #endregion

        /// <summary>
        /// Create a http request.
        /// </summary>
        /// <param name="URL">URL for requesting, like 'http://baidu.com/'.</param>
        /// <param name="Method">Enum, mark for method of request, default is 'GET'.</param>
        /// <param name="WithSession">Whether the request need login session, default is 'true'.</param>
        /// <param name="IsAsync">Whether the request is asynchronous, default is 'true'.</param>
        public UniRequest
            (
                string URL,
                RequestType Method = RequestType.GET,
                bool WithSession = false
            )
        {
            RequestCodeType = "UTF-8";  //Init default value of request encoding.

            ResponseCodeType = "UTF-8";  //Init default value of response encoding.

            IsAsync = false;  //Init default value of way of request.

            ReturnBytes = false;  //Init default value of type of response content.

            req = (HttpWebRequest)HttpWebRequest.Create(URL);  //Set Url.

            if (WithSession)  //Set Session for api which use login verify.
            {
                req.CookieContainer = Global.GlobalCookie;
            }
            else
            {
                req.CookieContainer = new CookieContainer();
            }

            switch (Method)  //Set request methond for HttpWebRequest.
            {
                case RequestType.GET:
                    req.Method = "GET";
                    break;
                case RequestType.POST:
                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";
                    break;
            }

            if (Method == RequestType.POST)
            {
                ProcParams();  //When use POST, proccess params to URLEncoded string and write to stream.
            }

            DoRequest();  //Receive response.
        }

        /// <summary>
        /// Add a string param with key and value.
        /// </summary>
        /// <param name="key">Key for a value.</param>
        /// <param name="value">Specific value.</param>
        public void AddParams(string key, string value)
        {
            if (Params.ContainsKey(key))
            {
                throw new ParamExistedException();  //Exception, the key has already in the dictionary.
            }
            Params.Add(key, value);
        }

        /// <summary>
        /// Serialize params collections to string and write the request stream.
        /// </summary>
        private void ProcParams()
        {
            if (Params.Count == 0)
            {
                return;
            }
            byte[] ParamBinary; //Binary for saving string.
            StringBuilder ParamString = new StringBuilder();
            foreach (var item in Params)
            {
                ParamString.AppendFormat("{0}={1}", item.Key, item.Value);
                if (item.Key != Params.Keys.ElementAt(Params.Count - 1))
                {
                    ParamString.Append("&");
                }
            }
            ParamBinary = Encoding.GetEncoding(RequestCodeType).GetBytes(ParamString.ToString());

            if (IsAsync)
            {
                WriteParamsToStreamAsync(ParamBinary);  //Use asynchronous type to write.
            }
            else
            {
                WriteParamsToStream(ParamBinary);  //Use synchronous type to write.
            }
        }

        /// <summary>
        /// Write bytes to stream by synchronous way.
        /// </summary>
        /// <param name="Params">Bytes of params' URLEncoded string.</param>
        private void WriteParamsToStream(byte[] Params)
        {
            using (Stream stream = req.GetRequestStream())
            {
                stream.Write(Params, 0, Params.Length);
            }
        }

        /// <summary>
        /// Write bytes to stream by asynchronous way.
        /// </summary>
        /// <param name="Params">Bytes of params' URLEncoded string.</param>
        private async void WriteParamsToStreamAsync(byte[] Params)
        {
            using (Stream stream = await req.GetRequestStreamAsync())
            {
                await stream.WriteAsync(Params, 0, Params.Length);
            }
        }

        /// <summary>
        /// Receive response from remote server.
        /// </summary>
        private async void DoRequest()
        {
            HttpWebResponse res;  //Http response.

            try
            {
                if (IsAsync)
                {
                    res = (HttpWebResponse)await req.GetResponseAsync();  //Use asynchronous way to get response.
                }
                else
                {
                    res = (HttpWebResponse)req.GetResponse();  //Use synchronous way to get response.
                }
            }
            catch
            {
                OnRequestFinished(new BasedReturned
                    {
                        Result = false,
                        Detail = "GET_RESPONSE_FAILED"
                    }); //Error.
                return;
            }

            object ReturnContent;

            try
            {
                using (Stream stream = res.GetResponseStream())  //Return string or bytes.
                {
                    if (ReturnBytes)
                    {
                        ReturnContent = await GetBytes(stream);
                    }
                    else
                    {
                        ReturnContent = await GetString(stream);
                    }
                }
            }
            catch
            {
                OnRequestFinished(new BasedReturned
                {
                    Result = false,
                    Detail = "RESPONSE_STREAM_ERROR"
                });  //Error.
                return;
            }

            OnRequestFinished(new BasedReturned
                {
                    Result = true,
                    Detail = ReturnContent
                });  //Successfully Return.
        }

        /// <summary>
        /// Get string from response stream.
        /// </summary>
        /// <param name="stream">Response Stream.</param>
        /// <returns>String of response content.</returns>
        private async Task<string> GetString(Stream stream)
        {
            string ResponseContent = "";
            StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(ResponseCodeType)); //Encoding.
            if (IsAsync)
            {
                ResponseContent = await reader.ReadToEndAsync();
            }
            else
            {
                ResponseContent = reader.ReadToEnd();
            }
            reader.Close();
            stream.Close();
            return ResponseContent;
        }

        /// <summary>
        /// Get bytes from response stream.
        /// </summary>
        /// <param name="stream">Response stream.</param>
        /// <returns>Bytes of response content.</returns>
        private async Task<byte[]> GetBytes(Stream stream)
        {
            byte[] ResponseContent = new byte[stream.Length];
            while (stream.CanRead)
            {
                byte[] buffer = new byte[256];
                if (IsAsync)
                {
                    await stream.ReadAsync(buffer, 0, 256);  //Read to buffer use asynchronous way.
                }
                else
                {
                    stream.Read(buffer, 0, 256);  //Read to buffer use synchronous way.
                }
                int n = ResponseContent.Length;
                for (int i = 0; i < buffer.Length; i++)
                {
                    ResponseContent[n + i] = buffer[i];  //Add.
                }
            }
            stream.Close();
            return ResponseContent;
        }
    }
}