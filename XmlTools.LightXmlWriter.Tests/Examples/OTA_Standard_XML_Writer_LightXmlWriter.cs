using System;

namespace XmlTools.Test.Examples
{
  public static class OTA_Standard_XML_Writer_LightXmlWriter
  {
    public static void Write(LightXmlWriter writer)
    {
      const string prefix = "soapenv";

      writer.WriteStartElement(prefix, "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
      writer.WriteAttributeString("xmlns", "ns", null, "http://www.opentravel.org/OTA/2003/05");

      writer.WriteStartElement(prefix, "Header", null);
      writer.WriteEndElement(prefix, "Header");

      writer.WriteStartElement(prefix, "Body", null);

      WriteOtaVehResRQ(writer);

      writer.WriteEndElement(prefix, "Body");
      writer.WriteEndElement(prefix, "Envelope");
    }

    private static void WriteOtaVehResRQ(LightXmlWriter writer)
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
      writer.WriteAttributeString("Age", 25);
      writer.WriteEndElement("DriverType");

      WriteRateQualifier(writer);
      WriteSpecialEquipPrefs(writer);
      writer.WriteStartElement("TPA_Extensions");

      writer.WriteStartElement("TPA_Extension_Flags");
      writer.WriteAttributeString("EnhancedTotalPrice", "true");
      writer.WriteEndElement("TPA_Extension_Flags");

      writer.WriteEndElement("TPA_Extensions");
      writer.WriteEndElement("VehResRQCore");

      writer.WriteStartElement("VehResRQInfo");
      WriteArrivalDetails(writer, "LH1234");
      writer.WriteStartElement("RentalPaymentPref");

      writer.WriteStartElement("Voucher");
      writer.WriteAttributeString("SeriesCode", "cust-abc123");
      writer.WriteEndElement("Voucher");

      writer.WriteEndElement("RentalPaymentPref");

      writer.WriteStartElement("Reference");
      writer.WriteAttributeString("ID", "ER1AL");
      writer.WriteAttributeString("DateTime", "2013-05-01T19:36:00");
      writer.WriteAttributeString("Type", "16");
      writer.WriteEndElement("Reference");

      writer.WriteStartElement("TPA_Extensions");

      writer.WriteStartElement("TPA_Extensions_Ref");
      writer.WriteAttributeString("AltResNumber", "q8ot");
      writer.WriteEndElement("TPA_Extensions_Ref");

      writer.WriteStartElement("TPA_Extensions_Ref");
      writer.WriteAttributeString("CoRef1", "cust-abc123");
      writer.WriteEndElement("TPA_Extensions_Ref");

      writer.WriteEndElement("TPA_Extensions");
      writer.WriteEndElement("VehResRQInfo");
      writer.WriteEndElement("OTA_VehResRQ");
    }

    private static void WritePOS(LightXmlWriter writer)
    {
      writer.WriteStartElement("POS");

      writer.WriteStartElement("Source");
      writer.WriteAttributeString("ISOCountry", "FR");
      writer.WriteStartElement("RequestorID");
      writer.WriteAttributeString("Type", 4);
      writer.WriteAttributeString("ID", "XMLRTA");
      writer.WriteStartElement("CompanyName");
      writer.WriteAttributeString("Code", "EX");
      writer.WriteAttributeString("CompanyShortName", "EHIXMLTEST");
      writer.WriteEndElement("CompanyName");
      writer.WriteEndElement("RequestorID");
      writer.WriteEndElement("Source");

      writer.WriteStartElement("Source");
      writer.WriteStartElement("RequestorID");
      writer.WriteAttributeString("Type", 4);
      writer.WriteAttributeString("ID", "00000000");
      writer.WriteAttributeString("ID_Context", "IATA");
      writer.WriteEndElement("RequestorID");
      writer.WriteEndElement("Source");

      writer.WriteEndElement("POS");
    }

    private static void WriteVehRentalCore(LightXmlWriter writer)
    {
      writer.WriteStartElement("VehRentalCore");

      writer.WriteAttributeString("PickUpDateTime", "2017-05-30T09:00:00");
      writer.WriteAttributeString("ReturnDateTime", "2017-05-31T09:00:00");

      writer.WriteStartElement("PickUpLocation");
      writer.WriteAttributeString("LocationCode", "TIAC61");
      writer.WriteEndElement("PickUpLocation");

      writer.WriteStartElement("ReturnLocation");
      writer.WriteAttributeString("LocationCode", "TIAC61");
      writer.WriteEndElement("ReturnLocation");

      writer.WriteEndElement("VehRentalCore");
    }

    private static void WriteCustomer(LightXmlWriter writer)
    {
      writer.WriteStartElement("Customer");
      writer.WriteStartElement("Primary");

      writer.WriteStartElement("PersonName");
      writer.WriteElementString("NamePrefix", "Mr");
      writer.WriteElementString("GivenName", "John");
      writer.WriteElementString("Surname", "Doe");
      writer.WriteEndElement("PersonName");

      writer.WriteStartElement("Telephone");
      writer.WriteAttributeString("PhoneUseType", "3");
      writer.WriteAttributeString("PhoneTechType", "1");
      writer.WriteAttributeString("CountryAccessCode", "");
      writer.WriteAttributeString("AreaCityCode", "");
      writer.WriteAttributeString("PhoneNumber", "666-777-888");
      writer.WriteEndElement("Telephone");

      writer.WriteElementString("Email", "john.doe@example.com");

      writer.WriteEndElement("Primary");
      writer.WriteEndElement("Customer");
    }

    private static void WriteVendorPref(LightXmlWriter writer, string code)
    {
      writer.WriteStartElement("VendorPref");
      writer.WriteAttributeString("Code", code);
      writer.WriteEndElement("VendorPref");
    }

    private static void WriteVehPref(LightXmlWriter writer)
    {
      writer.WriteStartElement("VehPref");
      writer.WriteAttributeString("Code", "EBMR");
      writer.WriteEndElement("VehPref");
    }

    private static void WriteRateQualifier(LightXmlWriter writer)
    {
      writer.WriteStartElement("RateQualifier");
      writer.WriteAttributeString("RateQualifier", "ER1AL");
      writer.WriteEndElement("RateQualifier");
    }

    private static void WriteSpecialEquipPrefs(LightXmlWriter writer)
    {
      writer.WriteStartElement("SpecialEquipPrefs");

      writer.WriteStartElement("SpecialEquipPref");
      writer.WriteAttributeString("EquipType", "7");
      writer.WriteAttributeString("Quantity", 1);
      writer.WriteEndElement("SpecialEquipPref");

      writer.WriteStartElement("SpecialEquipPref");
      writer.WriteAttributeString("EquipType", "8");
      writer.WriteAttributeString("Quantity", 1);
      writer.WriteEndElement("SpecialEquipPref");

      writer.WriteEndElement("SpecialEquipPrefs");
    }

#if !NET462
    private static void WriteArrivalDetails(LightXmlWriter writer, string val)
    {
      writer.WriteStartElement("ArrivalDetails");
      writer.WriteAttributeString("TransportationCode", 14);
      writer.WriteAttributeString("Number", val.AsSpan(2));

      writer.WriteStartElement("OperatingCompany");
      writer.WriteAttributeString("Code", val.AsSpan(0, 2));
      writer.WriteEndElement("OperatingCompany");

      writer.WriteEndElement("ArrivalDetails");
    }
#else
    private static void WriteArrivalDetails(LightXmlWriter writer, string val)
    {
      writer.WriteStartElement("ArrivalDetails");
      writer.WriteAttributeString("TransportationCode", 14);
      writer.WriteAttributeString("Number", val.Substring(2));

      writer.WriteStartElement("OperatingCompany");
      writer.WriteAttributeString("Code", val.Substring(0, 2));
      writer.WriteEndElement("OperatingCompany");

      writer.WriteEndElement("ArrivalDetails");
    }
#endif
  }
}
