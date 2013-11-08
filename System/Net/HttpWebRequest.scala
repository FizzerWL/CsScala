package System.Net

import System.NotImplementedException

object HttpWebRequest
{
  def Create(url:String):WebRequest =
  {
    throw new NotImplementedException();
  }
}
class HttpWebRequest extends WebRequest 
{

}