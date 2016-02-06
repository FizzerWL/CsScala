package System.Net;

import System.IO.Stream;
import System.NotImplementedException;
import java.net._;
import java.io._;
import scala.collection.JavaConversions._;

class HttpWebResponse(_req:HttpURLConnection, cookies:CookieContainer, _isError:Boolean) extends WebResponse(_req, _isError)
{

	val StatusCode:Int = _req.getResponseCode();
	val Cookies:CookieContainer = cookies;
	
	
	if (cookies != null)
	{
	  val s = _req.getHeaderFields().get("Set-Cookie");
	  if (s != null)
		  for(cookie <- s)
			  cookies.AddRaw(cookie);
	}
}