package System.Drawing

class Font(font:java.awt.Font) {
  final val Font = font;
  def this(famName:String, emSize:Float) =
  {
    this(new java.awt.Font(famName, 0, emSize.toInt));
  }
}