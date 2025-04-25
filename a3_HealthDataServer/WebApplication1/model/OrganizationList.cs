using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebApplication1.model
{
    [XmlRoot("Organizations")] // Define the root element name for the XML
    public class OrganizationList
    {
        [XmlElement("Organization")] // Define the name for each item in the list
        public List<Organization> Organizations { get; set; }
    }
}