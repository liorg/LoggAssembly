using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using Microsoft.Xrm.Sdk;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Guardian.WF.Logger
{
    public class WFlogger1
    {
        string _CategoryName = null;
        IOrganizationService _service;
        
        protected static volatile string LoggerXmlSetting = null;
        protected static volatile object LockXmlSetting = new object();

        public WFlogger1(string categoryName,IOrganizationService service)
        {
            _CategoryName = categoryName; 
            _service = service;
             ReplaceEnterpriceLibraryConfig();

        }

        void ReplaceEnterpriceLibraryConfig()
        {

            if (LoggerXmlSetting == null)
            {
                lock (LockXmlSetting)
                {
                    if (LoggerXmlSetting == null)
                    {
                        var crm = new ConfigCrm(_service);
                        LoggerXmlSetting = crm.GetXmlWebResourceByName("new_entlib");
                    }
                }
            }

            //var builder = new ConfigurationSourceBuilder();//
            //var configSource = new FileConfigurationSource(@"c:\\tmp\v.xml"); //new XmlLoggerConfigurationSource(xml);
            var configSource = new XmlLoggerConfigurationSource(LoggerXmlSetting);

            //builder.UpdateConfigurationWithReplace(configSource);
            EnterpriseLibraryContainer.Current = EnterpriseLibraryContainer.CreateDefaultContainer(configSource);
        }


        public void WriteError(string msg,string title)
        {
            var logEntry = new LogEntry();
            if (!string.IsNullOrEmpty(_CategoryName))
                logEntry.Categories.Add(_CategoryName);
            logEntry.EventId = 100;
            logEntry.Priority = 2;
            logEntry.Message = msg;
            logEntry.Title = title;
            logEntry.ExtendedProperties.Add("OrganizationService", _service);
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEntry);
        }

        public void WriteError(string msg)
        {
            WriteError(msg, "Error");
        }

        public void WriteError(Exception e,string title="")
        {
          
        }

    }
}
