package System;

import java.util.Base64

object Convert {

  def ToByte(s: String, base: Int): Byte = java.lang.Integer.valueOf(s, base).toByte;
  def ToInt32(s: String, base: Int): Int = Integer.valueOf(s, base);
  def toString(i: Int, base: Int): String = Integer.toString(i, base);
  def toString(i: Long, base: Int): String = java.lang.Long.toString(i, base);

  def ToBase64String(bytes: Array[Byte]): String = Base64.getEncoder().encodeToString(bytes);
  def FromBase64String(s:String):Array[Byte] = Base64.getDecoder().decode(s.replace("\n", ""));
 
}