package System.Xml.Linq;
import org.jdom2._
import System.NotImplementedException
import scala.collection.JavaConverters._

abstract class XContainer(elem: Element) extends XNode(elem) {
  val _elem = elem; //null if we're an XDocument, which overrides our methods so these should never be called.

  def Add(obj: Any) {
    if (obj.isInstanceOf[XNode])
      _elem.addContent(obj.asInstanceOf[XNode]._node);
    else if (obj.isInstanceOf[XAttribute])
      _elem.setAttribute(obj.asInstanceOf[XAttribute]._attr)
    else
      throw new NotImplementedException("Adding unexpected type");
  }

  def Elements(): Traversable[XElement] = _elem.getChildren().asScala.map(new XElement(_));
  def Elements(name: String): Traversable[XElement] = _elem.getChildren(name).asScala.map(new XElement(_));
  def Attributes(): Traversable[XAttribute] = _elem.getAttributes().asScala.map(new XAttribute(_));
  
  def Element(name: String): XElement = {
    val child = _elem.getChild(name);
    if (child == null)
      return null;
    else
      return new XElement(child);
  }

  def DescendantNodes(): Traversable[XNode] = {
    return _elem.getContent().asScala.map(XDocument.Factory(_));
  }
}