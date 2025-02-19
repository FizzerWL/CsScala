package System.Diagnostics

object Debug {
  def Assert(b:Boolean) =
  {
    if (!b)
      throw new Exception("Assert failed");
  }
}