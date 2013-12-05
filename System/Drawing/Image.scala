package System.Drawing;
import System.Drawing.Imaging.ImageFormat;
import System.IntPtr;
import System.IO.Stream;
import System.NotImplementedException;

object Image
{
  	def FromStream(s:Stream):Image =
	{
		throw new NotImplementedException();
	}

}

class Image
{

	
	var Width:Int = 0;
	var Height:Int = 0;
	
	def RotateFlip(fliptype:Int)
	{
		throw new NotImplementedException();
	}
	
	def GetThumbnailImage(width:Int, height:Int, abort:()=>Boolean, callbackData:IntPtr):Image =
	{
		throw new NotImplementedException();
	}
	
	def Save(s:Stream, format:ImageFormat)
	{
		throw new NotImplementedException();
	}
}