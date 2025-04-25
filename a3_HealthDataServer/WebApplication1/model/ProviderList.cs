using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebApplication1.model
{
    [XmlRoot("Providers")] // Define the root element name for the XML
    public class ProviderList
    {
        [XmlElement("Provider")] // Define the name for each item in the list
        public List<Provider> Providers { get; set; }
    }
}