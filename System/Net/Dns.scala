package System.Net;

import java.net.InetAddress

object Dns
{


	def GetHostName():String = 
	{
		return InetAddress.getLocalHost().getHostName();
	}
}