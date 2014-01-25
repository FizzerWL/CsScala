package System.Drawing;

class PointF(x:Float = 0, y:Float = 0)
{

	
	final var X = x;
	final var Y = y;
	
	override def equals(other:Any):Boolean =
	  {
	    if (!other.isInstanceOf[PointF])
	      return false;
	    val o = other.asInstanceOf[PointF];
	    return o.X == X && o.Y == Y;
	  }
	override def hashCode():Int = X.toInt + Y.toInt;
}