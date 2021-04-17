using System;
using System.IO;

#if NETSTANDARD1_3
using System.Buffers;
#endif

namespace XmlTools
{
  /// <summary>
  /// Light implementation of XmlWriter equivalent designed to be as close as
  /// possible of XmlWriter usage &amp; behaviour with most common settings (no pretty-print, no xml declaration, etc.)
  /// </summary>
  public sealed partial class LightXmlWriter : IDisposable
  {
    private readonly TextWriter writer;
    private bool writingElement = false;
    private bool writingAttribute = false;
    private bool valueWritten = false;
#if NETSTANDARD1_3
    private char[] buffer;
#endif

#if NETSTANDARD1_3
    /// <summary>Initializes a new instance of the <see cref="LightXmlWriter"/> class.</summary>
    /// <param name="writer">TextWriter which LightXmlWriter directly writes to.</param>
    /// <param name="bufferSize">Size of internal buffer used for single operation on string.</param>
    /// <exception cref="ArgumentNullException">writer property is null.</exception>
    public LightXmlWriter(TextWriter writer, int bufferSize = 1024)
    {
      this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
      this.buffer = ArrayPool<char>.Shared.Rent(bufferSize);
    }
#else
    /// <summary>Initializes a new instance of the <see cref="LightXmlWriter"/> class.</summary>
    /// <param name="writer">TextWriter which LightXmlWriter directly writes to.</param>
    /// <exception cref="ArgumentNullException">writer property is null.</exception>
    public LightXmlWriter(TextWriter writer)
    {
      this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
    }
#endif

    /// <summary>Gets TextWriter which LightXmlWriter directly writes to.</summary>
    public TextWriter Writer => this.writer;

    public void Dispose()
    {
#if NETSTANDARD1_3
      if (this.buffer != null)
      {
        ArrayPool<char>.Shared.Return(this.buffer);
        this.buffer = null!;
      }
#endif
      this.writer.Dispose();
    }

    public void WriteRaw(string value)
    {
      WriteValue(value, escape: false);
    }

    public void WriteString(string? value, bool escape = true)
    {
      WriteValue(value, escape);
    }
  }
}
