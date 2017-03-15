package System.Drawing.Imaging;

object ImageFormat
{

	
	val Jpeg = new ImageFormat("jpg");
	val Png = new ImageFormat("png");
}

class ImageFormat(str:String)
{
  final val Str = str;
}