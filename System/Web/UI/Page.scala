package System.Web.UI

import System.Web.HttpRequest
import System.Web.HttpResponse
import System.EventArgs
import System.Web.HttpServerUtility
import System.Web.HttpContext


abstract class Page extends Control {

  private val ctx = HttpContext.Current;
  final val Request = ctx.Request;
  final val Response = ctx.Response;
  final val Server = new HttpServerUtility();
  
  
  def OnError(args:EventArgs)
  {
    
  }
  
}