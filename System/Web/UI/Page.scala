package System.Web.UI

import System.Web.HttpRequest
import System.Web.HttpResponse
import System.EventArgs
import System.Web.HttpServerUtility
import System.Web.HttpContext

abstract class Page extends Control {

  def Request = Context.Request;
  def Response = Context.Response;
  final val Server = new HttpServerUtility();

  def OnError(args: EventArgs) {

  }

}