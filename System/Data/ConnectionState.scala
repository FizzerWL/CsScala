package System.Data;

object ConnectionState
{
	
	final val Closed:Int = 0;
	final val Open:Int = 1;
	final val Connecting:Int = 2;
	final val Executing:Int = 4;
	final val Fetching:Int = 8;
	final val Broken:Int = 16;
}