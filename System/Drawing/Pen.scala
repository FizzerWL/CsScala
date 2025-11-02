package System.Drawing;

class Pen(c:Color, thickness:Float)
{

  final val Thickness = thickness;
  final val Color = c;

  var LineJoin:Int = 0; //todo
  var StartCap:Int = 0; //todo
  var EndCap:Int = 0; //todo
  var MiterLimit:Float = 0; //todo
  var Alignment:Int = 0; //todo
}