using System;
using System.Diagnostics;
using System.Text;

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

        Span<byte> buffer = stackalloc byte[Encoding.UTF8.GetByteCount(name)];
        int count = Encoding.UTF8.GetBytes(name.AsSpan(), buffer);
        Debug.Assert(count == buffer.Length);

        this.stream.Write(buffer.Slice(count));
        this.stream.WriteByte((byte)'>');
        this.valueWritten = true;
      }

      this.writingElement = false;
    }

    public void WriteElementString(XmlExcapedName name, char value)
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

    public void WriteElementString(XmlExcapedName name, string? value, bool escapeValue = true)
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

    public void WriteElementString(XmlExcapedName name, ReadOnlySpan<char> value, bool escapeValue = true)
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

    public void WriteElementString(XmlExcapedName name, byte value)
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

    public void WriteElementString(XmlExcapedName name, long value)
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

    public void WriteElementString(XmlExcapedName name, double value)
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

    public void WriteElementString(XmlExcapedName name, decimal value)
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

    public void WriteElementString(XmlExcapedName name, DateTime value)
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

    public void WriteElementString(XmlExcapedName name, DateTimeOffset value)
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

    public void WriteElementString(XmlExcapedName name, TimeSpan value)
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

    public void WriteElementString(XmlExcapedName name, Guid value)
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
