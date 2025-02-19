package System.Xml;

import System.Text.StringBuilder
import javax.xml.stream.XMLOutputFactory
import java.io.StringWriter
import org.jdom2.Document
import org.jdom2.Element
import org.jdom2.output.XMLOutputter
import org.jdom2.output.Format

object XmlWriter {
  def Create(sb: StringBuilder, settings: XmlWriterSettings): XmlWriter =
    {
      val buf = new StringWriter();
      sb.SetStringWriter(buf);
      return new XmlWriter(buf);
    }

}

//This class avoids using XMLStreamWriter since it doesn't seem to encode newlines within attribute values 

class XmlWriter(buf: StringWriter) {
  final val _doc = new Document();
  final var _currElement:Element = null;

  final var _flushed = false;
  def Flush():Unit = {
    if (_flushed)
      throw new Exception("Cannot flush twice");
    buf.write(new XMLOutputter(Format.getCompactFormat().setOmitDeclaration(true)).outputString(_doc))
    _flushed = true;
  }

  def WriteAttributeString(localName: String, value: String):Unit = {
    _currElement.setAttribute(localName, value);
  }

  def WriteStartElement(name: String):Unit = {
    val p = _currElement;
    _currElement = new Element(name);
    (if (p == null) _doc else p).addContent(_currElement);
  }

  def WriteEndElement():Unit = {
    val parent = _currElement.getParent();
    if (parent == null || parent == _doc)
      _currElement = null;
    else
      _currElement = parent.asInstanceOf[Element];
  }

  def WriteString(text: String):Unit = {
    _currElement.setText(text);
  }

}