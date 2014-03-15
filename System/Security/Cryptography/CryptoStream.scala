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

object CryptoStream {
  private final val _cipher = new ThreadLocal[Cipher]() {
    override def initialValue(): Cipher =
      {
        Security.addProvider(new BouncyCastleProvider());
        Cipher.getInstance("DESede/CBC/PKCS5Padding", "BC");
      }
  };

}

class CryptoStream(stream: MemoryStream, encryptor: ICryptoTransform, streamMode: Int) {

  def Write(readFrom: Array[Byte], offset: Int, length: Int) {
    try {
      val fact = SecretKeyFactory.getInstance("DESede");
      val key = new SecretKeySpec(encryptor.DES.Key, "DESede");
      val iv = new IvParameterSpec(encryptor.DES.IV);

      val cip = CryptoStream._cipher.get();

      cip.init(if (encryptor.Encrypt) Cipher.ENCRYPT_MODE else Cipher.DECRYPT_MODE, key, iv);
      stream._overrideArray = cip.doFinal(readFrom, offset, length);
    }
    catch {
      case ex: javax.crypto.BadPaddingException => throw new CryptographicException(ex);
      case ex: javax.crypto.IllegalBlockSizeException => throw new CryptographicException(ex);
    }
  }

  def Dispose() {}
}