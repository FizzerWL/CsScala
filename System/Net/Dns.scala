package System.Net;
import System.NotImplementedException;
import java.net.InetAddress

object Dns
{


	def GetHostName():String = 
	{
		return InetAddress.getLocalHost().getHostName();
	}
}