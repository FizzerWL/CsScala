package System.Xml.Linq;

import org.jdom2._

abstract class XNode(node: Content) extends XObject {
  val _node = node;

  def ReplaceWith(node: XNode) {
    val parent = _node.getParent();
    val index = parent.indexOf(_node);
    parent.removeContent(_node);
    parent.addContent(index, node._node);
  }
  
  def GetNode():Content = {
    if (this.isInstanceOf[XDocument])
      return this.asInstanceOf[XDocument].Root._node;
    else
      return _node;
  }
}