package System.Xml.Linq

import org.jdom2._
import org.jdom2.input._
import java.io.StringReader
import System.Xml.XmlNodeType
import org.jdom2.output.XMLOutputter
import System.Xml.XmlException
import System.NotImplementedException

object XDocument {

  private final val _builder = new ThreadLocal[SAXBuilder]() { override def initialValue(): SAXBuilder = new SAXBuilder(); };

  val Outputter = new XMLOutputter();

  def Parse(str: String): XDocument = {
    try {
      return new XDocument(_builder.get().build(new StringReader(str)));
    } catch {
      case ex: org.jdom2.input.JDOMParseException => throw new XmlException(ex.getMessage(), ex);
    }
  }
  def Load(str: String): XDocument = new XDocument(_builder.get().build(str));

  def Factory(content: Content): XNode =
    {
      return content match {
        case e: Comment => new XComment(e)
        case e: Element => new XElement(e)
        case e: Text    => new XText(e)
      };
    }
}

class XDocument(_doc: Document) extends XContainer(null) {
  def this() {
    this(new Document());
  }
  def this(root: XElement) {
    this(new Document(root._elem));
  }

  def Root: XElement = new XElement(_doc.getRootElement());

  def NodeType: Int = XmlNodeType.Document;

  override def Elements(): Traversable[XElement] = Array(Root);
  override def Elements(name: String): Traversable[XElement] = Array(Element(name));
  override def Attributes(): Traversable[XAttribute] = Array[XAttribute]();
  override def Element(name: String): XElement =
    {
      if (Root.Name.LocalName == name)
        return Root;
      else
        return null;
    }

  override def Add(obj: Any) {
    if (obj.isInstanceOf[XNode])
      _doc.addContent(obj.asInstanceOf[XNode]._node);
    else
      throw new NotImplementedException("Adding unexpected type");

  }

  override def DescendantNodes(): Traversable[XNode] = Array[XNode](Root) ++ Root.DescendantNodes();
  
  override def toString(): String = XDocument.Outputter.outputString(_doc);
  def toString(opts:Int): String = toString(); //TODO: Support formatting option
  
  def Save(path: String) {
    System.IO.File.WriteAllText(path, toString());
  }
}