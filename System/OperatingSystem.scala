package System

class OperatingSystem 
{
  def Platform:Int = {
    //All we do is return 4 if we're linux, and anything else otherwise.  Not an accurate representation of this property, but it's all we need right now
    if (java.lang.System.getProperty("os.name").indexOf("Windows") != -1)
      return 0;
    else
      return 4;
  }

}