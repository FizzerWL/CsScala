package System.IO;


class FileInfo(path:String)
{

	
	
	var Length:Int = new java.io.File(path).length().toInt;
	var LastWriteTime:System.DateTime = new System.DateTime(new org.joda.time.Instant(new java.io.File(path).lastModified()));
	
}