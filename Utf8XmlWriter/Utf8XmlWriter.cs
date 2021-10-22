using System;
using System.IO;
using System.Threading.Tasks;

[module: System.Runtime.CompilerServices.SkipLocalsInit]
namespace Tools.Text.Xml
{
  /// <summary>
  /// Light implementation of XmlWriter equivalent designed to be as close as
  /// possible of XmlWriter usage &amp; behaviour with most common settings (no pretty-print, no xml declaration, etc.)
  /// </summary>
  public sealed partial class Utf8XmlWriter : IDisposable, IAsyncDisposable
  {
    private readonly Stream stream;
    private bool writingElement = false;
    private bool writingAttribute = false;
    private bool valueWritten = false;

    /// <summary>Initializes a new instance of the <see cref="Utf8XmlWriter"/> class.</summary>
    /// <param name="stream">Stream which <see cref="Utf8XmlWriter"/> directly writes to. Can be forward-only.</param>
    /// <exception cref="ArgumentNullException">writer property is <see langword="null"/>.</exception>
    public Utf8XmlWriter(Stream stream)
    {
      this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
    }

    /// <summary>Gets <see cref="Stream"/> which <see cref="Utf8XmlWriter"/> directly writes to.</summary>
    public Stream Writer => this.stream;

    public void Dispose()
    {
      this.stream.Flush();
    }

    public ValueTask DisposeAsync()
    {
      return new ValueTask(this.stream.FlushAsync());
    }

    public void Flush() => this.stream.Flush();

    public Task FlushAsync() => this.stream.FlushAsync();
  }
}
