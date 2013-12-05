package System.Text

import System.NotImplementedException
import java.nio.charset.Charset

object UTF8Encoding
{
  def UTF8:UTF8Encoding = 
  {
    return new UTF8Encoding();
  }
}

class UTF8Encoding 
{
  def GetBytes(str:String):Array[Byte] =
  {
    return str.getBytes(Charset.forName("UTF-8"));
  }
  
  def GetString(b:Array[Byte]):String =
  {
    return new String(b, "UTF-8");
  }
}