package System.Drawing;
import java.awt.image.BufferedImage
import java.io.File;
import javax.imageio.ImageIO
import System.Drawing.Imaging.PixelFormat

object Bitmap {

  def FromFile(path:String):Bitmap = new Bitmap(ImageIO.read(new File(path)));
}

class Bitmap(img:BufferedImage) extends Image(img) {
  
  def this(w: Int, h: Int, format: Int = 0)
  {
    this(new BufferedImage(w, h, if (format == PixelFormat.Format24bppRgb) BufferedImage.TYPE_3BYTE_BGR else BufferedImage.TYPE_INT_ARGB));
  }
}