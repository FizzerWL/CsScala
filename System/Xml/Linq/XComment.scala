package System.Xml.Linq;

import org.jdom2._
import System.Xml.XmlNodeType

class XComment(node:Comment) extends XNode(node)
{

  def NodeType:Int = XmlNodeType.Comment; 
}