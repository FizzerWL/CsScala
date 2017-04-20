package System.Drawing;

class Rectangle(x:Int, y:Int, w:Int, h:Int)
{
	final var X:Int = x;
	final var Y:Int = y;
	final var Width:Int = w;
	final var Height:Int = h;

	override def equals(other:Any):Boolean =
	  {
	    if (!other.isInstanceOf[Rectangle])
	      return false;
	    val o = other.asInstanceOf[Rectangle];
	    return o.X == X && o.Y == Y && o.Width == Width && o.Height == Height;
	  }
	override def hashCode():Int = X + Y + Width + Height;
	override def toString():String = X + "/" + Y + " " + Width + "/" + Height;
}