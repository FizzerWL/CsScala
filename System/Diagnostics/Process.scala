package System.Diagnostics;
import System.NotImplementedException;

object Process {

  final val _current = new Process("java", GetCurrentPid()); //simple hack so that GetCurrentProcess().ProcessName works
  def GetCurrentProcess(): Process = _current;

  def Start(path: String): Process = {
    throw new NotImplementedException();
  }

  def GetCurrentPid(): Int = {
    //TODO: In java9, just use ProcessHandle.current().getPid();

    try {
      val runtime =
        java.lang.management.ManagementFactory.getRuntimeMXBean();
      val jvm = runtime.getClass().getDeclaredField("jvm");
      jvm.setAccessible(true);
      val mgmt = jvm.get(runtime).asInstanceOf[sun.management.VMManagement];
      val pid_method = mgmt.getClass().getDeclaredMethod("getProcessId");
      pid_method.setAccessible(true);
      return pid_method.invoke(mgmt).asInstanceOf[Integer];
    }
    catch {
      case ex: Exception => return 0;
    }
  }
}

class Process(name: String, pid: Int) {

  var HasExited: Boolean = false;
  final val ProcessName: String = name;
  final val Id: Int = pid;

  def Kill() {
    throw new NotImplementedException();
  }

}