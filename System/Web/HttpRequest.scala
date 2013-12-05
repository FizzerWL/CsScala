package System.Web;
import System.Collections.Specialized.NameValueCollection;
import System.NotImplementedException;
import System.Uri;

class HttpRequest
{

	var UrlReferrer:Uri = null;
	var Cookies:HttpCookieCollection = new HttpCookieCollection();
	var Headers:NameValueCollection = new NameValueCollection();
	var UserHostAddress:String = "TODO";
	var UserAgent:String = "TODO";
}