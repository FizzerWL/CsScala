package System.Drawing;
import java.awt.image.BufferedImage
import java.io.File;
import javax.imageio.ImageIO
import System.Drawing.Imaging.PixelFormat
import System.IO.MemoryStream
import java.io.ByteArrayInputStream

object Bitmap {

  def FromFile(path:String):Bitmap = new Bitmap(ImageIO.read(new File(path)));
  def FromStream(strm:MemoryStream):Bitmap = new Bitmap(ImageIO.read(new ByteArrayInputStream(strm.ToArray())));
}

class Bitmap(img:BufferedImage) extends Image(img) {
  
  val PixelFormat:Int = 0;
  def this(w: Int, h: Int, format: Int = 0) =
  {
    this(new BufferedImage(w, h, if (format == PixelFormat.Format24bppRgb) BufferedImage.TYPE_3BYTE_BGR else BufferedImage.TYPE_INT_ARGB));
  }
  
  def Clone(rect:Rectangle, format:Int = 0):Bitmap = {
    //TODO: Obey format
    return new Bitmap(img.getSubimage(rect.X, rect.Y, rect.Width, rect.Height));
  }
}