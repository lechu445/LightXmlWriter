using System;
using XmlTools.Helpers;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Contains methods to write elements.
  /// </summary>
  public sealed partial class Utf8XmlWriter
  {
    public void WriteElementString(string name, string? value, bool escapeValue = true)
    {
      WriteStartElement(name);
      if (value == null)
      {
        WriteEndElement(name);
      }
      else
      {
        WriteValue(value, escapeValue);
        this.stream.WriteByte((byte)'<');
        this.stream.WriteByte((byte)'/');
        this.stream.WriteUtf8EncodedString(name);
        this.stream.WriteByte((byte)'>');
        this.valueWritten = true;
      }

      this.writingElement = false;
    }

    public void WriteElementString(string name, bool value)
    {
      WriteStartElement(name);
      WriteValue(value);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.WriteUtf8EncodedString(name);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(string name, char value)
    {
      // TODO: optimize
      WriteStartElement(name);
      WriteValue(value);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.WriteUtf8EncodedString(name);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(string name, int value)
    {
      // TODO: optimize
      WriteStartElement(name);
      WriteValue(value);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.WriteUtf8EncodedString(name);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(XmlEncodedName name, char value)
    {
      WriteStartElement(name);
      WriteValue(value);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.Write(name.Utf8Bytes);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(XmlEncodedName name, string? value, bool escapeValue = true)
    {
      WriteStartElement(name);
      if (value == null)
      {
        WriteEndElement(name);
      }
      else
      {
        WriteValue(value, escapeValue);
        this.stream.WriteByte((byte)'<');
        this.stream.WriteByte((byte)'/');
        this.stream.Write(name.Utf8Bytes);
        this.stream.WriteByte((byte)'>');
        this.valueWritten = true;
      }

      this.writingElement = false;
    }

    public void WriteElementString(XmlEncodedName name, ReadOnlySpan<char> value, bool escapeValue = true)
    {
      WriteStartElement(name);
      WriteValue(value, escapeValue);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.Write(name.Utf8Bytes);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(XmlEncodedName name, XmlEncodedValue value)
    {
      WriteStartElement(name);
      WriteValue(value);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.Write(name.Utf8Bytes);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(XmlEncodedName name, byte value)
    {
      WriteStartElement(name);
      WriteValue(value);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.Write(name.Utf8Bytes);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(XmlEncodedName name, long value)
    {
      WriteStartElement(name);
      WriteValue(value);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.Write(name.Utf8Bytes);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(XmlEncodedName name, double value)
    {
      WriteStartElement(name);
      WriteValue(value);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.Write(name.Utf8Bytes);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(XmlEncodedName name, decimal value)
    {
      WriteStartElement(name);
      WriteValue(value);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.Write(name.Utf8Bytes);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(XmlEncodedName name, DateTime value)
    {
      WriteStartElement(name);
      WriteValue(value);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.Write(name.Utf8Bytes);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(XmlEncodedName name, DateTimeOffset value)
    {
      WriteStartElement(name);
      WriteValue(value);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.Write(name.Utf8Bytes);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(XmlEncodedName name, TimeSpan value)
    {
      WriteStartElement(name);
      WriteValue(value);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.Write(name.Utf8Bytes);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(XmlEncodedName name, Guid value)
    {
      WriteStartElement(name);
      WriteValue(value);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      this.stream.Write(name.Utf8Bytes);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }
  }
}
