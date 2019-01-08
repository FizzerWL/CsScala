package System.Net;

object HttpStatusCode
{

	final val NotModified:Int = 304;
	final val Forbidden = 403;
	final val NotFound:Int = 404;
	
	def ToString(code:Int):String = code.toString();
}