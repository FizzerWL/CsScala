package System.Security.Cryptography;

import java.security.MessageDigest

class SHA256Managed {
  final val _digest = MessageDigest.getInstance("SHA-256")

  def ComputeHash(bytes: Array[Byte]): Array[Byte] =
    {
      return _digest.digest(bytes);
    }

}