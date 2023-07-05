/******************************************************************
* Licensed Materials - Property of HCL
* (c) Copyright HCL Technologies Ltd. 2015, 2021.
* Note to U.S. Government Users Restricted Rights.
******************************************************************/
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Watchfire.HttpProxy;
using Watchfire.InfraTypes;
using Watchfire.ScanTypes;
using Watchfire.ScanTypes.Utility;
using ProxyLite;

namespace BurpTrafficImporter
{
    class BurpToExd
    {
        private ExdWriter _exdWriter;

        public BurpToExd()
        {
        }
        public void Convert(string sourceFile, string targetFile)
        {
            Stream sourceStream = new FileStream(sourceFile, FileMode.Open);
            Stream targetStream = new FileStream(targetFile, FileMode.Create);
            Convert(sourceStream, targetStream);
            sourceStream.Close();
            targetStream.Close();
        }

        public void Convert(string sourceFile, Stream targetStream)
        {
            Stream sourceStream = new FileStream(sourceFile, FileMode.Open);
            Convert(sourceStream, targetStream);
            sourceStream.Close();
        }

        public void Convert(Stream sourceStream, Stream targetStream)
        {
            _exdWriter = new ExdWriter(new StreamWriter(targetStream));

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(sourceStream);
            XmlNodeList items = xmlDoc.SelectNodes("items/item");
            if (items != null)
            {
                int index = 0;
                WFCookieContainer cookiesContainer = new WFCookieContainer();
                foreach (XmlNode item in items)
                {
                    XmlNode requestNode = item.SelectSingleNode("request");
                    XmlNode responseNode = item.SelectSingleNode("response");
                    XmlNode urlNode = item.SelectSingleNode("url");

                    if (requestNode == null || responseNode == null || urlNode == null)
                    {
                        continue;
                    }
                    MemoryStream request = new MemoryStream();
                    ToMemoryStream(requestNode, ref request);
                    MemoryStream response = new MemoryStream();
                    ToMemoryStream(responseNode, ref response);
                    
                    string url = urlNode.InnerText;

                    RecordedProxyData data = GetProxyData(request, response, url, index, cookiesContainer);
                    _exdWriter.AddRequest(data);
                    index++;
                }
            }

            _exdWriter.Close();
        }

        private static void ToMemoryStream(XmlNode node, ref MemoryStream stream)
        {
            if (node.Attributes != null && node.Attributes["base64"].Value == "true")
            {
                byte[] decoded = System.Convert.FromBase64String(node.InnerText);
                stream.Write(decoded, 0, decoded.Length);
            }
            else
            {
                byte[] decoded = Encoding.UTF8.GetBytes(node.InnerText);
                stream.Write(decoded, 0, decoded.Length);
            }
        }
        private RecordedProxyData GetProxyData(MemoryStream request, MemoryStream response, string url, int requestIndex, WFCookieContainer cookieContainer)
        {

            HttpRequestInfo requestInfo = new HttpRequestInfo();
            TrafficResponseInfo responseInfo = new TrafficResponseInfo();

            ByteString requestBS = new ByteString(request.GetBuffer(), 0, (int)request.Position);
            requestInfo.ParseRequest(requestBS, url, true);

            responseInfo.AddResponseChunk(response.GetBuffer(), (int)response.Position);


            HttpParameterList parameters = RequestParameterParser.ParseParametersFromRequest(requestInfo);

            // parse all the variables from the request
            //HttpParameterList parameters = Proxy.RequestParameterParser.ParseParametersFromRequest(requestInfo, postData);

            Uri uri = ConstructUrl(requestInfo);

            WFCookieContainer reqCookies = new WFCookieContainer();
            try
            {
                List<SiteCookie> siteCookies = reqCookies.AddCookies(uri, requestInfo.Headers);
                List<SiteCookie> responseCookies = null;
                if (cookieContainer != null)
                {
                    responseCookies = cookieContainer.GetCookies(uri);
                }

                reqCookies = new WFCookieContainer();
                foreach (SiteCookie cookie in siteCookies)
                {
                    bool found = false;
                    if (responseCookies != null)
                    {
                        foreach (SiteCookie respCookie in responseCookies)
                        {
                            if (respCookie.Name.UnicodeString == cookie.Name.UnicodeString)
                            {
                                found = true;
                                respCookie.SetValue(cookie.Value);
                                reqCookies.Add(new SiteCookie(respCookie));
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        reqCookies.Add(cookie);
                    }
                }
            }
            catch
            {
                //
            }

            try
            {
                if (cookieContainer != null)
                {
                    cookieContainer.AddCookies(uri, responseInfo.Headers);
                }
            }
            catch
            {//
            }

            WFCookieContainer respCookies = new WFCookieContainer();
            try
            {
                respCookies.AddCookies(uri, responseInfo.Headers);
                if (cookieContainer != null)
                {
                    cookieContainer.AddCookies(uri, responseInfo.Headers);
                }
            }
            catch
            {//
            }

            //Construct response header
            string contentTypeHeader = responseInfo.ContentType;
            byte[] responseBinary = new byte[responseInfo.BodyLength];
            Array.Copy(response.GetBuffer(), responseInfo.BodyIndex, responseBinary, 0, responseInfo.BodyLength);
            Encoding responseEnc= Encoding.UTF8;

            //Construct response header
            if (responseInfo.Headers.ContainsKey(HeaderList.TransferEncodingHeaderName) && responseInfo.Headers[HeaderList.TransferEncodingHeaderName].Contains("chunked"))
            {
                responseInfo.Headers[HeaderList.TransferEncodingHeaderName].RemoveAll(value => value != null && value.Equals("chunked"));
                if (responseInfo.Headers[HeaderList.TransferEncodingHeaderName].Count == 0)
                {
                    responseInfo.Headers.Remove(HeaderList.TransferEncodingHeaderName);
                }
                responseInfo.Headers.Set(HeaderList.HeaderContentLength, responseBinary.Length.ToString());
            }

            string responseHeader = responseInfo.StatusLine + "\n" + responseInfo.Headers;

            RecordedProxyData data = new RecordedProxyData(requestInfo.Method, uri.AbsoluteUri, requestInfo.RequestEncoding,
                responseEnc, responseHeader, responseInfo.StatusCode, reqCookies, respCookies)
            {
                Ordinal = requestIndex,
                Request = requestBS,
                Parameters = parameters,
                ResponseBinary = responseBinary
            };

            return data;
        }

        /// <summary>
        /// Construct URI for the current request
        /// </summary>
        /// <returns>URI object for the current request</returns>
        public static Uri ConstructUrl(HttpRequestInfo requestInfo)
        {
            // Build the URL to save with Browsed data
            UriBuilder builder = new UriBuilder { Host = requestInfo.Host };

            if (!requestInfo.IsDefaultPort)
                builder.Port = requestInfo.Port;

            builder.Path = requestInfo.Path;

            if (requestInfo.IsSecure)
                builder.Scheme = "https";

            return builder.Uri;
        }

        class TrafficResponseInfo
        {
            public TrafficResponseInfo()
            {
                Headers = new HeaderList();
                BodyIndex = 0;
            }

            public void AddResponseChunk(byte[] responseChunck, int length)
            {
                int eoh = responseChunck.FindEndOfHttpHeader();
                if (eoh != -1)
                {
                    BodyIndex = eoh + 4;
                    BodyLength = length - BodyIndex;
                    int lineIndex = responseChunck.FindEndOfHttpHeaderLine(0);
                    StatusLine = Encoding.UTF8.GetString(responseChunck, 0, lineIndex);
                    int tmp = StatusLine.IndexOf(" ", 9, StringComparison.Ordinal);
                    StatusCode = int.Parse(StatusLine.Substring(9, tmp - 9));

                    lineIndex += 2;
                    int lastIndex = lineIndex;
                    lineIndex = responseChunck.FindEndOfHttpHeaderLine(lineIndex);
                    while (lastIndex != lineIndex)
                    {
                        string headerLine = Encoding.UTF8.GetString(responseChunck, lastIndex, lineIndex - lastIndex);
                        int headerIndex = headerLine.IndexOf(":", StringComparison.InvariantCulture);
                        Headers.Add(headerLine.Substring(0, headerIndex), headerLine.Substring(headerIndex + 1).TrimStart());
                        lineIndex += 2;
                        lastIndex = lineIndex;
                        lineIndex = responseChunck.FindEndOfHttpHeaderLine(lineIndex);
                    }
                }
            }

            public int BodyIndex { get; private set; }
            public int BodyLength { get; private set; }

            public HeaderList Headers { get; private set; }

            public string StatusLine { get; private set; }

            public int StatusCode { get; private set; }

            public string ContentType
            {
                get
                {
                    StringList ct = Headers["Content-Type"];
                    if (ct != null && ct.Count > 0)
                    {
                        return ct[0].ToLower();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
        }
    }

    public static class CodeExtensionsFunctions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteArr"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static int FindEndOfHttpHeaderLine(this byte[] byteArr, int startIndex)
        {
            bool slashR = false;
            int length = byteArr.Length;
            int offset = -1;
            for (int i = startIndex; i < length; i++)
            {
                if (byteArr[i] == '\r')
                {
                    slashR = true;
                    continue;
                }
                else if (byteArr[i] == '\n')
                {
                    if (slashR)
                    {
                        offset = i - 1;
                        break;
                    }
                }
                slashR = false;
            }

            return offset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteArr"></param>
        /// <returns></returns>
        public static int FindEndOfHttpHeader(this byte[] byteArr)
        {
            int state = 0;
            int length = byteArr.Length;
            int offset = -1;
            for (int i = 0; i < length; i++)
            {
                if (byteArr[i] == '\r')
                {
                    state++;
                    continue;
                }
                else if (byteArr[i] == '\n' && state > 0)
                {
                    state++;
                    if (state == 4)
                    {
                        offset = i - 3;
                        break;
                    }
                    continue;
                }
                state = 0;
            }

            return offset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="substr"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static IEnumerable<int> AllIndexesOf(this string str, string substr, bool ignoreCase = false)
        {
            if (string.IsNullOrWhiteSpace(str) || string.IsNullOrWhiteSpace(substr))
            {
                yield break;
            }

            int index = 0;
            while ((index = str.IndexOf(substr, index, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) != -1)
            {
                yield return index;
                index += 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strArr"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="sepeartor"></param>
        /// <returns></returns>
        public static string Join(this string[] strArr, int startIndex = 0, int length = -1, string sepeartor = null)
        {
            StringBuilder sb = new StringBuilder();
            bool sepExist = string.IsNullOrEmpty(sepeartor);
            if (length == -1)
            {
                length = strArr.Length;
            }
            for (int i = startIndex; i < length; i++)
            {
                sb.Append(strArr[i]);
                if (sepExist)
                {
                    sb.Append(sepeartor);
                }
            }
            return sb.ToString();
        }
    }
}
