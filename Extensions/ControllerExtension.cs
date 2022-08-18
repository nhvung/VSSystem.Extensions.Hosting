using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VSSystem.Extensions.Hosting.Controllers;
using VSSystem.Extensions.Hosting.Extenstions;

namespace VSSystem.Extensions.Hosting.Controllers
{
    public static partial class ControllerExtension
    {
        #region Request
        public static Task<byte[]> GetRequestBytesAsync(this AController sender)
        {
            return sender.GetRequestBytesAsync(sender.HttpContext);
        }
        public static Task<Stream> GetRequestStreamAsync(this AController sender)
        {
            return sender.GetRequestStreamAsync(sender.HttpContext);
        }
        public static Task<string> GetRequestStringAsync(this AController sender, Encoding encoding = null)
        {
            return sender.GetRequestStringAsync(sender.HttpContext, encoding);
        }
        public static Task<TRequest> GetRequestObject<TRequest>(this AController sender, Encoding encoding = null)
        {
            return sender.GetRequestObject<TRequest>(sender.HttpContext, encoding);
        }
        #endregion

        #region Response

        #region Async
        public static Task<double> ResponseJsonAsync(this AController sender, object obj, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            return sender.ResponseJsonAsync(sender.HttpContext, obj, statusCode, headers);
        }
        public static Task<double> ResponseZipAsync(this AController sender, byte[] zipBytes, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            return sender.ResponseZipAsync(sender.HttpContext, zipBytes, statusCode, headers);
        }
        public static Task<double> ResponseStreamAsync(this AController sender, Stream stream, string contentType, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            return sender.ResponseStreamAsync(sender.HttpContext, stream, contentType, statusCode, headers);
        }
        public static Task<double> ResponsePngImageAsync(this AController sender, byte[] imageBytes, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            return sender.ResponsePngImageAsync(sender.HttpContext, imageBytes, statusCode, headers);
        }
        public static Task<double> ResponseJpgImageAsync(this AController sender, byte[] imageBytes, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            return sender.ResponseJpgImageAsync(sender.HttpContext, imageBytes, statusCode, headers);
        }
        public static Task<double> ResponseJpegImageAsync(this AController sender, byte[] imageBytes, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            return sender.ResponseJpegImageAsync(sender.HttpContext, imageBytes, statusCode, headers);
        }
        public static Task<double> ResponseXmlAsync(this AController sender, object obj, HttpStatusCode statusCode, Encoding encoding = default, List<KeyValuePair<string, string>> headers = null)
        {
            return sender.ResponseXmlAsync(sender.HttpContext, obj, statusCode, encoding, headers);
        }
        public static Task<double> ResponseTextAsync(this AController sender, string text, string contentType, HttpStatusCode statusCode, Encoding encoding = default, List<KeyValuePair<string, string>> headers = null)
        {
            return sender.ResponseTextAsync(sender.HttpContext, text, contentType, statusCode, encoding, headers);
        }
        public static Task<double> ResponseEmptyAsync(this AController sender, string contentType, HttpStatusCode statusCode, List<KeyValuePair<string, string>> headers = null)
        {
            return sender.ResponseEmptyAsync(sender.HttpContext, contentType, statusCode, headers);
        }

        #endregion

        #endregion
    }
}