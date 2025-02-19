package System.Web;

import System.DateTime
import System.Web.UI.HtmlTextWriter

class RedirectException(url:String, httpCode:Int = 302) extends java.lang.Exception
{
  final val Url = url;
  final val HttpCode = httpCode;
}

class ResponseEndException extends java.lang.Exception { }

class HttpResponse {

  final val Cookies = new HttpCookieCollection();
  var StatusCode: Int = 200;
  var ContentType = "text/html; charset=utf-8";

  var _writer:HtmlTextWriter = null;
  def Write(s: String):Unit = {
    _writer.Write(s);
  }

  def End():Unit = {
    throw new ResponseEndException();
  }

  def Redirect(url: String):Unit = {
    throw new RedirectException(url);
  }
}