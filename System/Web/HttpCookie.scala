package System.Web;
import System.DateTime;
import System.NotImplementedException;

class HttpCookie(name: String, value: String = null) 
{

  var Name = name;
  var Value = value;
  var Expires:DateTime = null;

}