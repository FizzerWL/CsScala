package System.Drawing.Drawing2D;
import java.awt.geom.GeneralPath;
import java.awt.geom.Path2D;
import java.awt.geom.Arc2D;
import java.util.ArrayList;

class GraphicsPath {
  final val Shape = new GeneralPath();
  final var _isEmpty = true;
  final var penX = 0f;
  final var penY = 0f;

  final def FillMode: Int = if (Shape.getWindingRule() == Path2D.WIND_EVEN_ODD) System.Drawing.Drawing2D.FillMode.Alternate else System.Drawing.Drawing2D.FillMode.Winding;
  final def FillMode_=(winding: Int) {
    Shape.setWindingRule(if (winding == System.Drawing.Drawing2D.FillMode.Alternate) Path2D.WIND_EVEN_ODD else Path2D.WIND_NON_ZERO);
  }
  
  def CheckPen(x:Float, y:Float, endX:Float, endY:Float) {
    if (penX != x || penY != y)
      Shape.moveTo(x, y);
    penX = endX;
    penY = endY;
  }

  def AddLine(x1: Float, y1: Float, x2: Float, y2: Float) {
    _isEmpty = false;
    CheckPen(x1, y1, x2, y2);
    Shape.lineTo(x2, y2);
  }

  def AddArc(x: Float, y: Float, w: Float, h: Float, startAngle: Float, sweepAngle: Float) {
    _isEmpty = false;
    val arc = new Arc2D.Float(x, y, w, h, startAngle, sweepAngle, Arc2D.OPEN);
    val startPoint = arc.getStartPoint();
    val endPoint = arc.getEndPoint();
    CheckPen(startPoint.getX().toFloat, startPoint.getY().toFloat, endPoint.getX().toFloat, endPoint.getY().toFloat);
    Shape.append(arc, false);
  }

  def AddBezier(x1: Float, y1: Float, x2: Float, y2: Float, x3: Float, y3: Float, x4: Float, y4: Float) {
    _isEmpty = false;
    CheckPen(x1, y1, x4, y4);
    Shape.curveTo(x2, y2, x3, y3, x4, y4);
  }

  def CloseFigure() {
    if (!_isEmpty)
      Shape.closePath();
  }

}