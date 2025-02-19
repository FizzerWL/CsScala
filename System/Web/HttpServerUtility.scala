package System.Web

import java.lang.Exception

class HttpServerUtility {

  var LastError:Exception = null;
  
  def GetLastError():Exception = LastError;
  def ClearError():Unit = { LastError = null; }
}