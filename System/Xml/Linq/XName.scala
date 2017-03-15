package System.Xml.Linq;


class XName(str:String)
{

	val LocalName:String = str;
	

	override def toString():String =
	{
		return LocalName;
	}
}