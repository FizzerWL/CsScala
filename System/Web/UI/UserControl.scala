package System.Web.UI
import System.Web.HttpRequest
import System.Web.HttpResponse
import System.Web.HttpContext

abstract class UserControl extends Control {

  def Request = Context.Request;
  def Response = Context.Response;

}