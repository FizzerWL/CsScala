package System.Web;
import System.DateTime;

class HttpCookie(name: String, value: String = null) 
{

  var Name = name;
  var Value = value;
  var Expires:DateTime = null;
  var HttpOnly = false;
  var Secure = false;

}