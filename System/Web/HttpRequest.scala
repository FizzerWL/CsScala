package System.Web;
import System.Collections.Specialized.NameValueCollection
import System.NotImplementedException
import System.Uri
import System.CsScala
import System.IO.Stream
import java.net.URI

class HttpRequest
{

	var UrlReferrer:Uri = null;
	var UserHostAddress:String = null;
	var UserAgent:String = null;
	var RawUrl:String = null;
	var HttpMethod = "GET";
	final val Cookies = new HttpCookieCollection();
	final val Headers = new NameValueCollection();
	final val QueryString = new NameValueCollection(true); 
	final val Files = new HttpFileCollection();
	
	var _fullUrl:String = null;
	private var _url:Uri = null;
	def Url:Uri = {
	  if (_url == null)
	    _url = new Uri(_fullUrl);
	  return _url;
	}
	
	var _form:NameValueCollection = null;
	var GetForm:()=>NameValueCollection = null;
	def Form:NameValueCollection = {
	  if (_form == null)
	    _form = GetForm();
	  
	  return _form;
	}
	
	
	var GetInputStream:()=>Stream = null;
	var _inputStream:Stream = null;
	def InputStream:Stream = {
	  if (_inputStream == null)
	    _inputStream = GetInputStream();

	  return _inputStream;
	}
	
	
	def Params(str:String):String = CsScala.Coalesce(Form(str), QueryString(str));
}