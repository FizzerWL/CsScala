package System.Net;
import System.IO.Stream
import System.NotImplementedException
import java.net.URLConnection

class WebResponse(_req:URLConnection)
{
	
	val ContentLength:Int = _req.getContentLength();
	def GetResponseStream():Stream =
	{
		return new Stream(_req.getInputStream(), null, null, null)
	}
	
	def Dispose()
	{
		
	}
}