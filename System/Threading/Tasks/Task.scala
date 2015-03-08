package System.Threading.Tasks;
import System.NotImplementedException;
import System.Threading.ThreadPool

object Task {
  final val Factory: TaskFactory = new TaskFactory();
}

class Task[T](fn: () => T) {

  var Result: T = _;
  @volatile var IsCompleted: Boolean = false;
  var IsFaulted: Boolean = false;
  var Exception: Exception = null;

  def Wait() {
    while (!IsCompleted)
      java.lang.Thread.sleep(10);
  }

  def Start() {
    ThreadPool.QueueUserWorkItem(_ => {
      try {
        Result = fn();
        IsFaulted = false;
      } catch {
        case ex: java.lang.Exception =>
          Exception = ex;
          IsFaulted = true;
      }
      IsCompleted = true;

    }, null)
  }

}

//JVM does not support a non-generic type with the same name as a generic type like .net does.  Therefore, we re-map the non-generic version to NonGenericTask in Translations.xml 
object NonGenericTask {
  final val Factory: TaskFactory = Task.Factory;
}
class NonGenericTask(fn: () => Unit) {

  @volatile var IsCompleted: Boolean = false;
  var IsFaulted: Boolean = false;
  var Exception: Exception = null;

  def Wait() {
    while (!IsCompleted)
      java.lang.Thread.sleep(10);
  }

  def Start() {
    ThreadPool.QueueUserWorkItem(_ => {
      try {
        fn();
        IsFaulted = false;
      } catch {
        case ex: java.lang.Exception =>
          Exception = ex;
          IsFaulted = true;
      }
      IsCompleted = true;

    }, null)
  }

}