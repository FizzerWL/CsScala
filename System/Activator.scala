package System

import raksha.RakshaRoot

object Activator {

  def CreateInstance(t: System.Type): Any = {
    val clazz = t.clazz
    val constructor = clazz.getConstructor()
    constructor.newInstance()
  }

  def CreateInstance(t: System.Type, args:Array[AnyRef]): Any = {
    val clazz = t.clazz
    val constructor = clazz.getConstructor()
    constructor.newInstance(args:_*)
  }

  def CreateInstance[T]()(implicit m:Manifest[T]): T = {
    val clazz = m.runtimeClass
    val constructor = clazz.getConstructor()
    constructor.newInstance().asInstanceOf[T]
  }

  def CreateInstance[T](args:Array[AnyRef])(implicit m:Manifest[T]): T = {
    val clazz = m.runtimeClass
    val constructor = clazz.getConstructor()
    constructor.newInstance(args:_*).asInstanceOf[T]
  }
}
