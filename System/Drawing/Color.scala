package System.Drawing
import System.CsScala;

object Color {
  def FromArgb(a: Int, r: Int, g: Int, b: Int): Color = new Color(a.toByte, r.toByte, g.toByte, b.toByte);
  def FromArgb(r: Int, g: Int, b: Int): Color = new Color(255.toByte, r.toByte, g.toByte, b.toByte);

  val Black: Color = new Color(255.toByte, 0, 0, 0);
  val Green: Color = new Color(255.toByte, 0, 255.toByte, 0);
  val Transparent: Color = new Color(0, 0, 0, 0);
  val Gray: Color = new Color(128.toByte, 128.toByte, 128.toByte);
  val White: Color = new Color(255.toByte, 255.toByte, 255.toByte, 255.toByte);
}

class Color(a: Byte = 0, r: Byte = 0, g: Byte = 0, b: Byte = 0) {

  val A = a;
  val R = r;
  val G = g;
  val B = b;

  override def equals(other: Any): Boolean = {
    if (!other.isInstanceOf[Color])
      return false;
    val o = other.asInstanceOf[Color];
    return o.A == a && o.R == r && o.G == g && o.B == b;
  }
  override def hashCode(): Int = a + r + g + b;

  def Color: java.awt.Color = new java.awt.Color(CsScala.ByteToInt(r), CsScala.ByteToInt(g), CsScala.ByteToInt(b), CsScala.ByteToInt(a));
}