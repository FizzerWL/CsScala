package System.Text;

import java.nio.charset.Charset

class ASCIIEncoding extends Encoding
{

	
	def GetBytes(str:String):Array[Byte] =
	{
		return str.getBytes(Charset.forName("US-ASCII"));
	}
	
}