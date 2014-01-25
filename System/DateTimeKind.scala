package System

object DateTimeKind {

  final val Unspecified = 0;
  final val Utc = 1;
  final val Local = 2;
  
  def toString(kind:Int):String = kind match
  {
    case Unspecified => "Unspecified";
    case Utc => "Utc";
    case Local => "Local";
  }
  
  def Parse(s:String):Int = s match
  {
    case "Unspecified" => Unspecified;
    case "Utc" => Utc;
    case "Local" => Local;
  }
}