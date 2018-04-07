package System.Drawing;

class RectangleF(x:Float = 0, y:Float = 0, w:Float = 0, h:Float = 0)
{

	var X:Float = x;
	var Y:Float = y;
	var Width:Float = w;
	var Height:Float = h;
	
	override def equals(other:Any):Boolean =
	  {
	    if (!other.isInstanceOf[RectangleF])
	      return false;
	    val o = other.asInstanceOf[RectangleF];
	    return o.X == X && o.Y == Y && o.Width == Width && o.Height == Height;
	  }
	override def hashCode():Int = (X + Y + Width + Height).toInt;
	
	def ToRectangle():Rectangle = new Rectangle(x.toInt, y.toInt, w.toInt, h.toInt);
	def Left = X;
	def Top = Y;
	
	override def toString():String = X + "/" + Y + " " + Width + "/" + Height;
}