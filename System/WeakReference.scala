package System

class WeakReference(obj:Any) {
  final val _weak = new java.lang.ref.WeakReference(obj);

  def IsAlive:Boolean = _weak.get() != null;
  def Target:Any = _weak.get();
}