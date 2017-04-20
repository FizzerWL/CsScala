package System.Threading;

import java.util.concurrent._;

object ThreadPool {
  val _pool = Executors.newCachedThreadPool();

  def QueueUserWorkItem(cb: AnyRef => Unit, obj: AnyRef = null) {
    _pool.execute(new Runnable {
      def run() {
    	cb(obj);        
      }
    });
  }

}