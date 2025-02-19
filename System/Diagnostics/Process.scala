package System.Diagnostics;
import System.NotImplementedException;

object Process {

  final val _current = new Process("java", GetCurrentPid()); //simple hack so that GetCurrentProcess().ProcessName works
  def GetCurrentProcess(): Process = _current;

  def Start(path: String): Process = {
    throw new NotImplementedException();
  }

  def GetCurrentPid(): Int = {
    

    try {
      //return java.lang.ProcessHandle.current().pid(); //java 9+ way
      
      // java <9 way:
      val runtime = java.lang.management.ManagementFactory.getRuntimeMXBean();


      val name = runtime.getName();
      var i = name.indexOf('@');
      if (i == -1)
        return 0;
      return name.substring(0, i).toInt;


      
      //old way, that still works on windows and used to work on linux until we updated to ubuntu 24
      // val jvm = runtime.getClass().getDeclaredField("jvm");
      // jvm.setAccessible(true);
      // val mgmt = jvm.get(runtime).asInstanceOf[sun.management.VMManagement];
      // val pid_method = mgmt.getClass().getDeclaredMethod("getProcessId");
      // pid_method.setAccessible(true);
      // return pid_method.invoke(mgmt).asInstanceOf[Integer];
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

  def Kill():Unit = {
    throw new NotImplementedException();
  }

}