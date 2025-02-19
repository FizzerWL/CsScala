package System

class WeakReference[T](obj:Any) {
  final val _weak = new java.lang.ref.WeakReference(obj);

  def IsAlive:Boolean = _weak.get() != null;
  def Target:T = _weak.get().asInstanceOf[T];
  
  def TryGetTarget(result:CsRef[T]):Boolean = {
    val t = _weak.get();
    if (t == null)
      return false;
    result.Value = t.asInstanceOf[T];
    return true;
  }
}