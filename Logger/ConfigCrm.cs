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

    public class ConfigCrm
    {
        IOrganizationService _service;
        public ConfigCrm(IOrganizationService service)
        {
            _service = service;
        }

        public string GetXmlWebResourceByName(string configWebResource)
        {
            var rmr = new RetrieveMultipleRequest();
            var resp = new RetrieveMultipleResponse();
            Entity wb = new Entity();

            var query = new QueryExpression()
            {
                EntityName = "webresource",
                ColumnSet = new ColumnSet("content"),
                Criteria = new FilterExpression
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions = 
                {
                    new ConditionExpression
                    {
                        AttributeName = "name",
                        Operator = ConditionOperator.Equal,
                        Values = { configWebResource }
                    }
                }
                }
            };

            rmr.Query = query;
            resp = (RetrieveMultipleResponse)_service.Execute(rmr);
            wb = (Entity)resp.EntityCollection.Entities[0];

            byte[] b = Convert.FromBase64String(wb.Attributes["content"].ToString());

            return System.Text.Encoding.UTF8.GetString(b);

        }
    }
}

