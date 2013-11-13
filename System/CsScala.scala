package System
import scala.util.control._
import java.util.UUID
import java.nio.ByteBuffer
import scala.collection.mutable.ArrayBuffer
import scala.reflect.ClassTag
import java.util.ArrayList
import scala.collection.JavaConverters._

object CsScala 
{
    val cscontinue = new Breaks;
    val csbreak = new Breaks;


    val ushortMaxValue = 65535;
    val ushortMinValue = 0;
    val uintMinValue = 0;
    val uintMaxValue = 4294967295L;


    def Join(seperator:String, strs:Traversable[String]):String =
    {
      return strs.mkString(seperator);
    }
    
    def ToByteArray(uuid:UUID):Array[Byte] = 
    {
      val bb = ByteBuffer.wrap(new Array[Byte](16));
      bb.putLong(uuid.getMostSignificantBits());
      bb.putLong(uuid.getLeastSignificantBits());
      return bb.array();
    }
    
    def TryParseInt(s:String, out:CsRef[Int]):Boolean =
    {
      try
      {
        out.Value = s.toInt;
        return true;
      }
      catch
      {
        case e: NumberFormatException => return false;
      }
    }
    
    def IsNullOrEmpty(s:String):Boolean = 
    {
      return s == null || s.isEmpty();
    }
    
    def GetType(obj:Any):Type =
    {
      return new Type(obj);
    }
    
    def Sort[T](buf:ArrayBuffer[T], cmp:(T,T)=>Int)
    {
      buf.sortWith((f,s) => cmp(f,s) < 0);
    }
    
    def Sort(buf:ArrayBuffer[Int])
    {
      buf.sortBy(a => a);
    }
    
    def Contains[T](a:Traversable[T], item:T):Boolean =
    {
      for(e <- a)
        if (e == item)
          return true;
      return false;
    }
    
    def ToArray[T:ClassTag](buf:ArrayList[T]):Array[T] =
    {
      return buf.toArray().asInstanceOf[Array[T]];
    }
    
    def IsNaN(d:Double):Boolean =
    {
      return d.isNaN();
    }
    
    def IsInfinity(d:Double):Boolean =
    {
      return d.isInfinity;
    }
    
}