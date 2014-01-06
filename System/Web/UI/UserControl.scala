package System.Web.UI
import System.Web.HttpRequest
import System.Web.HttpResponse
import System.Web.HttpContext

abstract class UserControl extends Control {
  
  private val ctx = HttpContext.Current;
  
  final val Request = ctx.Request;
  final val Response = ctx.Response;
  
}