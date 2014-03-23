using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Guardian.WF.Logger
{

   

    public class XmlLoggerConfigurationSource : IConfigurationSource
    {
        private string _xml;

        public XmlLoggerConfigurationSource(string xml)
        {
            _xml = xml;
        }
      
        public ConfigurationSection GetSection(string sectionName)
        {
            if (sectionName == LoggingSettings.SectionName)
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ConformanceLevel = ConformanceLevel.Auto;

                using (StringReader stringReader = new StringReader(_xml))
                {
                    XmlTextReader xmlReader = new XmlTextReader(stringReader);
                    xmlReader.WhitespaceHandling = WhitespaceHandling.None;

                    var serializableSection = (LoggingSettings)Activator.CreateInstance(typeof(LoggingSettings));
                    ((ConfigurationSection)serializableSection).GetType().InvokeMember("DeserializeSection", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, serializableSection, new[] { 
                        xmlReader
                    });
                    //serializableSection.ReadXml(xmlReader);
                    return serializableSection;
                }
            }
            return null;
        }

        public void Add(string sectionName, ConfigurationSection configurationSection)
        {
        }

        public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
        }

        public void Remove(string sectionName)
        {
        }

        public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
        }

        public event EventHandler<ConfigurationSourceChangedEventArgs> SourceChanged;

        public void Dispose()
        {
            
        }
    }

  


}
