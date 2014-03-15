package System

import java.util.Date

object Environment 
{
  def TickCount:Int = new Date().getTime().toInt;
  def CurrentDirectory:String = java.lang.System.getProperty("user.dir");
  
  final val NewLine:String = "\n";

  final val OSVersion = new OperatingSystem();
  
  var ExitCode:Int = 0;
}