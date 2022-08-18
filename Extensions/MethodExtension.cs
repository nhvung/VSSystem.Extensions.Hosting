using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using VSSystem.IO;
using VSSystem.Net;

namespace VSSystem.Extensions.Hosting.Extenstions
{
    public static partial class MethodExtension
    {
        public static async Task<Stream> GetRequestStreamAsync(this object sender, HttpContext context)
        {
            Stream result = new MemoryStream64();
            try
            {
                await context.Request.Body.CopyToAsync(result, context.RequestAborted);
                result.Seek(0, SeekOrigin.Begin);
            }
            catch { }
            return result;
        }
        public static async Task<byte[]> GetRequestBytesAsync(this object sender, HttpContext context, Encoding encoding = default)
        {
            byte[] result = default;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await context.Request.Body.CopyToAsync(ms, context.RequestAborted);
                    ms.Close();
                    result = ms.ToArray();
                }
            }
            catch { }
            return result;
        }
        public static async Task<string> GetRequestStringAsync(this object sender, HttpContext context, Encoding encoding = default)
        {
            if (encoding == null)
            {
                encoding = Encoding.ASCII;
            }
            string result = string.Empty;

            try
            {
                Stream stream = await GetRequestStreamAsync(sender, context);

                using (var sr = new StreamReader(stream, encoding))
                {
                    result = await sr.ReadToEndAsync();
                    sr.Close();
                }
            }
            catch { }
            return result;
        }
        public static async Task<TRequest> GetRequestObject<TRequest>(this object sender, HttpContext context, Encoding encoding = null)
        {

            try
            {
                string requestString = await GetRequestStringAsync(sender, context, encoding);
                var result = JsonConvert.DeserializeObject<TRequest>(requestString);
                return result;
            }
            catch //(Exception ex)
            {
            }
            return default;
        }

        public static async Task<double> ResponseStreamAsync(this object sender, HttpContext context, Stream stream, string contentType, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            double result = 0;
            try
            {
                if (stream == null)
                {
                    stream = new MemoryStream();
                }
                context.Response.ContentLength = stream.Length - stream.Position;
                context.Response.StatusCode = (int)statusCode;
                if (headers?.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        try
                        {
                            context.Response.Headers.Add(header.Key, header.Value);
                        }
                        catch
                        {
                        }
                    }
                }
                context.Response.ContentType = contentType;
                DateTime utcNow = DateTime.UtcNow;
                await stream.CopyToAsync(context.Response.Body, context.RequestAborted);
                TimeSpan ts = DateTime.UtcNow - utcNow;
                result = ts.TotalMilliseconds;
                await context.Response.Body.DisposeAsync();
            }
            catch
            {
            }
            return result;
        }
        async public static Task<double> ResponseBytesAsync(this object sender, HttpContext context, byte[] dataBytes, string contentType, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            double result = -1;
            try
            {
                using (var stream = new MemoryStream(dataBytes))
                {
                    result = await ResponseStreamAsync(sender, context, stream, contentType, statusCode, headers);
                    stream.Close();
                    stream.Dispose();
                }
            }
            catch { }
            return result;
        }
        public static Task<double> ResponseEmptyAsync(this object sender, HttpContext context, string contentType, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            return ResponseStreamAsync(sender, context, null, contentType, statusCode, headers);
        }

        public static Task<double> ResponseJsonAsync(this object sender, HttpContext context, object obj, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            try
            {
                if (obj != null)
                {
                    string jsonResponse = JsonConvert.SerializeObject(obj);
                    var dataBytes = Encoding.UTF8.GetBytes(jsonResponse);
                    return ResponseBytesAsync(sender, context, dataBytes, ContentType.Json, statusCode, headers);
                }
                else
                {
                    return ResponseEmptyAsync(sender, context, ContentType.Json, statusCode, headers);
                }
            }
            catch
            {
            }
            return Task.FromResult<double>(-1);
        }
        public static Task<double> ResponseZipAsync(this object sender, HttpContext context, byte[] zipBytes, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            try
            {
                return ResponseBytesAsync(sender, context, zipBytes, ContentType.Zip, statusCode, headers);
            }
            catch
            {
            }
            return Task.FromResult<double>(-1);
        }
        public static Task<double> ResponseZipAsync(this object sender, HttpContext context, Stream zipStream, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            try
            {
                return ResponseStreamAsync(sender, context, zipStream, ContentType.Zip, statusCode, headers);
            }
            catch
            {
            }
            return Task.FromResult<double>(-1);
        }
        public static async Task<double> ResponseTextAsync(this object sender, HttpContext context, string text, string contentType, HttpStatusCode statusCode, Encoding encoding = default, List<KeyValuePair<string, string>> headers = null)
        {
            double result = -1;
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    text = string.Empty;
                }
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                byte[] textBytes = encoding.GetBytes(text);
                result = await ResponseBytesAsync(sender, context, textBytes, contentType, statusCode, headers);
            }
            catch { }
            return result;
        }
        public static Task<double> ResponsePngImageAsync(this object sender, HttpContext context, byte[] imageBytes, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            try
            {
                return ResponseBytesAsync(sender, context, imageBytes, ContentType.Png, statusCode, headers);
            }
            catch
            {
            }
            return Task.FromResult<double>(-1);
        }
        public static Task<double> ResponseJpgImageAsync(this object sender, HttpContext context, byte[] imageBytes, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            try
            {
                return ResponseBytesAsync(sender, context, imageBytes, ContentType.Jpg, statusCode, headers);
            }
            catch
            {
            }
            return Task.FromResult<double>(-1);
        }
        public static Task<double> ResponseJpegImageAsync(this object sender, HttpContext context, byte[] imageBytes, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            try
            {
                return ResponseBytesAsync(sender, context, imageBytes, ContentType.Jpeg, statusCode, headers);
            }
            catch { }
            return Task.FromResult<double>(-1);
        }
        public static Task<double> ResponseXmlAsync(this object sender, HttpContext context, object src, HttpStatusCode statusCode, Encoding encoding = default, List<KeyValuePair<string, string>> headers = null)
        {

            try
            {
                if (src != null)
                {
                    if (encoding == null)
                        encoding = Encoding.UTF8;
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    XmlWriterSettings writerSettings = new XmlWriterSettings();
                    //writerSettings.OmitXmlDeclaration = true;
                    byte[] xmlBytes = new byte[0];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        StreamWriter xmlStream;
                        xmlStream = new StreamWriter(ms, encoding);
                        var xmlWr = XmlWriter.Create(xmlStream, writerSettings);
                        XmlSerializer serializer = new XmlSerializer(src.GetType());
                        serializer.Serialize(xmlWr, src, ns);
                        ms.Close();
                        ms.Dispose();
                        xmlBytes = ms.ToArray();
                    }
                    return ResponseBytesAsync(sender, context, xmlBytes, ContentType.Xml, statusCode, headers);
                }
                else
                {
                    return ResponseEmptyAsync(sender, context, ContentType.Xml, statusCode, headers);
                }
            }
            catch// (Exception ex)
            {
                return ResponseEmptyAsync(sender, context, ContentType.Xml, HttpStatusCode.InternalServerError, headers);
            }
        }
    }
}