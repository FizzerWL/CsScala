package System.Xml.XPath;

import System.Xml.Linq.XElement
import System.Xml.Linq.XNode
import org.jdom2.xpath._
import org.jdom2.filter.Filters
import System.Xml.XmlException
import scala.collection.JavaConverters._

object Extensions
{

	
	def XPathSelectElement(node:XNode, expression:String):XElement =
	{
		val xpfac = XPathFactory.instance();
		val xp = xpfac.compile(expression, Filters.element());
		val it = xp.evaluate(node.GetNode()).iterator();
		
		if (!it.hasNext())
		  return null;
		
		return new XElement(it.next());
	}
	def XPathSelectElements(node:XNode, expression:String):Iterable[XElement] =
	{
		val xpfac = XPathFactory.instance();
		val xp = xpfac.compile(expression, Filters.element());
		return xp.evaluate(node.GetNode()).asScala.map(new XElement(_));
	}
	
	
	
}