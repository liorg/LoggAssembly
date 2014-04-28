using Guardian.Menta.Cache;
using Guardian.Menta.Common.Configuration.JSON;
using Guardian.Menta.Interfaces.Cache;
using Guardian.WF.Logger;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
//using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
//using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guardian.Menta.CRM.Workflow
{
    public class WFTestLogger : CodeActivity
    {
        protected IOrganizationService _Service { get; set; }
        protected IWorkflowContext _WorkflowContext { get; set; }
        protected CodeActivityContext _ExecutionContext { get; set; }
        protected ITracingService _TracingService { get; set; }
        protected static volatile string LoggerXmlSetting = null;
        protected static volatile object LockXmlSetting = new object();

        protected override void Execute(CodeActivityContext executionContext)
        {
            _ExecutionContext = executionContext;
            _TracingService = executionContext.GetExtension<ITracingService>();
            _WorkflowContext = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            _Service = serviceFactory.CreateOrganizationService(_WorkflowContext.UserId);

            WFlogger1 wflogger = new WFlogger1("General", _Service);
            var cache=GetCacheManagerByConfig(GetCacheConfig());

            cache.SetItem("somekey", () => { return "some valur to cache"; }, TimeSpan.FromHours(5));

            wflogger.WriteError("hhhh", "test");
            wflogger.WriteError("dddd2", "test");
            wflogger.WriteError("ddd3d", "test");
        }


        protected ConfigCaching GetCacheConfig()
        {
            var configurationFactory = new ConfigurationFactory();
            var configuration = configurationFactory.GetConfiguration(_Service);
            return configuration.ConfigCaching;

        }
        ICacheItem GetCacheManagerByConfig(ConfigCaching cacheConfig)
        {
            return CacheCrmManager.CacheManagerFactory(cacheConfig);
        }

    }
}
