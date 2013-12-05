package System.Threading.Tasks;
import System.NotImplementedException;
import System.Threading.ThreadPool

object Task
{
	final val Factory:TaskFactory = new TaskFactory();

}

class Task[T](fn:()=>T)
{

	var Result:T = _;
	@volatile var IsCompleted:Boolean = false;
	var IsFaulted:Boolean = false;
	var Exception:Exception = null;
	
	
	
	def Wait()
	{
		while (!IsCompleted)
		  java.lang.Thread.sleep(10);
	}
	
	def Start()
	{
		ThreadPool.QueueUserWorkItem(_ => {
		  try
		  {
		    Result = fn();
		    IsFaulted = false;
		    IsCompleted = true;
		  }
		  catch
		  {
		    case ex: java.lang.Exception =>
		      Exception = ex;
		      IsFaulted = true;
		      IsCompleted = true;
		  }
		  
		}, null)
	}
	
}