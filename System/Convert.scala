package System;

object Convert {

  def ToByte(s: String, base: Int): Byte =
    {
      return java.lang.Integer.valueOf(s, base).toByte;
    }
  def ToInt32(s: String, base: Int): Int =
    {
      return Integer.valueOf(s, base);
    }

  def ToBase64String(bytes: Array[Byte]): String =
    {
      return new String(Base64Coder.encode(bytes));
    }
  def FromBase64String(s:String):Array[Byte] =
    {
      return Base64Coder.decode(s);
    }

  def toString(i: Int, base: Int): String =
    {
      return Integer.toString(i, base);
    }
  def toString(i: Long, base: Int): String =
    {
      return java.lang.Long.toString(i, base);
    }
}