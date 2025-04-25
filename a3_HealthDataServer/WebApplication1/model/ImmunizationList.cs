using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebApplication1.model
{
    [XmlRoot("Immunizations")] // Define the root element name for the XML
    public class ImmunizationList
    {
        [XmlElement("Immunization")] // Define the name for each item in the list
        public List<Immunization> Immunizations { get; set; }
    }
}