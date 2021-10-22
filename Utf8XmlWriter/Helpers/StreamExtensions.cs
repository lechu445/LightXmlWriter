using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace XmlTools.Helpers
{
  internal static class StreamExtensions
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void WriteUtf8EncodedString(this Stream stream, ReadOnlySpan<char> value)
    {
      Span<byte> prefixBuffer = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(value)];
      System.Text.Encoding.UTF8.GetBytes(value, prefixBuffer);
      stream.Write(prefixBuffer);
    }
  }
}
