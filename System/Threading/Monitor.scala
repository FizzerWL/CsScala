package System.Threading;

import com.google.common.collect.MapMaker
import java.util.concurrent.locks.ReentrantLock
import System.ArgumentNullException
import java.util.concurrent.TimeUnit

object Monitor {

  final val _locks = new MapMaker().weakKeys().makeMap[Any, ReentrantLock]();

  def Enter(obj: Any):Unit = {
    if (obj == null)
      throw new ArgumentNullException("obj is null");

    val lock = new ReentrantLock(true);
    val existing = _locks.putIfAbsent(obj, lock);

    if (existing != null)
      existing.lock();
    else
      lock.lock();
  }
  def TryEnter(obj: Any, timeoutMS: Int): Boolean = {
    if (obj == null)
      throw new ArgumentNullException("obj is null");

    val lock = new ReentrantLock(true);
    val existing = _locks.putIfAbsent(obj, lock);

    if (existing != null)
      existing.tryLock(timeoutMS, TimeUnit.MILLISECONDS);
    else
      lock.tryLock(timeoutMS, TimeUnit.MILLISECONDS);
  }
  def Exit(obj: Any):Unit = {
    if (obj == null)
      throw new ArgumentNullException("obj is null");

    val lock = _locks.get(obj);

    if (lock == null)
      throw new Exception("Object " + obj + " not locked");
    lock.unlock();
  }
}