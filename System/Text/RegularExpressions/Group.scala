package System.Text.RegularExpressions;

class Group(v: String) {

  final val Value: String = v;
  override def toString(): String = v;
  final val Success = v != null;
}