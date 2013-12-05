package System.Xml.Linq


import org.jdom2._
import org.jdom2.input._
import java.io.StringReader
import System.Xml.XmlNodeType

object XDocument
{
  def Parse(str:String):XDocument = 
  {
    return new XDocument(new SAXBuilder().build(new StringReader(str)));
  }
  def Load(str:String):XDocument = 
  {
    return new XDocument(new SAXBuilder().build(str));
  }
  
  def Factory(content:Content):XNode =
  {
    return content match
    {
    case e: Comment => new XComment(e)
    case e: Element => new XElement(e)
    //case e: Attribute => new XAttribute(e)
    };
  }
}

class XDocument(_doc:Document) extends XContainer(null)
{
  def this()
  {
    this(new Document());
  }
  
  final val Root:XElement = new XElement(_doc.getRootElement());  
  
  def NodeType:Int = XmlNodeType.Document;
}