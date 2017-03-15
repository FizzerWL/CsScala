package System;

class TimeoutException(msg:String = "No message", inner:Exception = null) extends Exception(msg, inner)
{
	
}