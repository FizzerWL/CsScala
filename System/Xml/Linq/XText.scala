package System.Xml.Linq;

import org.jdom2._
import System.Xml.XmlNodeType

class XText(node:Text) extends XNode(node)
{

  def NodeType:Int = XmlNodeType.Text; 
}