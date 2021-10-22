using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using XmlTools.Helpers;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Contains methods to write elements.
  /// </summary>
  public sealed partial class Utf8XmlWriter
  {
    // Closes one element of specified tag name.
    public void WriteEndElement(XmlEncodedName name)
    {
      if (this.valueWritten)
      {
        Debug.Assert(!this.writingElement, "The value must always be false");
        this.stream.WriteByte((byte)'<');
        this.stream.WriteByte((byte)'/');
        this.stream.Write(name.Utf8Bytes);
      }
      else if (this.writingElement)
      {
        this.stream.WriteByte((byte)'/');
      }

      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    // Closes one element of specified tag name.
    public void WriteEndElement(string name)
    {
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      WriteEndElement(name.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteEndElement(ReadOnlySpan<char> name)
    {
      if (this.valueWritten)
      {
        Debug.Assert(!this.writingElement, "The value must always be false");
        this.stream.WriteByte((byte)'<');
        this.stream.WriteByte((byte)'/');
        this.stream.WriteUtf8EncodedString(name);
      }
      else if (this.writingElement)
      {
        this.stream.WriteByte((byte)'/');
      }

      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }
  }
}
