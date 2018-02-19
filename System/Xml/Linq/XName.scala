package System.Xml.Linq;

object XName {
  def Get(localName: String, namespaceName: String): XName = return new XName(localName, namespaceName);
}

class XName(localName: String, ns:String = null) {

  val LocalName = localName;
  val NamespaceName = ns;

  override def toString(): String = LocalName;
}