package System.IO;

import java.util.ArrayList

object Directory {

  def Exists(path: String): Boolean =
    {
      val f = new java.io.File(path);
      return f.exists() && f.isDirectory();
    }

  def CreateDirectory(path: String):Unit = {
    new java.io.File(path).mkdirs();
  }

  private def WalkFiles(path: String, ret: ArrayList[String]):Unit = {

    val root = new java.io.File(path);
    val list = root.listFiles();

    if (list == null)
      return;

    for (f <- list) {
      if (f.isDirectory())
        WalkFiles(f.getAbsolutePath(), ret);
      else
        ret.add(f.getAbsolutePath());

    }
  }

  def GetFilesInternal(path: String, option: Int): Array[String] =
    {

      if (option == SearchOption.AllDirectories) {
        val ret = new ArrayList[String]();
        WalkFiles(path, ret);
        return ret.toArray(Array[String]());
      }
      else {
        val files = new java.io.File(path).listFiles();
        if (files == null)
          return new Array[String](0);
        else
          return files.map(_.getAbsolutePath());
      }

    }

  def PatternRegex(pattern: String): String = pattern.replaceAll("([^a-zA-z0-9 ])", "\\\\$1").replace("\\*", ".*");

  def GetFiles(path: String, pattern: String = "*", option: Int = SearchOption.TopDirectoryOnly): Array[String] =
    {
      var all = GetFilesInternal(path, option);
      if (pattern == "*" || pattern == "*.*")
        return all;
      else {
        val replacedPattern = PatternRegex(pattern);
        return all.filter(_.matches(replacedPattern));
      }
    }

  def GetDirectories(path: String): Array[String] =
    {
      val files = new java.io.File(path).listFiles();
      if (files == null)
        return new Array[String](0);
      else
        return files.filter(_.isDirectory()).map(_.getAbsolutePath());
    }

  def WalkDirectories(path: String, ret: ArrayList[String]):Unit = {
    for (d <- GetDirectories(path)) {
      ret.add(d);
      WalkDirectories(d, ret);
    }
  }

  def GetDirectoriesInternal(path: String, option: Int): Array[String] =
    {
      if (option == SearchOption.TopDirectoryOnly)
        return GetDirectories(path);
      else {
        val ret = new ArrayList[String]();
        WalkDirectories(path, ret);
        return ret.toArray().asInstanceOf[Array[String]];
      }
    }

  def GetDirectories(path: String, pattern: String = "*.*", option: Int = SearchOption.TopDirectoryOnly): Array[String] =
    {
      val all = GetDirectoriesInternal(path, option);
      if (pattern == "*" || pattern == "*.*")
        return all;
      else {
        val patternRegex = PatternRegex(pattern);
        return all.filter(_.matches(patternRegex));
      }
    }

  private def DeleteRecursive(directory: java.io.File):Unit = {

    val files = directory.listFiles();
    if (files != null) {
      for (e <- files)
        if (e.isDirectory())
          DeleteRecursive(e);
        else {
          if (!e.delete())
            throw new IOException("Delete failed");
        }
    }

    if (!directory.delete())
      throw new IOException("Delete failed");

  }

  def Delete(path: String, recur: Boolean):Unit = {

    if (!recur) {
      if (!new java.io.File(path).delete())
        throw new IOException("Delete failed");
    }
    else
      DeleteRecursive(new java.io.File(path));
  }
}