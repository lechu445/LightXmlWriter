﻿using System;
using System.Diagnostics;
using System.Text;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Provides a way to transform UTF-8 text into a form that is suitable for XML element value.
  /// </summary>
  /// <remarks>
  /// This can be used to cache and store known strings used for writing XML ahead of time by pre-encoding them up front.
  /// </remarks>
  public readonly struct XmlEncodedValue : IEquatable<XmlEncodedValue>
  {
    private readonly byte[] value;
    private readonly string text;

    private XmlEncodedValue(byte[] value)
    {
      Debug.Assert(value != null);
      this.value = value;
      this.text = Encoding.UTF8.GetString(value);
    }

    public ReadOnlySpan<byte> Utf8Bytes => this.value;

    public static XmlEncodedValue Encode(string value)
    {
      if (value is null)
      {
        throw new ArgumentNullException(nameof(value));
      }

      byte[] encoded = EncodeToBytes(value);
      return new XmlEncodedValue(encoded);
    }

    public static XmlEncodedValue Encode(ReadOnlySpan<char> value)
    {
      byte[] encoded = EncodeToBytes(value);
      return new XmlEncodedValue(encoded);
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
        .Replace("\r\n", "\n")
        .Replace("\n", "\r\n");

      return Encoding.UTF8.GetBytes(escapedText);
    }

    public bool Equals(XmlEncodedValue other)
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
      if (obj is XmlEncodedValue encodedText)
      {
        return Equals(encodedText);
      }
      return false;
    }

    public override int GetHashCode()
      => this.text == null ? 0 : text.GetHashCode();

    public override string ToString()
      => this.text ?? string.Empty;

    public static bool operator ==(XmlEncodedValue left, XmlEncodedValue right)
      => left.Equals(right);

    public static bool operator !=(XmlEncodedValue left, XmlEncodedValue right)
      => !(left == right);
  }
}