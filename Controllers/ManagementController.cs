using System;
using System.Text;
using System.Threading.Tasks;
using VSSystem.Extensions.Hosting.Extenstions;
using VSSystem.Extensions.Hosting.Models;
using VSSystem.Extensions.Hosting.Request;
using VSSystem.ServiceProcess.Hosting;

namespace VSSystem.Extensions.Hosting.Controllers
{
    public class ManagementController : AController
    {
        public ManagementController() : base("ManagementController")
        {
        }
        protected async override Task _ProcessApiContext(string path, string queryString)
        {
            if (IsPost())
            {
                if (path.Equals($"{_ServicePath}api/management/subhost/disableall/", StringComparison.InvariantCultureIgnoreCase))
                {
                    InjectionHosts.DisableAllHosts();
                    await this.ResponseJsonAsync(DefaultResponse.Success, System.Net.HttpStatusCode.OK);
                }
                else if (path.Equals($"{_ServicePath}api/management/subhost/enableall/", StringComparison.InvariantCultureIgnoreCase))
                {
                    InjectionHosts.EnableAllHosts();
                    await this.ResponseJsonAsync(DefaultResponse.Success, System.Net.HttpStatusCode.OK);
                }
                else if (path.Equals($"{_ServicePath}api/management/service/startall/", StringComparison.InvariantCultureIgnoreCase))
                {
                    await InjectionServices.StartAllServicesAsync();
                    await this.ResponseJsonAsync(DefaultResponse.Success, System.Net.HttpStatusCode.OK);
                }
                else if (path.Equals($"{_ServicePath}api/management/service/stopall/", StringComparison.InvariantCultureIgnoreCase))
                {
                    await InjectionServices.StopAllServicesAsync();
                    await this.ResponseJsonAsync(DefaultResponse.Success, System.Net.HttpStatusCode.OK);
                }
                else if (path.Equals($"{_ServicePath}api/management/service/disableallworkers/", StringComparison.InvariantCultureIgnoreCase))
                {
                    var requestObj = await this.GetRequestObject<ServiceActionRequest>(Encoding.UTF8);
                    if (requestObj != null)
                    {
                        await InjectionServices.DisableAllWorkersAsync(requestObj.ServiceName);
                    }
                    await this.ResponseJsonAsync(DefaultResponse.Success, System.Net.HttpStatusCode.OK);
                }
                else if (path.Equals($"{_ServicePath}api/management/disableallworkers/", StringComparison.InvariantCultureIgnoreCase))
                {
                    var requestObj = await this.GetRequestObject<ServiceActionRequest>(Encoding.UTF8);
                    if (requestObj != null)
                    {
                        await InjectionServices.DisableAllWorkersAsync(requestObj.ServiceName);
                    }
                    await this.ResponseJsonAsync(DefaultResponse.Success, System.Net.HttpStatusCode.OK);
                }
            }
            else
            {
                if (path.Equals($"{_ServicePath}api/management/subhost/list/", StringComparison.InvariantCultureIgnoreCase))
                {
                    var hostObjs = InjectionHosts.GetHosts();
                    if (hostObjs?.Count > 0)
                    {
                        await this.ResponseJsonAsync(hostObjs, System.Net.HttpStatusCode.OK);
                    }
                    else
                    {
                        await this.ResponseJsonAsync(DefaultResponse.NoRecordFound, System.Net.HttpStatusCode.OK);
                    }
                }
                else if (path.Equals($"{_ServicePath}api/management/service/list/", StringComparison.InvariantCultureIgnoreCase))
                {
                    var serviceObjs = InjectionServices.GetAllServices();
                    if (serviceObjs?.Count > 0)
                    {
                        await this.ResponseJsonAsync(serviceObjs, System.Net.HttpStatusCode.OK);
                    }
                    else
                    {
                        await this.ResponseJsonAsync(DefaultResponse.NoRecordFound, System.Net.HttpStatusCode.OK);
                    }
                }
                else if (path.Equals($"{_ServicePath}api/management/service/listworkers/", StringComparison.InvariantCultureIgnoreCase))
                {
                    string serviceName = HttpContext.Request.Query["servicename"];
                    var workerObjs = InjectionServices.GetWorkers(serviceName);
                    await this.ResponseJsonAsync(workerObjs, System.Net.HttpStatusCode.OK);
                }
                else if (path.Equals($"{_ServicePath}api/Management/service/disableworkers/", StringComparison.InvariantCultureIgnoreCase))
                {
                    var requestObj = await this.GetRequestObject<ServiceActionRequest>(Encoding.UTF8);
                    if (requestObj != null)
                    {
                        await InjectionServices.DisableWorkersAsync(requestObj.ServiceName, requestObj.WorkerNames);
                    }
                    await this.ResponseJsonAsync(DefaultResponse.Success, System.Net.HttpStatusCode.OK);
                }

                else
                {
                    await base._ProcessApiContext(path, queryString);
                }
            }


        }
    }
}
