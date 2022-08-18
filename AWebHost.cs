using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using VSSystem.IO;
using VSSystem.Logger;
using VSSystem.Net;
using VSSystem.ServiceProcess;
using VSSystem.ServiceProcess.Extensions;

namespace VSSystem.Extensions.Hosting
{
    public abstract class AWebHost : VSSystem.ServiceProcess.Hosting.AHost
    {
        static Task Main(string[] args) { return Task.CompletedTask; }
        protected int _port;
        protected List<string> _Urls;
        public List<string> Urls { get { return _Urls; } set { _Urls = value; } }
        protected bool _allowSynchronousIO;
        public AWebHost(string name, int port,
            string rootName = default,
            string privateKey = default) : base(name, rootName, privateKey)
        {
            _port = port;
            _allowSynchronousIO = false;
        }

        #region Inherit
        async public override Task StartAsync(string[] args, CancellationToken cancellationToken = default)
        {
            try
            {
                await _InitializeParametersAsync(args);
                var host = await _InitializeHost(args);
                var hostTask = host.RunAsync(cancellationToken);

                try
                {

                    int processID = 0;
                    var currentProcess = Process.GetCurrentProcess();
                    if (currentProcess != null)
                    {
                        processID = currentProcess.Id;
                    }
                    if (OSVersion == VSSystem.Models.OSVersion.Windows)
                    {
                        this.CreateWindowsStopServiceFile(WorkingFolder, _Name, processID);
                        this.CreateWindowsRestartServiceFile(WorkingFolder, _Name, processID);
                    }
                    else if (OSVersion == VSSystem.Models.OSVersion.Unix)
                    {
                        this.CreateUbuntuStopServiceFile(WorkingFolder, _Name, processID);
                        this.CreateUbuntuRestartServiceFile(WorkingFolder, _Name, processID);
                        VSSystem.Extensions.CLIExtension.Execute(new System.Collections.Generic.List<string>() { "chmod 777 *.sh" });
                    }
                }
                catch { }

                this.LogInfo($"{_Name} Host started.");

                _ = _RunAdditionComponents();

                await hostTask;
            }
            catch (Exception ex)
            {
                this.LogErrorWithTag("StartAsync", ex);
            }
        }
        #endregion

        #region Private
        protected override Task<IHostBuilder> _InitializeHostBuilder(string[] args)
        {
            var builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);
            _UseConfiguration(args);
            _InitializeLogger();
            if (_port > 0)
            {
                builder = builder.ConfigureWebHostDefaults(webBuilder =>
                    {
                        _ConfigurePort(webBuilder);
                        _UseStartup(webBuilder);
                        _InitializeInjectionHosts(args);
                    });
            }
            return Task.FromResult(builder);
        }
        protected virtual void _ConfigurePort(IWebHostBuilder webHostBuilder)
        {
            try
            {
                _Urls = new List<string>();
                string webSection = "web";

                webHostBuilder.ConfigureKestrel((ctx, opts) =>
                {
                    opts.AllowSynchronousIO = _allowSynchronousIO;
                    if (_port > 0)
                    {
                        opts.Listen(IPAddress.Any, _port, listenOpts =>
                        {
                            listenOpts.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
                        });
                        if (_IPv4Addresses?.Count > 0)
                        {
                            foreach (var ipAddress in _IPv4Addresses)
                            {
                                _Urls.Add(string.Format("http://{0}:{1}", ipAddress, _port));
                            }
                        }
                    }

                    int maxConcurrentConnection;
                    string sConnection = _ini.ReadValue<string>(webSection, "max_concurrent_connections", "100");
                    int.TryParse(sConnection, out maxConcurrentConnection);
                    if (maxConcurrentConnection <= 0)
                    {
                        maxConcurrentConnection = 1;
                    }
                    opts.Limits.MaxConcurrentConnections = maxConcurrentConnection;
                    opts.Limits.MaxConcurrentUpgradedConnections = maxConcurrentConnection;
                    opts.Limits.MaxRequestBodySize = long.MaxValue;

                    _AdditionalHostInit();
                });
            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }
        }

        protected abstract void _UseStartup(IWebHostBuilder webHostBuilder);

        protected override void _UseConfiguration(string[] args)
        {
            try
            {
                _ini = _LoadInstanceConfiguration().Result;
                string webSection = "web";

                string sHttpPort = _ini.ReadValue<string>(webSection, "http_port", _port.ToString());
                int.TryParse(sHttpPort, out _port);
            }
            catch { }
        }
        #endregion
    }
}