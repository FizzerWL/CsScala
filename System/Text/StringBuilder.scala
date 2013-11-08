package System.Text


class StringBuilder(sb:java.lang.StringBuilder) 
{
  
  def this(capacity:Int)
  {
    this(new java.lang.StringBuilder(capacity));
  }
  def this(initial:String)
  {
    this(new java.lang.StringBuilder(initial));
  }
  def this()
  {
    this(new java.lang.StringBuilder());
  }
  
  def Append(c:Char)
  {
    sb.append(c);
  }
  def Append(s:String)
  {
    sb.append(s);
  }
  def AppendLine(s:String)
  {
    sb.append(s);
    sb.append('\n');
  }
  def Append(i:Int)
  {
    sb.append(i);
  }
  
  def Insert(index:Int, c:Char)
  {
    sb.insert(index, c);
  }
  def Insert(index:Int, s:String)
  {
    sb.insert(index, s);
  }
  
  def Length:Int =
  {
    return sb.length();
  }
  
  override def toString():String = 
  {
    return sb.toString();
  }
}