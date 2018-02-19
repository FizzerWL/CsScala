package System.Xml.Linq

import org.jdom2._
import System.Xml.XmlNodeType
import System.NotImplementedException
import org.jdom2.output.XMLOutputter

class XElement(elem: Element) extends XContainer(elem) {
  if (elem == null)
    throw new Exception("null element");

  def this(name: String, children: Any*) {
    this(new Element(name));
    for (c <- children) {
      if (c.isInstanceOf[String])
        this.Value = c.asInstanceOf[String];
      else
        this.Add(c);
    }
  }
  
  def this(name: XName, children: Any*) {
    this(new Element(name.LocalName, name.NamespaceName));
    for (c <- children) {
      if (c.isInstanceOf[String])
        this.Value = c.asInstanceOf[String];
      else
        this.Add(c);
    }
  }
  def Attribute(name: String): XAttribute = {

    val a = _elem.getAttribute(name);
    if (a == null)
      return null;
    else
      return new XAttribute(a);
  }

  def SetAttributeValue(name: String, value: String) {
    _elem.setAttribute(name, value);
  }

  def Value: String = _elem.getText();
  def Value_=(str: String) = _elem.setText(str);

  def Name: XName = new XName(_elem.getName());
  def Name_=(value:XName) {
    _elem.setName(value.LocalName);
    if (value.NamespaceName != null)
      _elem.setNamespace(Namespace.getNamespace(value.NamespaceName));
  }

  def NodeType: Int = XmlNodeType.Element;

  def GetDefaultNamespace(): XNamespace = {
    throw new NotImplementedException("stub");
  }
  override def toString(): String = XDocument.Outputter.outputString(elem);

}