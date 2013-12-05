package System.Collections.Generic

trait IComparer[T] {

  def Compare(x:T, y:T):Int;
}