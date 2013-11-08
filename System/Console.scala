package System

object Console {
  def WriteLine(s:String)
  {
    java.lang.System.out.println(s);
  }
  
  def WriteLine(i:Int)
  {
    java.lang.System.out.println(i.toString());
  }
  def WriteLine(i:Double)
  {
    java.lang.System.out.println(i.toString());
  }
  def WriteLine(i:Boolean)
  {
    java.lang.System.out.println(i.toString());
  }

}