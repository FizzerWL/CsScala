package System.Xml.Linq;
import org.jdom2._
import System.NotImplementedException
import scala.collection.JavaConverters._


abstract class XContainer(elem:Element) extends XNode(elem)
{
  val _elem = elem; //null if we're an XDocument
  
  def Add(obj:Any)
  {
    if (obj.isInstanceOf[XNode])
      _elem.addContent(obj.asInstanceOf[XNode]._node);
    else
      throw new NotImplementedException("Adding unexpected type");
  }
  
  def Elements():Traversable[XElement] =
  {
    return _elem.getChildren().asScala.map(new XElement(_));
  }
  def Elements(name:String):Traversable[XElement] =
  {
    return _elem.getChildren(name).asScala.map(new XElement(_));
  }
  def Attributes():Traversable[XAttribute] =
  {
    return _elem.getAttributes().asScala.map(new XAttribute(_));
  }
  def Element(name:String):XElement =
  {
    return new XElement(_elem.getChild(name));
  }

  def DescendantNodes():Traversable[XNode] =
  {
    return _elem.getContent().asScala.map(XDocument.Factory(_));
  }
}