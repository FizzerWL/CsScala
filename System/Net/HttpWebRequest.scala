package System.Net

import System.NotImplementedException
import System.Uri
import System.IO.Stream
import java.net._;
import java.io._;

object HttpWebRequest
{
  def Create(url:String):WebRequest = Create(new Uri(url));
  def Create(url:Uri):WebRequest = new HttpWebRequest(url._url.openConnection().asInstanceOf[HttpURLConnection])
}
class HttpWebRequest(_req:HttpURLConnection) extends WebRequest 
{

  
  def Method:String = _req.getRequestMethod();
  def Method_=(m:String) = _req.setRequestMethod(m);
  
  def ContentType:String = _req.getRequestProperty("Content-Type");
  def ContentType_=(c:String) = _req.setRequestProperty("Content-Type", c);
  
  def ContentLength:Int = _req.getRequestProperty("Content-Length").toInt;
  def ContentLength_=(c:Int) = _req.setRequestProperty("Content-Length", c.toString);

  def Timeout:Int = _req.getConnectTimeout();
  def Timeout_=(t:Int) = _req.setConnectTimeout(t);
  
  var CookieContainer:CookieContainer = null;
  
  def GetResponse():WebResponse = new HttpWebResponse(_req, CookieContainer, false);
  def GetRequestStream(): Stream = 
    {
      _req.setDoOutput(true);
      return new Stream(null, _req.getOutputStream(), null, null);
    }
}