package System.Drawing.Drawing2D;
import System.NotImplementedException;

class GraphicsPath
{
	var FillMode:Int = 0;

	def AddLine(x1:Float, y1:Float, x2:Float, y2:Float)
	{
		throw new NotImplementedException();
	}
	
	def AddArc(x:Float, y:Float, w:Float, h:Float, startAngle:Float, sweepAngle:Float)
	{
		throw new NotImplementedException();
	}
	
	def CloseFigure()
	{
		throw new NotImplementedException();
	}
	
	def AddBezier(x1:Float, y1:Float, x2:Float, y2:Float, x3:Float, y3:Float, x4:Float, y4:Float)
	{
		throw new NotImplementedException();
	}
}