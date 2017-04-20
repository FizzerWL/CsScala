package System.Threading.Tasks;

class TaskFactory
{

	
	def StartNew[T](fn:()=>T):Task[T] =
	{
		val r = new Task(fn);
		r.Start();
		return r;
	}
	
	
	def StartNew(fn:()=>Unit):NonGenericTask =
	{
		val r = new NonGenericTask(fn);
		r.Start();
		return r;
	}
	
}