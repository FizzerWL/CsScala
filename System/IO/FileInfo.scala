package System.IO;
import System.NotImplementedException;

class FileInfo(path:String)
{

	
	
	var Length:Int = new java.io.File(path).length().toInt;
	
}