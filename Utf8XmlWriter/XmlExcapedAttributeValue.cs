using System;
using System.Diagnostics;
using System.Text;

namespace Tools.Text.Xml
{
  public readonly struct XmlExcapedAttributeValue : IEquatable<XmlExcapedAttributeValue>
  {
    private readonly byte[] value;
    private readonly string text;

    private XmlExcapedAttributeValue(byte[] value)
    {
      Debug.Assert(value != null);
      this.value = value;
      this.text = Encoding.UTF8.GetString(value);
    }

    public ReadOnlySpan<byte> Utf8Bytes => this.value;

    public static XmlExcapedAttributeValue Encode(string value)
    {
      if (value is null)
      {
        throw new ArgumentNullException(nameof(value));
      }

      byte[] encoded = EncodeToBytes(value);
      return new XmlExcapedAttributeValue(encoded);
    }

    public static XmlExcapedAttributeValue Encode(ReadOnlySpan<char> value)
    {
      byte[] encoded = EncodeToBytes(value);
      return new XmlExcapedAttributeValue(encoded);
    }

    private static byte[] EncodeToBytes(ReadOnlySpan<char> value)
    {
      return EncodeToBytes(new string(value));
    }

    private static byte[] EncodeToBytes(string value)
    {
      var escapedText = value
        .Replace("&", "&amp;")
        .Replace(">", "&gt;")
        .Replace("<", "&lt;")
        .Replace("\"", "&quot;")
        .Replace("\r\n", "\n")
        .Replace("\n", "&#xA;");

      return Encoding.UTF8.GetBytes(escapedText);
    }

    public bool Equals(XmlExcapedAttributeValue other)
    {
      if (value == null)
      {
        return other.value == null;
      }
      else
      {
        return value.Equals(other.value);
      }
    }

    public override bool Equals(object? obj)
    {
      if (obj is XmlExcapedAttributeValue encodedText)
      {
        return Equals(encodedText);
      }
      return false;
    }

    public override int GetHashCode()
      => this.text == null ? 0 : this.text.GetHashCode();

    public override string ToString()
      => this.text ?? string.Empty;

    public static bool operator ==(XmlExcapedAttributeValue left, XmlExcapedAttributeValue right)
      => left.Equals(right);

    public static bool operator !=(XmlExcapedAttributeValue left, XmlExcapedAttributeValue right)
      => !(left == right);
  }
}
