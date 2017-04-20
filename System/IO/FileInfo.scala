package System.IO;


class FileInfo(path:String)
{

	
	
	var Length:Int = new java.io.File(path).length().toInt;
	
}