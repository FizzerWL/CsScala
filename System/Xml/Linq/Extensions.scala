package System.Xml.Linq;

object Extensions
{
	
	def Remove[T <: XNode](a:Iterable[T]) =
	{
		for(e <- a)
		{
		  e._node.getParent().removeContent(e._node);
		}
	}
	
}