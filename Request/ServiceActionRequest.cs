using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSSystem.Extensions.Hosting.Request
{
    public class ServiceActionRequest
    {

        string _ServiceName;
        public string ServiceName { get { return _ServiceName; } set { _ServiceName = value; } }

        List<string> _WorkerNames;
        public List<string> WorkerNames { get { return _WorkerNames; } set { _WorkerNames = value; } }
        public ServiceActionRequest()
        {
            _ServiceName = string.Empty;
            _WorkerNames = new List<string>();
        }
    }
}
