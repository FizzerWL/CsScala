package System.Xml.Linq;
import org.jdom2._
import System.NotImplementedException
import scala.collection.JavaConverters._
import java.util.ArrayList
import scala.collection.mutable.ArrayBuffer

abstract class XContainer(elem: Element) extends XNode(elem) {
  val _elem = elem; //null if we're an XDocument, which overrides our methods so these should never be called.

  def Add(obj: Any) {
    if (obj == null)
      return ;

    if (obj.isInstanceOf[XNode]) {
      val toAdd = obj.asInstanceOf[XNode]._node;

      if (toAdd.getParent() == null)
        _elem.addContent(toAdd);
      else
        _elem.addContent(toAdd.clone()); //if it has a parent already, we must clone since jdom2 won't let us add the same element to two parents, but .net does.
    } else if (obj.isInstanceOf[XAttribute])
      _elem.setAttribute(obj.asInstanceOf[XAttribute]._attr);
    else if (obj.isInstanceOf[Traversable[Any]]) {
      for (c <- obj.asInstanceOf[Traversable[Any]])
        Add(c);
    } else if (obj.isInstanceOf[String])
      _elem.setText(obj.asInstanceOf[String]);
    else
      throw new NotImplementedException("Adding unexpected type " + obj.getClass().getName());
  }
  def AddFirst(obj: Any) {
    if (obj.isInstanceOf[XNode]) {
      val toAdd = obj.asInstanceOf[XNode]._node;

      if (toAdd.getParent() == null)
        _elem.addContent(0, toAdd);
      else
        _elem.addContent(0, toAdd.clone()); //if it has a parent already, we must clone since jdom2 won't let us add the same element to two parents, but .net does.
    }
    else
      Add(obj);
  }
  def Elements(): Traversable[XElement] = _elem.getChildren().asScala.map(new XElement(_));
  def Elements(name: String): Traversable[XElement] = _elem.getChildren(name).asScala.map(new XElement(_));

  def Descendants(): Traversable[XElement] = {
    val ret = new ArrayBuffer[XElement]();
    for (e <- Elements()) {
      ret += e;
      for (e2 <- e.Descendants())
        ret += e2;
    }
    return ret;
  }

  def Attributes(): Traversable[XAttribute] = _elem.getAttributes().asScala.map(new XAttribute(_));

  def Element(name: String): XElement = {
    val child = _elem.getChild(name);
    if (child == null)
      return null;
    else
      return new XElement(child);
  }

  def Nodes(): Traversable[XNode] = _elem.getContent().asScala.map(XDocument.Factory(_));

  def DescendantNodes(): Traversable[XNode] = {
    val ret = new ArrayBuffer[XNode]();
    val it = _elem.getDescendants();
    while (it.hasNext())
      ret += XDocument.Factory(it.next());
    return ret;
  }
}