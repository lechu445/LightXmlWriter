using System.Xml;

namespace XmlTools.Test.Examples
{
  public static class Simple_XML_Writer
  {
    public static void Write(LightXmlWriter writer)
    {
      const string prefix = "soap";
      const string ns = "req";

      writer.WriteStartElement(prefix, "Envelope", "http://www.w3.org/2003/05/soap-envelope");
      writer.WriteAttributeString("xmlns", ns, null, "http://request.rentacar.karve.com/");
      writer.WriteStartElement(prefix, "Header", null);
      writer.WriteElementString(ns, "Password", null, "some password");
      writer.WriteElementString(ns, "User", null, "some login");
      writer.WriteEndElement(prefix, "Header");

      writer.WriteStartElement(prefix, "Body", null);
      writer.WriteStartElement(ns, "CreateReserveRequest", null);
      WriteBody(writer);
      writer.WriteEndElement(ns, "CreateReserveRequest");
      writer.WriteEndElement(prefix, "Body");
      writer.WriteEndElement(prefix, "Envelope");
    }

    private static void WriteBody(LightXmlWriter writer)
    {
      writer.WriteElementString("ReserveId", true);
      writer.WriteStartElement("ClientName");
      writer.WriteValue("MR");
      writer.WriteValue(' ');
      writer.WriteValue("John");
      writer.WriteValue(' ');
      writer.WriteValue("Doe");
      writer.WriteEndElement("ClientName");
      writer.WriteElementString("PickUpOfficeId", 88);
      writer.WriteElementString("PickUpDate", "2017-10-10", escapeValue: false);
      writer.WriteElementString("PickUpTime", "09:00", escapeValue: false);
      writer.WriteElementString("DropOffOfficeId", "89", escapeValue: false);
      writer.WriteElementString("DropOffDate", "2017-10-15", escapeValue: false);
      writer.WriteElementString("DropOffTime", "10:00", escapeValue: false);

      writer.WriteElementString("ConceptsIds", "1");
      writer.WriteElementString("ConceptsIds", "2");
      writer.WriteElementString("ConceptsIds", "3");

      writer.WriteElementString("Flight", "LH12344");
      writer.WriteElementString("CarTypeId", "FDMR");
      writer.WriteElementString("RateId", "XXX TEST");
    }

    public static void Write(XmlWriter writer)
    {
      const string prefix = "soap";
      const string ns = "req";

      writer.WriteStartElement(prefix, "Envelope", "http://www.w3.org/2003/05/soap-envelope");
      writer.WriteAttributeString("xmlns", ns, null, "http://request.rentacar.karve.com/");
      writer.WriteStartElement(prefix, "Header", null);
      writer.WriteElementString(ns, "Password", null, "some password");
      writer.WriteElementString(ns, "User", null, "some login");
      writer.WriteEndElement(); //Header

      writer.WriteStartElement(prefix, "Body", null);
      writer.WriteStartElement(ns, "CreateReserveRequest", null);
      WriteBody(writer);
      writer.WriteEndElement(); //CreateReserveRequest
      writer.WriteEndElement(); //Body
      writer.WriteEndElement(); //Envelope
    }

    private static void WriteBody(XmlWriter writer)
    {
      writer.WriteElementString("ReserveId", "True");
      writer.WriteStartElement("ClientName");
      writer.WriteValue("MR");
      writer.WriteRaw(" ");
      writer.WriteValue("John");
      writer.WriteRaw(" ");
      writer.WriteValue("Doe");
      writer.WriteEndElement(); //ClientName
      writer.WriteElementString("PickUpOfficeId", "88");
      writer.WriteElementString("PickUpDate", "2017-10-10");
      writer.WriteElementString("PickUpTime", "09:00");
      writer.WriteElementString("DropOffOfficeId", "89");
      writer.WriteElementString("DropOffDate", "2017-10-15");
      writer.WriteElementString("DropOffTime", "10:00");

      writer.WriteElementString("ConceptsIds", "1");
      writer.WriteElementString("ConceptsIds", "2");
      writer.WriteElementString("ConceptsIds", "3");

      writer.WriteElementString("Flight", "LH12344");
      writer.WriteElementString("CarTypeId", "FDMR");
      writer.WriteElementString("RateId", "FTI TEST");
    }
  }
}
