using System.Xml;

namespace XmlTools.Test.Examples
{
  public static class OTA_Standard_XML_Writer_XmlWriter
  {
    public static void Write(XmlWriter writer)
    {
      const string prefix = "soapenv";

      writer.WriteStartElement(prefix, "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
      writer.WriteAttributeString("xmlns", "soapenv", null, "http://schemas.xmlsoap.org/soap/envelope/");
      writer.WriteAttributeString("xmlns", "ns", null, "http://www.opentravel.org/OTA/2003/05");

      writer.WriteStartElement(prefix, "Header", null);
      writer.WriteEndElement();

      writer.WriteStartElement(prefix, "Body", null);

      WriteOtaVehResRQ(writer);

      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    private static void WriteOtaVehResRQ(XmlWriter writer)
    {
      writer.WriteStartElement(null, "OTA_VehResRQ", "http://www.opentravel.org/OTA/2003/05");
      writer.WriteAttributeString("PrimaryLangID", "EN");
      writer.WriteAttributeString("Target", "Test");
      writer.WriteAttributeString("Version", "3.0");
      writer.WriteAttributeString("xmlns", "http://www.opentravel.org/OTA/2003/05");
      writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
      WritePOS(writer);
      writer.WriteStartElement("VehResRQCore");
      writer.WriteAttributeString("Status", "Available");
      WriteVehRentalCore(writer);
      WriteCustomer(writer);

      WriteVendorPref(writer, "ET");
      WriteVehPref(writer);

      writer.WriteStartElement("DriverType");
      writer.WriteAttributeString("Age", "25");
      writer.WriteEndElement();

      WriteRateQualifier(writer);
      WriteSpecialEquipPrefs(writer);
      writer.WriteStartElement("TPA_Extensions");

      writer.WriteStartElement("TPA_Extension_Flags");
      writer.WriteAttributeString("EnhancedTotalPrice", "true");
      writer.WriteEndElement();

      writer.WriteEndElement();
      writer.WriteEndElement();

      writer.WriteStartElement("VehResRQInfo");
      WriteArrivalDetails(writer, "LH1234");
      writer.WriteStartElement("RentalPaymentPref");

      writer.WriteStartElement("Voucher");
      writer.WriteAttributeString("SeriesCode", "cust-abc123");
      writer.WriteEndElement();

      writer.WriteEndElement();

      writer.WriteStartElement("Reference");
      writer.WriteAttributeString("ID", "ER1AL");
      writer.WriteAttributeString("DateTime", "2013-05-01T19:36:00");
      writer.WriteAttributeString("Type", "16");
      writer.WriteEndElement();

      writer.WriteStartElement("TPA_Extensions");

      writer.WriteStartElement("TPA_Extensions_Ref");
      writer.WriteAttributeString("AltResNumber", "q8ot");
      writer.WriteEndElement();

      writer.WriteStartElement("TPA_Extensions_Ref");
      writer.WriteAttributeString("CoRef1", "cust-abc123");
      writer.WriteEndElement();

      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    private static void WritePOS(XmlWriter writer)
    {
      writer.WriteStartElement("POS");

      writer.WriteStartElement("Source");
      writer.WriteAttributeString("ISOCountry", "FR");
      writer.WriteStartElement("RequestorID");
      writer.WriteAttributeString("Type", "4");
      writer.WriteAttributeString("ID", "XMLRTA");
      writer.WriteStartElement("CompanyName");
      writer.WriteAttributeString("Code", "EX");
      writer.WriteAttributeString("CompanyShortName", "EHIXMLTEST");
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();

      writer.WriteStartElement("Source");
      writer.WriteStartElement("RequestorID");
      writer.WriteAttributeString("Type", "4");
      writer.WriteAttributeString("ID", "00000000");
      writer.WriteAttributeString("ID_Context", "IATA");
      writer.WriteEndElement();
      writer.WriteEndElement();

      writer.WriteEndElement();
    }

    private static void WriteVehRentalCore(XmlWriter writer)
    {
      writer.WriteStartElement("VehRentalCore");

      writer.WriteAttributeString("PickUpDateTime", "2017-05-30T09:00:00");
      writer.WriteAttributeString("ReturnDateTime", "2017-05-31T09:00:00");

      writer.WriteStartElement("PickUpLocation");
      writer.WriteAttributeString("LocationCode", "TIAC61");
      writer.WriteEndElement();

      writer.WriteStartElement("ReturnLocation");
      writer.WriteAttributeString("LocationCode", "TIAC61");
      writer.WriteEndElement();

      writer.WriteEndElement();
    }

    private static void WriteCustomer(XmlWriter writer)
    {
      writer.WriteStartElement("Customer");
      writer.WriteStartElement("Primary");

      writer.WriteStartElement("PersonName");
      writer.WriteElementString("NamePrefix", "Mr");
      writer.WriteElementString("GivenName", "John");
      writer.WriteElementString("Surname", "Doe");
      writer.WriteEndElement();

      writer.WriteStartElement("Telephone");
      writer.WriteAttributeString("PhoneUseType", "3");
      writer.WriteAttributeString("PhoneTechType", "1");
      writer.WriteAttributeString("CountryAccessCode", "");
      writer.WriteAttributeString("AreaCityCode", "");
      writer.WriteAttributeString("PhoneNumber", "666-777-888");
      writer.WriteEndElement();

      writer.WriteElementString("Email", "john.doe@example.com");

      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    private static void WriteVendorPref(XmlWriter writer, string code)
    {
      writer.WriteStartElement("VendorPref");
      writer.WriteAttributeString("Code", code);
      writer.WriteEndElement();
    }

    private static void WriteVehPref(XmlWriter writer)
    {
      writer.WriteStartElement("VehPref");
      writer.WriteAttributeString("Code", "EBMR");
      writer.WriteEndElement();
    }

    private static void WriteRateQualifier(XmlWriter writer)
    {
      writer.WriteStartElement("RateQualifier");
      writer.WriteAttributeString("RateQualifier", "ER1AL");
      writer.WriteEndElement();
    }

    private static void WriteSpecialEquipPrefs(XmlWriter writer)
    {
      writer.WriteStartElement("SpecialEquipPrefs");

      writer.WriteStartElement("SpecialEquipPref");
      writer.WriteAttributeString("EquipType", "7");
      writer.WriteAttributeString("Quantity", "1");
      writer.WriteEndElement();

      writer.WriteStartElement("SpecialEquipPref");
      writer.WriteAttributeString("EquipType", "8");
      writer.WriteAttributeString("Quantity", "1");
      writer.WriteEndElement();

      writer.WriteEndElement();
    }

    private static void WriteArrivalDetails(XmlWriter writer, string val)
    {
      writer.WriteStartElement("ArrivalDetails");
      writer.WriteAttributeString("TransportationCode", "14");
      writer.WriteAttributeString("Number", val.Substring(2));

      writer.WriteStartElement("OperatingCompany");
      writer.WriteAttributeString("Code", val.Substring(0, 2));
      writer.WriteEndElement();

      writer.WriteEndElement();
    }
  }
}
