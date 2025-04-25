using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebApplication1.model
{
    [XmlRoot("Patients")] // Define the root element name for the XML
    public class PatientList
    {
        [XmlElement("Patient")] // Define the name for each item in the list
        public List<Patient> Patients { get; set; }
    }
}