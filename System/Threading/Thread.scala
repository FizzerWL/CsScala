package System.Threading;

object Thread {
  def Sleep(ms: Int) {
    java.lang.Thread.sleep(ms);
  }

  def CurrentThread: Thread = new Thread(java.lang.Thread.currentThread());
}

class ThreadRunnable(fn: Any => Unit) extends Runnable {
  def run() { fn(null); }
}

class Thread(_t: java.lang.Thread) {

  def this(fn: Any => Unit) {
    this(new java.lang.Thread(new ThreadRunnable(fn)));
  }

  def Name: String = _t.getName();
  def Name_=(v: String) { _t.setName(v); }

  def ManagedThreadId: Int = _t.getId().toInt;
  def IsAlive: Boolean = _t.isAlive(); //.getState() != java.lang.Thread.State.TERMINATED;

  def Start() = _t.start();

  def SetApartmentState(state: Int) {}
  def Abort() { _t.stop(); }

}