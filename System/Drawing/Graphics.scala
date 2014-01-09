package System.Drawing;
import System.Drawing.Drawing2D.GraphicsPath;
import System.NotImplementedException;

object Graphics
{
  	def FromImage(bmp:Bitmap):Graphics = new Graphics(bmp);
}

class Graphics(bmp:Bitmap)
{

	var InterpolationMode:Int = 0;
	
	def Dispose()
	{
	}
	
	def Clear(c:Color)
	{
		throw new NotImplementedException();
	}
	
	
	def FillRectangle(b:Brush, x:Float, y:Float, w:Float, h:Float)
	{
		throw new NotImplementedException();
	}
	
	def DrawRectangle(p:Pen, x:Float, y:Float, w:Float, h:Float)
	{
		throw new NotImplementedException();
	}
	
	def FillPath(brush:Brush, path:GraphicsPath)
	{
		throw new NotImplementedException();
	}
	def DrawPath(pen:Pen, path:GraphicsPath)
	{
		throw new NotImplementedException();
	}
	
	def DrawEllipse(p:Pen, x:Float, y:Float, xRad:Float, yRad:Float)
	{
		throw new NotImplementedException();
	}
	def FillEllipse(b:Brush, x:Float, y:Float, xRad:Float, yRad:Float)
	{
		throw new NotImplementedException();
	}
	
	def DrawImage(img:Image, dest:Rectangle, src:Rectangle, unit:Int)
	{
		throw new NotImplementedException();
	}
}