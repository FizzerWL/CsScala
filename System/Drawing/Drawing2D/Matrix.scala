package System.Drawing.Drawing2D;
import System.Drawing.PointF
import org.lwjgl.util.vector._
import java.awt.geom.AffineTransform
import java.awt.geom.Point2D

class Matrix(m11: Float = 1, m12: Float = 0, m21: Float = 0, m22: Float = 1, dx: Float = 0, dy: Float = 0) {

  //http://msdn.microsoft.com/en-us/library/system.drawing.drawing2d.matrix(v=vs.110).aspx
  val _m = new AffineTransform(m11, m12, m21, m22, dx, dy);
  
  
  override def equals(other: Any): Boolean =
    {
      if (!other.isInstanceOf[Matrix])
        return false;
      val o = other.asInstanceOf[Matrix];
      return o._m == _m;
    }
  override def hashCode(): Int = _m.hashCode();
  
  def Elements:Array[Float] = {
    val doubles = new Array[Double](6);
    _m.getMatrix(doubles); //m00 m10 m01 m11 m02 m12
    return Array[Float](doubles(0).toFloat, doubles(1).toFloat, doubles(2).toFloat, doubles(3).toFloat, doubles(4).toFloat, doubles(5).toFloat);
  }
  
  def Translate(x: Float, y: Float):Unit = {
    _m.translate(x,y);
  }

  def Scale(x: Float, y: Float):Unit = {
    _m.scale(x,y);
  }
  def Rotate(angle: Float):Unit = {
    _m.rotate(angle / 360f * Math.PI * 2);
  }

  def TransformPoints(p: Array[PointF]):Unit = {
    var i = 0;
    while (i < p.length)
    {
      val dest = new Point2D.Float();
      _m.transform(new Point2D.Float(p(i).X, p(i).Y), dest);
      p(i) = new PointF(dest.x, dest.y);
      i += 1;
    }
  }

  def TransformVectors(p: Array[PointF]):Unit = {
    
    val noTranslateDoubles = new Array[Double](4);
    _m.getMatrix(noTranslateDoubles);
    val noTranslate = new AffineTransform(noTranslateDoubles(0), noTranslateDoubles(1), noTranslateDoubles(2), noTranslateDoubles(3), 0, 0);
    
    var i = 0;
    while (i < p.length)
    {
      val dest = new Point2D.Float();
      noTranslate.transform(new Point2D.Float(p(i).X, p(i).Y), dest);
      p(i) = new PointF(dest.x, dest.y);
      i += 1;
    }
  }

  def Multiply(m: Matrix):Unit = {
    _m.concatenate(m._m);
  }

  def IsIdentity: Boolean = _m.isIdentity();

  
  override def toString():String = _m.toString();
}