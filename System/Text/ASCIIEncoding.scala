package System.Text;

import java.nio.charset.Charset

class ASCIIEncoding extends Encoding
{
  def GetBytes(str:String):Array[Byte] = str.getBytes(Charset.forName("US-ASCII"));
  def GetString(b:Array[Byte]):String = new String(b, "US-ASCII");
}