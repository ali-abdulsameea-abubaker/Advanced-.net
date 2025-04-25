using System;
using System.Xml.Serialization;

namespace WebApplication1.model
{
    [XmlRoot("Immunizations")]
    public class Immunization
    {
        [XmlElement("ImmunizationId")]
        public Guid Id { get; set; }

        [XmlElement("CreatedAt")]
        public DateTimeOffset CreationTime { get; set; }

        [XmlElement("Name")]
        public string OfficialName { get; set; }

        [XmlElement("TradeName")]
        public string? TradeName { get; set; }

        [XmlElement("Lot")]
        public string LotNumber { get; set; }

        [XmlElement("ExpiresOn")]
        public DateTimeOffset ExpirationDate { get; set; }

        [XmlElement("LastUpdated")]
        public DateTimeOffset? UpdatedTime { get; set; }
        
    }
}