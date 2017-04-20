package System.Security.Cryptography;

object TripleDES {
  def Create(): TripleDES = new TripleDES();
}

class TripleDES 
{

  var IV: Array[Byte] = null;
  var Key: Array[Byte] = null;

  def CreateEncryptor(): ICryptoTransform = new TripleDESTransform(this, true);
  def CreateDecryptor(): ICryptoTransform = new TripleDESTransform(this, false);

}

class TripleDESTransform(des:TripleDES, encrypt:Boolean) extends ICryptoTransform
{
  def DES = des;
  def Encrypt = encrypt;
}