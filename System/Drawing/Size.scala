package System.Drawing

class Size(w: Int = 0, h: Int = 0) {
  final var Width: Int = w;
  final var Height: Int = h;
  
  override def equals(other: Any): Boolean =
    {
      if (!other.isInstanceOf[Size])
        return false;
      val o = other.asInstanceOf[Size];
      return o.Width == Width && o.Height == Height;
    }
  override def hashCode(): Int = Width + Height;
}