package System.Runtime.Caching

import System.Collections.ObjectModel.Collection
import System.TimeSpan
import System.DateTime

class CacheItemPolicy 
{
  val ChangeMonitors = new System.Collections.Generic.List[ChangeMonitor]();
  var SlidingExpiration:TimeSpan = null;
  var AbsoluteExpiration:DateTime = null;
}