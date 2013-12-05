package System.Drawing

import System.NotImplementedException

object Color
{
  def FromArgb(a:Int, r:Int, g:Int, b:Int):Color = 
  {
    return new Color(a,r,g,b);
  }
  def FromArgb(r:Int, g:Int, b:Int):Color = 
  {
    return new Color(255,r,g,b);
  }
  
  val Black:Color = new Color(255, 0, 0, 0);
  val Green:Color = new Color(255, 0, 255, 0);
  val Transparent:Color = new Color(0,0,0,0);
  val White:Color = new Color(255, 255, 255, 255);
}

class Color(a:Int = 0, r:Int = 0, g:Int = 0, b:Int = 0)
{

  val A:Int = a;
  val R:Int = r;
  val G:Int = g;
  val B:Int = b;
}