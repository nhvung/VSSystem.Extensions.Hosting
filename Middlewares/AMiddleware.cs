using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using VSSystem.Extensions.Hosting.Extenstions;
using VSSystem.Extensions.Hosting.Models;
using VSSystem.ServiceProcess.Hosting;

namespace VSSystem.Extensions.Hosting
{
    public abstract class AMiddleware
    {
        protected readonly RequestDelegate _next;
        protected string _servicePath;
        protected string _serviceName;
        protected string _privateKey;
        public AMiddleware(RequestDelegate next)
        {
            _next = next;
            _servicePath = string.Empty;
            _serviceName = string.Empty;
        }
        public AMiddleware(RequestDelegate next, string serviceName, string privateKey = "")
        {
            _next = next;
            _serviceName = serviceName;
            _servicePath = string.IsNullOrWhiteSpace(serviceName) ? "" : serviceName + "/";
            _privateKey = privateKey;
        }
        public Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path;
            var splitPaths = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            path = string.Join("/", splitPaths) + "/";
            if (!string.IsNullOrWhiteSpace(_servicePath))
            {
                if (path.IndexOf(_servicePath, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    bool hostEnabled = InjectionHosts.GetHostEnabled(_serviceName);
                    if (!hostEnabled)
                    {
                        return _InvokeNotFound(context);
                    }
                }
            }

            return _Invoke(context, path);
        }
        protected virtual Task _Invoke(HttpContext context, string path)
        {
            return _next?.Invoke(context);
        }
        protected virtual Task _InvokeNotFound(HttpContext context)
        {
            return this.ResponseJsonAsync(context, DefaultResponse.PathNotFound, System.Net.HttpStatusCode.NotFound);
        }
    }
}
