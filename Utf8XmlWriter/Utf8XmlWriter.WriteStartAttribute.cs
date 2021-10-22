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
    public void WriteStartAttribute(XmlExcapedName name)
    {
      WriteStartAttributeImpl(name.Utf8Bytes);
      this.writingAttribute = true;
    }

    public void WriteStartAttribute(string name)
    {
      Span<byte> nameBytes = stackalloc byte[Encoding.UTF8.GetByteCount(name)];
      int count = Encoding.UTF8.GetBytes(name.AsSpan(), nameBytes);
      Debug.Assert(count == nameBytes.Length);

      WriteStartAttributeImpl(nameBytes);
      this.writingAttribute = true;
    }

    public void WriteStartAttribute(XmlExcapedName prefix, XmlExcapedAttributeValue name)
    {
      WriteStartAttributeImpl(prefix.Utf8Bytes, name.Utf8Bytes);
      this.writingAttribute = true;
    }

    public void WriteStartAttribute(XmlExcapedName prefix, string name)
    {
      Span<byte> nameBytes = stackalloc byte[Encoding.UTF8.GetByteCount(name)];
      int count = Encoding.UTF8.GetBytes(name.AsSpan(), nameBytes);
      Debug.Assert(count == nameBytes.Length);

      WriteStartAttributeImpl(prefix.Utf8Bytes, nameBytes);
      this.writingAttribute = true;
    }

    public void WriteStartAttribute(string? prefix, XmlExcapedName name)
    {
      if (prefix is null)
      {
        WriteStartAttributeImpl(name.Utf8Bytes);
      }
      else
      {
        Span<byte> prefixBytes = stackalloc byte[Encoding.UTF8.GetByteCount(prefix)];
        int prefixCount = Encoding.UTF8.GetBytes(prefix.AsSpan(), prefixBytes);
        Debug.Assert(prefixCount == prefixBytes.Length);

        WriteStartAttributeImpl(prefixBytes, name.Utf8Bytes);
      }
      this.writingAttribute = true;
    }

    public void WriteStartAttribute(string? prefix, string name)
    {
      Span<byte> nameBytes = stackalloc byte[Encoding.UTF8.GetByteCount(name)];
      int count = Encoding.UTF8.GetBytes(name.AsSpan(), nameBytes);
      Debug.Assert(count == nameBytes.Length);

      if (prefix is null)
      {
        WriteStartAttributeImpl(nameBytes);
      }
      else
      {
        Span<byte> prefixBytes = stackalloc byte[Encoding.UTF8.GetByteCount(prefix)];
        int prefixCount = Encoding.UTF8.GetBytes(prefix.AsSpan(), prefixBytes);
        Debug.Assert(prefixCount == prefixBytes.Length);

        WriteStartAttributeImpl(prefixBytes, nameBytes);
      }
      this.writingAttribute = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteStartAttributeImpl(ReadOnlySpan<byte> prefix, ReadOnlySpan<byte> name)
    {
      this.stream.WriteByte((byte)' ');
      this.stream.Write(prefix);
      this.stream.WriteByte((byte)':');
      this.stream.Write(name);
      this.stream.WriteByte((byte)'=');
      this.stream.WriteByte((byte)'"');
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
