package System.Xml;

import System.Text.StringBuilder
import System.NotImplementedException
import javax.xml.stream.XMLOutputFactory
import java.io.StringWriter

object XmlWriter {
  def Create(sb: StringBuilder, settings: XmlWriterSettings): XmlWriter =
    {
      val buf = new StringWriter();
      sb.SetStringWriter(buf);
      return new XmlWriter(buf);
    }

}

class XmlWriter(buf: StringWriter) {
  final val _writer = XMLOutputFactory.newInstance().createXMLStreamWriter(buf);

  def Flush() {
  }

  def WriteAttributeString(localName: String, value: String) {
    _writer.writeAttribute(localName, value)
  }

  def WriteStartElement(name: String) {
    _writer.writeStartElement(name)
  }

  def WriteEndElement() {
    _writer.writeEndElement();
  }

  def WriteString(text: String) {
    _writer.writeCharacters(text);
  }

}