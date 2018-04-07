package System.Xml.Linq

import org.jdom2._
import System.Xml.XmlNodeType

class XAttribute(attr: Attribute) extends XObject {

  val _attr = attr;
  
  def this(name:String, value:Any) {
    this(new Attribute(name, value.toString()));
  }
  def this(name:XName, value:Any) {
    this(new Attribute(name.LocalName, value.toString(), Namespace.getNamespace("p", name.NamespaceName))); //TODO: Instead of always using the same prefix "p", find a way to come up with a unique prefix.
  }
  
  def Value: String = _attr.getValue();
  def Value_=(value: String) = _attr.setValue(value);

  def Name: XName = new XName(_attr.getName());
  def Name_=(name: XName) = _attr.setName(name.LocalName);

  def Remove() {
    _attr.getParent().removeAttribute(_attr);
  }

  def NodeType: Int = XmlNodeType.Attribute;
}