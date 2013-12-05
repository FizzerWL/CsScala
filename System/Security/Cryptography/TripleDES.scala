package System.Security.Cryptography;

import System.NotImplementedException;

object TripleDES
{
	def Create():TripleDES =
	{
		throw new NotImplementedException();
	}
}

class TripleDES
{

	
	var IV:Array[Byte] = null;
	var Key:Array[Byte] = null;
	
	def CreateEncryptor():ICryptoTransform =
	{
		throw new NotImplementedException();
	}
	def CreateDecryptor():ICryptoTransform =
	{
		throw new NotImplementedException();
	}
	
}