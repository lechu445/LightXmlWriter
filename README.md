# LightXmlWriter
The LightXmlWriter is a replacement of XmlWriter which can be useful in high-performance API implementation. Therefore there are several goals to reach:
- high performance
    - zero allocation
    - faster work than XmlWriter
    - no runtime validation
    - keep it simple (no configuration)
    - allow write non-string values without creation to intermediate string
    - allow disable escaping values in similar way to Newtonsoft.Json
    - produce as small as possible output XML
- easy migration from XmlWriter

## LightXmlWriter vs XmlWriter

### Benchmarks

EnterpriseLightXmlWriterBenchmarks.LightXmlWriter_Write_Xml
```
|                   Method |      Mean |     Error |    StdDev |    Median | Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------- |----------:|----------:|----------:|----------:|------:|------:|------:|----------:|
| LightXmlWriter_Write_Xml |  8.585 us | 0.1733 us | 0.2429 us |  8.600 us |     - |     - |     - |         - |
|      XmlWriter_Write_Xml | 17.923 us | 0.4765 us | 1.3671 us | 18.300 us |     - |     - |     - |    1288 B |
```

### Similarities
* Method names are kept the same, and they should work the same way

For example for both writers:
```cs
writer.WriteStartElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
```
produces:
```xml
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/">
```

* both are not thread-safe

Both writers keep state, so they must not be processed in parallel. LightXmlWriter does not do any validation, so it could produce an unexpeced output.

* both implement these methods:
    - WriteStartElement
    - WriteEndElement
    - WriteElementString
    - WriteStartAttribute
    - WriteEndAttribute
    - WriteAttributeString
    - WriteRaw
    - WriteValue

* both implement IDisposable interface

### Differences

* LightXmlWriter contains more overloads of methods 

The overload accepts int, double, bool, ReadOnlySpan&lt;char&gt; (only in .NET Core), char[] etc.
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

LightXmlWriter is about 5x faster and generates less allocations than XmlWriter because of methods overloads that don't need conversion to string before.

* LightXmlWriter produces more compreesed self-closed xml tags

The compressed version does not contain space before closing singn. It is still valid XML.

XmlWriter:
```cs
writer.WriteStartElement("Person");
writer.WriteEndElement();
//produces <Person />
```
LightXmlWriter:
```cs
writer.WriteStartElement("Person");
writer.WriteEndElement("Person");
//produces <Person/>
```

* LightXmlWriter does not validate & escape tag and attribute names

XmlReader throws an exception in case of unescaped name

* LightXmlWriter allows write values without escaping

This is similar to Newtonsoft JsonWriter where you can set flag `escape: false`. It brings better performance than with enabled escape. Use it where you are sure that value does not need escaping.

Example:
```cs
writer.WriteStartElement("Code");
writer.WriteValue("ABCD", escape: false);
writer.WriteEndElement();
//produces <Code>ABCD</Code>

writer.WriteElementString("Code", "ABCD", escapeValue: false);
//produces <Code>ABCD</Code>
```

## Migration to LightXmlWriter

1. To each `WriteEndElement();` add argument with name of closing element, e.g. `WriteEndElement("Person");`
2. Optionally to `WriteEndAttribute();` add argument with name of closing attribute, e.g. `WriteEndAttribute("Age");`
3. Optionally use more adequate method overload to reduce string allocations, e.g. `WriteAttributeString("Age", "25");` to `WriteAttributeString("Age", 25);`
4. Optionally disable value escaping where it is not needed, e.g. `WriteAttributeString("Code", "ABC123");` to `WriteAttributeString("Code", "ABC123", escapeValue: false);`
5. If you see any difference in output, report it [here](https://github.com/lechu445/LightXmlWriter/issues)

## Contributing

You are welcome to help with this package. There are also a lot to do: write more benchmark tests, more tests, more overloads, finding bugs, making optimisations of existing code.

## Acknowledgments

Please use it carefully. LightXmlWriter can produce an invalid XML, so write tests for each output.


## Requirements

Framework compatible with .NET Standard 1.3 (.NET Core 1.0, .NET Framework 4.6, Mono 4.6) or higher.  
Currently ReadOnlySpan&lt;char&gt; overloads are only available in .NET Core 2.1 and 2.2 builds.
