package System.Security.Cryptography;
import System.IO.Stream
import System.NotImplementedException
import System.IO.MemoryStream
import javax.crypto._
import javax.crypto.spec.DESedeKeySpec
import javax.crypto.spec.IvParameterSpec
import org.bouncycastle.jce.provider.BouncyCastleProvider
import java.security.Security
import javax.crypto.spec.SecretKeySpec


class CryptoStream(stream:MemoryStream, encryptor:ICryptoTransform, streamMode:Int)
{
  
  def Write(readFrom:Array[Byte], offset:Int, length:Int)
  {
    Security.addProvider(new BouncyCastleProvider());
    val cip = Cipher.getInstance("DESede/CBC/PKCS5Padding", "BC");
    
    val fact = SecretKeyFactory.getInstance("DESede");
    val key = new SecretKeySpec(encryptor.DES.Key, "DESede");// fact.generateSecret(new DESedeKeySpec(encryptor.DES.Key));
    val iv = new IvParameterSpec(encryptor.DES.IV);
    
    
    cip.init(if (encryptor.Encrypt) Cipher.ENCRYPT_MODE else Cipher.DECRYPT_MODE, key, iv);
    stream._overrideArray = cip.doFinal(readFrom, offset, length);
  }
  
  def Dispose() { }
}