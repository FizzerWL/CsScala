package System.Xml.Linq


import org.jdom2._
import System.Xml.XmlNodeType

class XAttribute(_attr:Attribute) extends XObject
{

  def Value:String = _attr.getValue();
  def Value_=(value:String) = _attr.setValue(value);
  
  
  def Name:XName = new XName(_attr.getName());
  def Name_=(name:XName) = _attr.setName(name.LocalName); 
  
  def Remove()
  {
    _attr.getParent().removeAttribute(_attr);
  }
  
  def NodeType:Int = XmlNodeType.Attribute;
}