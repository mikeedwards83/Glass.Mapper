using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Glass.Mapper.Sc.FakeDb
{
    public class FakeMediaConfig : MediaConfig
    {
        public FakeMediaConfig() {


            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<requestProtection />");
            ParseRequestProtectionConfiguration(xmlDoc.FirstChild);
        }

        
    }
}
