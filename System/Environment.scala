package System

import java.util.Date

object Environment 
{
  def TickCount:Int =
  {
    return new Date().getTime().toInt;
  }

}