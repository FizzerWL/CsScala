package System.Threading.Tasks;
import System.NotImplementedException;

class TaskFactory
{

	
	def StartNew[T](fn:()=>T):Task[T] =
	{
		val r = new Task(fn);
		r.Start();
		return r;
	}
	
}