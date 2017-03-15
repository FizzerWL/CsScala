package System.Net;

class WebException(msg:String = null, inner:Exception = null, resp:WebResponse = null) extends Exception(msg, inner)
{

	final val Response:WebResponse = resp;
	
	def Message:String = getMessage();
}