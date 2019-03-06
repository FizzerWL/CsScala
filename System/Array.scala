package System

import scala.reflect.ClassTag

object CsArray
{
  def IndexOf[T](array : Array[T], item: T): Int =
  {
    array.indexOf(item)
  }

  def Resize[T:ClassTag](array : CsRef[Array[T]], newLength : Int): Unit =
  {
    var copy: Array[T] = new Array[T](newLength)
    array.Value.copyToArray(copy, 0, math.min(array.Value.length, newLength))
    array.Value = copy
  }

  def FindAll[T](array : Array[T], fn: T => Boolean): Array[T] =
  {
    array.filter(fn)
  }

  def FindIndex[T](array : Array[T], fn: T => Boolean): Int =
  {
    array.indexWhere(fn)
  }

  def Copy[T](original : Array[T], target : Array[T], length : Int): Unit =
  {
    original.copyToArray(target, 0, length)
  }

  def Copy[T](sourceArray : Array[T], sourceIndex : Int, destinationArray : Array[T], destinationIndex : Int, length : Int): Unit =
  {
    java.lang.System.arraycopy(sourceArray, sourceIndex, destinationArray, destinationIndex, length)
  }

  def sort[T:Ordering](array : Array[T]): Unit =
  {
    scala.util.Sorting.quickSort(array)
  }

  def sort[T](array : Array[T], fn: (T, T) => Int): Unit =
  {
    val copy = array.sortWith((a, b) => fn(a , b) < 0)
    Array.copy(copy, 0, array, 0, array.length)
  }

  def Exists[T](array : Array[T], fn: T => Boolean): Boolean = {
    array.exists(fn)
  }
/*
Doesn't work :(
  def ConvertAll[T, F](array : Array[T], fn: T => F): Array[F] = {
    System.Linq.Enumerable.ToArray[F](System.Linq.Enumerable.Select(array, fn))
  }*/
}
