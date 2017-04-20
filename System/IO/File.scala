package System.IO;

import java.io._
import java.nio.file.Files
import java.nio.file.CopyOption
import java.nio.file.StandardCopyOption

object File {

  def Exists(path: String): Boolean =
    {
      val f = new java.io.File(path);
      return f.exists() && !f.isDirectory();
    }
  def Delete(path: String) {
    if (!new java.io.File(path).delete())
      throw new IOException("File could not be deleted");
  }

  def AppendAllText(path: String, text: String) {
    val fileWriter = new FileWriter(path, true);
    fileWriter.write(text);
    fileWriter.close();

  }
  def OpenRead(path: String): FileStream = new FileStream(new FileInputStream(path));
  def ReadAllBytes(path: String): Array[Byte] = {
    var f = new RandomAccessFile(path, "r");
    val ret = new Array[Byte](f.length().toInt);
    f.readFully(ret);
    f.close();
    return ret;
  }
  def ReadAllText(path: String): String = {
    val freader = new FileReader(path);
    val fileData = new StringBuffer();
    val reader = new BufferedReader(freader);
    val buf = new Array[Char](1024);
    var numRead = 0;
    do {
      numRead = reader.read(buf);
      if (numRead != -1)
        fileData.append(String.valueOf(buf, 0, numRead));
    } while (numRead != -1);
    reader.close();
    freader.close();
    return fileData.toString();
  }
  def WriteAllBytes(path: String, bytes: Array[Byte]) = {
    val f = new java.io.RandomAccessFile(path, "rw");
    f.setLength(0);
    f.write(bytes);
    f.close();
  }
  def WriteAllText(path: String, text: String) = {
    val fileWriter = new FileWriter(path);
    fileWriter.write(text);
    fileWriter.close();
  }

  def GetAttributes(path: String): Int = {
    return if (new File(path).canWrite()) FileAttributes.Normal else FileAttributes.ReadOnly;

  }
  def SetAttributes(path: String, attrs: Int) {
    new File(path).setReadable(attrs == FileAttributes.Normal);
  }

  def Copy(src: String, dest: String, overwrite: Boolean = false) {
    if (overwrite)
      Files.copy(new File(src).toPath(), new File(dest).toPath(), StandardCopyOption.REPLACE_EXISTING);
    else
      Files.copy(new File(src).toPath(), new File(dest).toPath());
  }
  def Move(src: String, dest: String) {
    Files.move(new File(src).toPath(), new File(dest).toPath());
  }
}