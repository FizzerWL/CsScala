package System.Xml.Linq

import org.jdom2._
import org.jdom2.input._
import java.io.StringReader
import System.Xml.XmlNodeType
import sun.reflect.generics.reflectiveObjects.NotImplementedException
import org.jdom2.output.XMLOutputter

object XDocument {

  
  private final val _builder = new ThreadLocal[SAXBuilder]()
  { override def initialValue():SAXBuilder = new SAXBuilder(); };
  
  
  val Outputter = new XMLOutputter();

  def Parse(str: String): XDocument = new XDocument(_builder.get().build(new StringReader(str)));
  def Load(str: String): XDocument = new XDocument(_builder.get().build(str));

  def Factory(content: Content): XNode =
    {
      return content match {
        case e: Comment => new XComment(e)
        case e: Element => new XElement(e)
        case e: Text => new XText(e)
      };
    }
}

class XDocument(_doc: Document) extends XContainer(null) {
  def this() {
    this(new Document());
  }

  final val Root: XElement = new XElement(_doc.getRootElement());

  def NodeType: Int = XmlNodeType.Document;

  override def Elements(): Traversable[XElement] =
    {
      return Array(Root);
    }
  override def Elements(name: String): Traversable[XElement] =
    {
      return Array(Element(name));
    }
  override def Attributes(): Traversable[XAttribute] =
    {
      return Array[XAttribute]();
    }
  override def Element(name: String): XElement =
    {
      if (Root.Name.LocalName == name)
        return Root;
      else
        return null;
    }

  override def DescendantNodes(): Traversable[XNode] =
    {
      return Array[XNode](Root) ++ Root.DescendantNodes();
    }
  override def toString(): String = XDocument.Outputter.outputString(_doc);
}