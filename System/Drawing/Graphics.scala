package System.Drawing;
import System.Drawing.Drawing2D.GraphicsPath;
import java.awt.BasicStroke
import System.NotImplementedException
import System.Drawing.Drawing2D.Matrix

object Graphics {
  def FromImage(bmp: Bitmap): Graphics = new Graphics(bmp);
}

class Graphics(bmp: Bitmap) {

  final var InterpolationMode = 0;
  final var SmoothingMode = 0;
  final var PixelOffsetMode = 0;
  final var CompositingQuality = 0;
  
  def ScaleTransform(x:Float, y:Float) {
    throw new NotImplementedException();
  }

  final val _graphics = bmp._img.createGraphics();

  def Dispose() {
  }

  def Clear(c: Color) {
    _graphics.clearRect(0, 0, bmp.Width, bmp.Height);
  }

  private def SetBrush(b: Brush) {
    if (b.isInstanceOf[SolidBrush])
      _graphics.setColor(b.asInstanceOf[SolidBrush].Color.Color);
    else
      throw new Exception("Brush handler");
  }
  private def SetPen(p: Pen) {
    _graphics.setStroke(new BasicStroke(p.Thickness));
    _graphics.setColor(p.Color.Color);
  }

  def FillRectangle(b: Brush, x: Float, y: Float, w: Float, h: Float) {
    SetBrush(b);
    _graphics.fillRect(x.toInt, y.toInt, w.toInt, h.toInt);
  }
  def FillRectangle(b: Brush, rect: Rectangle) {
    FillRectangle(b, rect.X, rect.Y, rect.Width, rect.Height);
  }
  def FillRectangle(b: Brush, rect: RectangleF) {
    FillRectangle(b, rect.X, rect.Y, rect.Width, rect.Height);
  }

  def DrawRectangle(p: Pen, x: Float, y: Float, w: Float, h: Float) {
    SetPen(p);
    _graphics.drawRect(x.toInt, y.toInt, w.toInt, h.toInt);
  }

  def FillPath(brush: Brush, path: GraphicsPath) {
    SetBrush(brush);
    _graphics.fill(path.Shape);
  }
  def DrawPath(pen: Pen, path: GraphicsPath) {
    SetPen(pen);
    _graphics.draw(path.Shape);
  }

  def DrawEllipse(p: Pen, x: Float, y: Float, xRad: Float, yRad: Float) {
    SetPen(p);
    _graphics.drawOval(x.toInt, y.toInt, xRad.toInt * 2, yRad.toInt * 2);
  }
  def FillEllipse(b: Brush, x: Float, y: Float, xRad: Float, yRad: Float) {
    SetBrush(b);
    _graphics.fillOval(x.toInt, y.toInt, xRad.toInt * 2, yRad.toInt * 2);
  }

  def DrawImage(img: Image, dest: Rectangle, src: Rectangle, unit: Int) {
    _graphics.drawImage(img._img, dest.X, dest.Y, dest.X + dest.Width, dest.Y + dest.Height,
      src.X, src.Y, src.X + src.Width, src.Y + src.Height, null);
  }
  def DrawImage(img: Image, dest: RectangleF, src: RectangleF, unit: Int) {
    DrawImage(img, dest.ToRectangle(), src.ToRectangle(), unit);
  }
  def DrawImage(img: Image, dest: Rectangle, src: RectangleF, unit: Int) {
    DrawImage(img, dest, src.ToRectangle(), unit);
  }
  def DrawImage(img: Image, dest: Rectangle) {
    _graphics.drawImage(img._img, dest.X, dest.Y, dest.Width, dest.Height, null);
  }
  def DrawImage(img: Image, dest: RectangleF) {
    DrawImage(img, dest.ToRectangle());
  }

  def DrawString(s: String, font: Font, brush: Brush, x: Float, y: Float) {
    SetBrush(brush);
    _graphics.setFont(font.Font);
    _graphics.drawString(s, x, y + font.Font.getSize());
  }

  def DrawString(s: String, font: Font, brush: Brush, rect: RectangleF) {
    DrawString(s, font, brush, rect, null);
  }

  def DrawString(s: String, font: Font, brush: Brush, rect: RectangleF, format: StringFormat) {

    if (format != null && format.FormatFlags != StringFormatFlags.NoWrap)
      throw new NotImplementedException("Wrapping not implemented");
    val center = format != null && format.Alignment == StringAlignment.Center;
    
    if (!center)
      DrawString(s, font, brush, rect.X, rect.Y);
    else
    {
      val metrics = _graphics.getFontMetrics(font.Font);
      val width = metrics.stringWidth(s);
      DrawString(s, font, brush, rect.X + (rect.Width - width) / 2, rect.Y);
    }

    /*
    val metrics = _graphics.getFontMetrics(font.Font);
    val img = new Bitmap(metrics.stringWidth(s), metrics.getHeight());
    Graphics.FromImage(img).DrawString(s, font, brush, 0, 0);
    
    val wRatio = rect.Width / img.Width;
    val hRatio = rect.Height / img.Height;

    val scaledRect = if (wRatio < hRatio) {
      val newHeight = img.Height * wRatio;
      new RectangleF(rect.X, if (center) (rect.Height - newHeight) / 2f + rect.Y else rect.Y, rect.Width, newHeight);
    } else {
      val newWidth = img.Width * hRatio;
      new RectangleF(if (center) (rect.Width - newWidth) / 2f + rect.X else rect.X, rect.Y, newWidth, rect.Height);
    }

    println("Drawing string " + s + ": rect=" + rect + " scaledRect=" + scaledRect + " center=" + center + " wRatio=" + wRatio + " hRatio=" + hRatio);
    DrawImage(img, scaledRect);*/
  }

  def Flush() {}
}