using System;
using System.Diagnostics;
using System.Text;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Provides a way to transform UTF-8 text into a form that is suitable for XML element name and attribute name.
  /// </summary>
  /// <remarks>
  /// This can be used to cache and store known strings used for writing XML ahead of time by pre-encoding them up front.
  /// </remarks>
  public readonly struct XmlEncodedName : IEquatable<XmlEncodedName>
  {
    private readonly byte[] value;
    private readonly string text;

    private XmlEncodedName(byte[] value)
    {
      Debug.Assert(value != null);
      this.value = value;
      this.text = Encoding.UTF8.GetString(value);
    }

    public ReadOnlySpan<byte> Utf8Bytes => this.value;

    public static XmlEncodedName Encode(string value)
    {
      if (value is null)
      {
        throw new ArgumentNullException(nameof(value));
      }

      if (value.Length == 0)
      {
        throw new ArgumentException("The empty text '' is not a valid local name.", nameof(value));
      }

      byte[] encoded = EncodeToBytes(value.AsSpan());
      return new XmlEncodedName(encoded);
    }

    public static XmlEncodedName Encode(ReadOnlySpan<char> value)
    {
      if (value.IsEmpty)
      {
        throw new ArgumentException("The empty text '' is not a valid local name.", nameof(value));
      }

      byte[] encoded = EncodeToBytes(value);
      return new XmlEncodedName(encoded);
    }

    private static byte[] EncodeToBytes(ReadOnlySpan<char> value)
    {
      ValidateInput(value);

      byte[] buffer = new byte[Encoding.UTF8.GetByteCount(value)];
      Encoding.UTF8.GetBytes(value, buffer);
      return buffer;
    }

    private static void ValidateInput(ReadOnlySpan<char> value)
    {
      Debug.Assert(!value.IsEmpty);

      for (int i = 0; i < value.Length; i++)
      {
        var ch = value[i];
        var isValid = char.IsLetter(ch) || ch == '_';
        if (!isValid)
        {
          throw new ArgumentException($"Invalid name character in '{value.ToString()}'. The '{ch}' character, hexadecimal value 0x{Convert.ToByte(ch):X2}, cannot be included in a name.");
        }
      }
    }

    public bool Equals(XmlEncodedName other)
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
      if (obj is XmlEncodedName encodedText)
      {
        return Equals(encodedText);
      }
      return false;
    }

    public override int GetHashCode()
      => this.text == null ? 0 : text.GetHashCode();

    public override string ToString()
      => this.text ?? string.Empty;

    public static bool operator ==(XmlEncodedName left, XmlEncodedName right)
      => left.Equals(right);

    public static bool operator !=(XmlEncodedName left, XmlEncodedName right)
      => !(left == right);
  }
}
