package System.Web.UI

import System.NotImplementedException
import System.EventArgs
import System.Web.HttpContext

abstract class Control {
  
  var Page:Page = null;
  var ID = "";
  var Visible = true;

  def RenderControl(writer:HtmlTextWriter)
  {
    throw new Exception("RenderControl not overridden by " + this);
  }
  def Render(writer:HtmlTextWriter)
  {
    if (Visible)
    {
      OnPreRender(null);
      RenderControl(writer);
    }
  }
  def OnInit(args:EventArgs) { }
  def OnLoad(args:EventArgs) { }
  def OnPreRender(args:EventArgs) { }
  
  def GetControls():Array[Control];
  
  def RecurseAll(fn:Control=>Unit)
  {
    fn(this);
    
    for(c <- GetControls())
      c.RecurseAll(fn);
  }
}