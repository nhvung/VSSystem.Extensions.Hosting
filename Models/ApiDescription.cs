using System.Collections.Generic;

namespace VSSystem.Extensions.Hosting.Models
{
    public class ApiDescription
    {
        string _Url;
        public string Url { get { return _Url; } set { _Url = value; } }

        string _Method;
        public string Method { get { return _Method; } set { _Method = value; } }

        string _ContentType;
        public string ContentType { get { return _ContentType; } set { _ContentType = value; } }
        List<KeyValuePair<string, string>> _Headers;
        public List<KeyValuePair<string, string>> Headers { get { return _Headers; } set { _Headers = value; } }

        object _Request;
        public object Request { get { return _Request; } set { _Request = value; } }

        object _Response;
        public object Response { get { return _Response; } set { _Response = value; } }

        public ApiDescription()
        {
            _Url = string.Empty;
            _Method = string.Empty;
            _Headers = new List<KeyValuePair<string, string>>();
            _Request = new object();
            _Response = new object();
            _ContentType = string.Empty;
        }
    }
}