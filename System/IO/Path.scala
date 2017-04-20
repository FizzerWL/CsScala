package System.IO;


object Path
{
	
	def Combine(p1:String, p2:String):String =
	{
		return new java.io.File(new java.io.File(p1), p2).getAbsolutePath();
	}
	def Combine(p1:String, p2:String, p3:String):String =
	{
		return new java.io.File(new java.io.File(new java.io.File(p1), p2), p3).getAbsolutePath();
	}
	def Combine(p1:String, p2:String, p3:String, p4:String):String =
	{
		return new java.io.File(new java.io.File(new java.io.File(new java.io.File(p1), p2), p3), p4).getAbsolutePath();
	}
	def GetDirectoryName(path:String):String =
	{
		new java.io.File(path).getParent();
	}
	
	def GetFileName(path:String):String =
	{
		new java.io.File(path).getName();
	}
	
	val DirectorySeparatorChar:String = "/";
	
	def GetTempFileName():String =
	{
		return java.io.File.createTempFile("csscala", null).getAbsolutePath();
	}
	
}