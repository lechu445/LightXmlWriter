# LightXmlWriter
This is a light implementation of XmlWriter equivalent designed to be as close as possible of XmlWriter usage &amp; behaviour with most common settings (no pretty-print, no xml declaration, etc.)

## LightXmlWriter vs XmlWriter

### Similarities
* Method names are kept the same, and they should work the same way

For example
```cs
writer.WriteStartElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
```
means:
```xml
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/">
```

* both are not thread-safe

Both readers keep state, so they must not be processed in parallel. LightXmlWriter does not do any validation, so it could produce an unexpeced output.

* both implement these methods:
    - WriteStartElement
    - WriteEndElement
    - WriteElementString
    - WriteStartAttribute
    - WriteEndAttribute
    - WriteAttributeString
    - WriteRaw
    - WriteValue

* both implement IDisposable intefrace

### Differences

* LightXmlWriter contains overloads of methods 
The overload accepts int, double, bool, ReadOnlySpan<char> (only in .NET Core), char[] etc.
You can even use overload that uses Action on TextWriter, so you can have all control!

* WriteEndElement must have name of the element

XmlWriter:
```cs
writer.WriteStartElement("Person");
writer.WriteEndElement();
```
LightXmlWriter:
```cs
writer.WriteStartElement("Person");
writer.WriteEndElement("Person");
```

* LightXmlWriter does not use XmlWriterSettings

It only uses its default settings, that's why it is light :)
What are the default settings?
It does not do any validation at run-time, no pretty-print of output, no XML declaration. So it is just simple writer.

* LightXmlWriter has better performance than XmlWriter

It's hard to measure difference, because LightXmlWriter can be used in more efficient way (more overloads of methods).
It seems, in similar scenario LightXmlWriter is at least 33% faster and 5x less allocations than XmlWriter.

## Contributing

You are welcome to help with this package. There are also a lot to do: write more benchmark tests, more tests, more overloads, finding bugs, making optimisations of existing code.

## Acknowledgments

Please use it carefully. LightXmlWriter can produce an invalid XML, so write tests for each output.
