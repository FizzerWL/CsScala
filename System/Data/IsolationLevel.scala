package System.Data;

object IsolationLevel
{
	final val Unspecified:Int = -1;
	final val Chaos:Int = 16;
	final val ReadUncommitted:Int = 256;
	final val ReadCommitted:Int = 4096;
	final val RepeatableRead:Int = 65536;
	final val Serializable:Int = 1048576;
	final val Snapshot:Int = 16777216;
	
}