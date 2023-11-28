using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Fanex.Tool.TraceSP.Models;

namespace Fanex.Tool.TraceSP.Services
{
    interface ISiteService
    {
        List<Site> GetSites(string siteXmlPath);
    }
    public class SiteService : ISiteService
    {
        public List<Site> GetSites(string siteXmlPath)
        {
            var sites = new List<Site>();

            if (File.Exists(siteXmlPath))
            {
                var deserializer = new XmlSerializer(typeof(List<Site>), new XmlRootAttribute("Sites"));

                using (var stream = File.OpenRead(siteXmlPath))
                {
                    sites = deserializer.Deserialize(stream) as List<Site>;
                }
            }

            return sites.OrderBy(x => x.Name).ToList();
        }
    }
}
