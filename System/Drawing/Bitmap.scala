package System.Drawing;
import System.NotImplementedException;
import java.awt.image.BufferedImage

class Bitmap(w:Int, h:Int, format:Int) extends Image(new BufferedImage(w, h, BufferedImage.TYPE_INT_ARGB))
{
}