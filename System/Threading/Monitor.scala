package System.Threading;
import System.NotImplementedException
import com.google.common.collect.MapMaker
import java.util.concurrent.locks.ReentrantLock
import System.ArgumentNullException

object Monitor {

  final val _locks = new MapMaker().weakKeys().makeMap[Any, ReentrantLock]();

  def Enter(obj: Any) {
    if (obj == null)
      throw new ArgumentNullException("obj is null");
    
    val lock = new ReentrantLock(true);
    val existing = _locks.putIfAbsent(obj, lock);

    if (existing != null)
      existing.lock();
    else
      lock.lock();
  }
  def Exit(obj: Any) {
    if (obj == null)
      throw new ArgumentNullException("obj is null");
    
    val lock = _locks.get(obj);

    if (lock == null)
      throw new Exception("Object " + obj + " not locked");
    lock.unlock();
  }
}