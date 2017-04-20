package System.Web.UI

import System.EventArgs
import System.Web.HttpContext
import com.google.common.cache.CacheBuilder
import System.DateTime
import java.util.concurrent.ConcurrentHashMap
import java.util.concurrent.locks.ReentrantLock
import java.util.ArrayList
import java.util.concurrent.TimeUnit

class ControlCache(html: String, expires: DateTime) {
  val Html = html;
  val Expires = expires;
}

object Control {
  final val Cache = new ConcurrentHashMap[Class[_], ControlCache]();
  final val Locks = new ConcurrentHashMap[Class[_], ReentrantLock]();
}

abstract class Control {

  final var Context: HttpContext = null;

  var Page: Page = null;
  var ID = "";
  var Visible = true;

  def RenderControl(writer: HtmlTextWriter) {
    throw new Exception("RenderControl not overridden by " + this);
  }
  def Render(writer: HtmlTextWriter) {
    if (Visible) {
      OnPreRender(null);
      RenderControl(writer);
    }
  }
  def CachedRender(writer: HtmlTextWriter) {
    if (!Visible)
      return ;

    if (CacheFor == 0 || (Context != null && Context.Request.QueryString("NoCache") != null))
      Render(writer);
    else {

      if (_cache != null)
        writer.Write(_cache.Html);
      else {
        val pos = writer.Position;
        Render(writer);
        val newCache = new ControlCache(writer.GetSubstring(pos), DateTime.Now.AddSeconds(CacheFor));
        Control.Cache.put(getClass(), newCache);
      }
      val lock = CacheLock;
      if (lock.isHeldByCurrentThread())
        lock.unlock();
    }
  }
  def OnInit(args: EventArgs) {}
  def OnLoad(args: EventArgs) {}
  def OnPreRender(args: EventArgs) {}

  def GetControls(): Array[Control];

  def RecurseAll(fn: Control => Unit) {
    if (_cache != null)
      return ;

    fn(this);

    for (c <- GetControls())
      c.RecurseAll(fn);
  }

  def CacheFor: Int = 0;
  def CacheLock: ReentrantLock =
    {
      val c = getClass();
      val lock = Control.Locks.get(c);
      if (lock != null)
        return lock;

      val newLock = new ReentrantLock(true);
      val existing = Control.Locks.putIfAbsent(c, newLock);
      if (existing != null)
        return existing;
      else
        return newLock;

    }

  var _cache: ControlCache = null;
  def DetermineCache(locksOwn: ArrayList[ReentrantLock]) {
    if (CacheFor == 0)
      return ;

    TryGetCache();
    if (_cache == null) {
      val lock = CacheLock;
      if (!lock.tryLock(10000, TimeUnit.MILLISECONDS))
        throw new Exception("Gave up waiting for lock");
      locksOwn.add(lock);
      TryGetCache();
    }
  }

  def TryGetCache() {
    _cache = Control.Cache.get(getClass());
    if (_cache != null && _cache.Expires.Ticks < DateTime.Now.Ticks)
      _cache = null;
  }

  def InitContext(ctx: HttpContext): Control =
    {
      RecurseAll(c => c.Context = ctx);
      return this;
    }
}