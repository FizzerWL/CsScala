package System.Drawing;
import System.Drawing.Imaging.ImageFormat
import System.IntPtr
import System.IO.Stream
import System.NotImplementedException
import java.awt.image.BufferedImage
import javax.imageio.ImageIO
import System.ArgumentException

object Image {
  def FromStream(s: Stream): Image = {
    val img = ImageIO.read(s._input);
    if (img == null)
      throw new ArgumentException("Image failed to parse");
    return new Image(img);
  } 

}

class Image(img: BufferedImage) {
  def Width: Int = img.getWidth();
  def Height: Int = img.getHeight();

  def RotateFlip(fliptype: Int) {
    
  }

  def GetThumbnailImage(width: Int, height: Int, abort: () => Boolean, callbackData: IntPtr): Image =
    {
      val scaled = img.getScaledInstance(width, height, java.awt.Image.SCALE_SMOOTH);
      val buf = new BufferedImage(scaled.getWidth(null), scaled.getHeight(null), BufferedImage.TYPE_INT_RGB);
      buf.getGraphics().drawImage(scaled, 0, 0 , null);
      return new Image(buf);
    }

  def Save(s: Stream, format: ImageFormat) {
    if (!ImageIO.write(img, format.Str, s._output))
      throw new Exception("ImageIO.write returned false");
  }
}