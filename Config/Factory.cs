using System;
using System.IO;
using System.Text;
using Microsoft.Xrm.Sdk;
using System.Net;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;
namespace Guardian.Menta.Common.Configuration.JSON
{
    public class ConfigurationFactory
    {
        const string ConstConfigurationName = "new_configuration";
        const string ConstConfigurationMessengeName = "new_messenger";
        const string ConstWebResourcesUrl = "{0}://{1}/{2}/WebResources";
        public Configuration GetConfiguration(IOrganizationService service)
        {
            string strConfig = GetConfiguration(ConstConfigurationName, service);

            var config = SerializeObject.JsonDeserializeToObject<Configuration>(strConfig);
            return config; 
        }

        public Configuration GetConfigurationByUrl(IOrganizationService service)
        {
            WebRequest request = GetWebRequest(service, Path.Combine(ConstWebResourcesUrl, ConstConfigurationName));
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        var reader = new StreamReader(stream);
                        string strConfig = reader.ReadToEnd();
                        var config = SerializeObject.JsonDeserializeToObject<Configuration>(strConfig);
                        response.Close();
                        return config;
                    }
                }
            }
            catch (WebException ex)
            {

                throw;
            }
        }

        public Configuration GetConfigurationByUrl(string scheme, string host, string organizationName)
        {
            WebRequest request = GetWebRequest(scheme, host, organizationName,Path.Combine(ConstWebResourcesUrl, ConstConfigurationName));
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        var reader = new StreamReader(stream);
                        string strConfig = reader.ReadToEnd();
                        var config = SerializeObject.JsonDeserializeToObject<Configuration>(strConfig);
                        response.Close();
                        return config;
                    }
                }
            }
            catch (WebException ex)
            {

                throw;
            }
        }

        private WebRequest GetWebRequest(string scheme, string host, string organizationName, string url)
        {
            url = string.Format(url, scheme, host, organizationName);
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            return request;
        }

      

        
     
        private WebRequest GetWebRequest(IOrganizationService service,string url)
        {
            if (!(service is OrganizationServiceProxy))
                return null;

            Uri serviceUri = (service as OrganizationServiceProxy).ServiceConfiguration.CurrentServiceEndpoint.Address.Uri;

            if (serviceUri.Segments.Length < 2)
                return null;

            url = string.Format(url, serviceUri.Scheme, serviceUri.Host, serviceUri.Segments[1]);
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            return request;
        }

        private string GetConfiguration(string configurationName, IOrganizationService service)
        {
            string resource = GetResource(configurationName, service);
            return resource;
        }

        /// <summary>
        /// Retrieves Crm Web Resource
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        private string GetResource(string resourceName, IOrganizationService service)
        {

            QueryExpression resQuery = new QueryExpression()
            {
                EntityName = "webresource",
                ColumnSet = new ColumnSet(new[] { "content" }),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("name", ConditionOperator.Equal, resourceName)
                    }
                }
            };

            EntityCollection res = service.RetrieveMultiple(resQuery);
            Entity resource = res.Entities.First();
            if (resource == null)
            {
                throw new Exception("Resours with name " + resourceName + " does't exsist.");
            }

            byte[] content = Convert.FromBase64String(resource["content"].ToString());
            return Encoding.UTF8.GetString(content);
        }
    }
}
