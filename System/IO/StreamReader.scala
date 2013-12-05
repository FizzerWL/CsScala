package System.IO;
import System.NotImplementedException;

class StreamReader(s:Stream)
{

  val _bufReader = new java.io.BufferedReader(s._textinput);
	
	def ReadToEnd():String =
	{
	  var sb = new StringBuilder();
	  var line:String = null;
	  do
	  {
	    line = _bufReader.readLine();
	    if (line != null)
	    {
	      sb.append(line);
	      sb.append("\n");
	    }
	  }
	  while (line != null);
	  return sb.toString();
	}
	
	def ReadLine():String =
	{
		return _bufReader.readLine();
	}
	
	def Dispose()
	{
		
	}
}