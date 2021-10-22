using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Contains methods to write values.
  /// </summary>
  public sealed partial class Utf8XmlWriter
  {
    public void WriteStartAttribute(XmlEncodedName name)
    {
      WriteStartAttributeImpl(name.Utf8Bytes);
      this.writingAttribute = true;
    }

    public void WriteStartAttribute(string name)
    {
      if (name is null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      WriteStartAttribute(name.AsSpan());
    }

    public void WriteStartAttribute(ReadOnlySpan<char> name)
    {
      Span<byte> nameBytes = stackalloc byte[Encoding.UTF8.GetByteCount(name)];
      int count = Encoding.UTF8.GetBytes(name, nameBytes);
      Debug.Assert(count == nameBytes.Length);

      WriteStartAttributeImpl(nameBytes);
      this.writingAttribute = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteStartAttributeImpl(ReadOnlySpan<byte> name)
    {
      this.stream.WriteByte((byte)' ');
      this.stream.Write(name);
      this.stream.WriteByte((byte)'=');
      this.stream.WriteByte((byte)'"');
    }
  }
}
