using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VSSystem.Extensions.Hosting.Extenstions;
using VSSystem.Extensions.Hosting.Models;
using VSSystem.Logger;

namespace VSSystem.Extensions.Hosting.Controllers
{
    public abstract class AController : Controller
    {
        protected string _Name;
        public string Name { get { return _Name; } }

        protected string _ServiceName, _ControllerName, _ServicePath;
        public string ServiceName { get { return _ServiceName; } }
        protected IConfiguration _configuration;
        protected ALogger _logger;
        public ALogger Logger { get { return _logger; } }
        protected string _privateKey;
        protected DirectoryInfo WorkingFolder => GlobalVariables.WorkingFolder;
        public AController(string name, string privateKey = "")
        {
            _Name = name;
            _privateKey = privateKey;
        }

        public AController(string name, string serviceName, ALogger logger, string privateKey = "")
        {
            _Name = name;
            _ServiceName = serviceName;
            _ServicePath = string.IsNullOrWhiteSpace(serviceName) ? "" : serviceName + "/";
            _logger = logger;
            _privateKey = privateKey;
        }

        protected virtual List<ApiDescription> _GetApiDescriptions()
        {
            List<ApiDescription> result = new List<ApiDescription>();
            return result;
        }
        protected virtual Task _ProcessApiContext(string path, string queryString)
        {
            try
            {
                return this.ResponseJsonAsync(DefaultResponse.Hello, System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }
            return Task.CompletedTask;
        }

        [Route("{service}/api/[controller]/{*url}")]
        [Route("{service}/api/[controller].aspx/{*url}")]
        [Route("{service}/api/[controller].ashx/{*url}")]
        [Route("api/[controller]/{*url}")]
        [Route("api/[controller].aspx/{*url}")]
        [Route("api/[controller].ashx/{*url}")]

        public virtual async Task ApiMethod()
        {
            string path = HttpContext.Request.Path;
            var splitPaths = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            path = string.Join("/", splitPaths) + "/";
            if (string.IsNullOrWhiteSpace(_ControllerName))
            {
                if (RouteData.Values.ContainsKey("controller"))
                {
                    _ControllerName = RouteData.Values["controller"].ToString();
                }
            }

            if (path.Equals($"api{_ServicePath}/" + _ControllerName + "/httpinfo/", StringComparison.InvariantCultureIgnoreCase))
            {
                List<ApiDescription> apiInfoObjs = _GetApiDescriptions();
                await this.ResponseJsonAsync(apiInfoObjs, System.Net.HttpStatusCode.OK);
            }
            else
            {
                string queryString = HttpContext.Request.QueryString.ToString();
                await _ProcessApiContext(path, queryString);
            }
        }

        protected virtual IActionResult _ProcessMvcContext(string path)
        {
            return Ok();
        }
        [Route("[controller]/{*url}")]
        [Route("[controller].aspx/{*url}")]
        [Route("[controller].ashx/{*url}")]
        [Route("{service}/[controller]/{*url}")]
        [Route("{service}/[controller].aspx/{*url}")]
        [Route("{service}/[controller].ashx/{*url}")]
        public async void MvcMethod()
        {
            try
            {

                string path = HttpContext.Request.Path;
                var splitPaths = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                path = string.Join("/", splitPaths) + "/";
                if (ControllerContext.RouteData.Values.ContainsKey("controller"))
                {
                    var ctrlVal = ControllerContext.RouteData.Values["controller"].ToString();
                    int ctrlIdx = path.IndexOf(ctrlVal, StringComparison.InvariantCultureIgnoreCase);

                    var actVal = path.Substring(ctrlIdx + ctrlVal.Length + 1)?.Split('/')?.FirstOrDefault();
                    if (ControllerContext.RouteData.Values.ContainsKey("action"))
                    {
                        if (string.IsNullOrEmpty(actVal))
                        {
                            actVal = "Index";
                        }
                        ControllerContext.RouteData.Values["action"] = actVal;
                    }
                }

                ActionContext actionContext = new ActionContext(ControllerContext.HttpContext, ControllerContext.RouteData, ControllerContext.ActionDescriptor, ControllerContext.ModelState);

                var mvcView = _ProcessMvcContext(path);
                try
                {
                    await mvcView.ExecuteResultAsync(actionContext);
                }
                catch (Exception ex)
                {
                    this.LogError(ex);
                }
            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }

        }

        #region Methods
        protected bool IsConnect()
        {
            return HttpMethods.IsConnect(HttpContext.Request.Method);
        }
        protected bool IsDelete()
        {
            return HttpMethods.IsDelete(HttpContext.Request.Method);
        }
        protected bool IsGet()
        {
            return HttpMethods.IsGet(HttpContext.Request.Method);
        }
        protected bool IsHead()
        {
            return HttpMethods.IsHead(HttpContext.Request.Method);
        }
        protected bool IsOptions()
        {
            return HttpMethods.IsOptions(HttpContext.Request.Method);
        }
        protected bool IsPatch()
        {
            return HttpMethods.IsPatch(HttpContext.Request.Method);
        }
        protected bool IsPost()
        {
            return HttpMethods.IsPost(HttpContext.Request.Method);
        }
        protected bool IsPut()
        {
            return HttpMethods.IsPut(HttpContext.Request.Method);
        }
        protected bool IsTrace()
        {
            return HttpMethods.IsTrace(HttpContext.Request.Method);
        }
        #endregion

    }
}