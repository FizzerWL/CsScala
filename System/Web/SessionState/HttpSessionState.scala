package System.Web.SessionState;

import com.google.common.cache.CacheBuilder
import java.util.concurrent.TimeUnit
import System.Web.HttpContext
import System.Web.HttpCookie
import java.util.HashMap
import com.google.common.cache.Cache
import java.util.UUID
import System.Net.Dns

object HttpSessionState {
  final val Sessions = CacheBuilder.newBuilder().expireAfterAccess(20, TimeUnit.MINUTES).build().asInstanceOf[Cache[String, HashMap[String, Any]]];

  final val SessionKey = "jses_" + Dns.GetHostName();
}

class HttpSessionState {

  def apply(k: String): Any =
    {

      val ctx = HttpContext.Current;

      val cookie = ctx.Request.Cookies(HttpSessionState.SessionKey);

      if (cookie == null)
        return null;

      val cache = HttpSessionState.Sessions.getIfPresent(cookie.Value);
      if (cache == null)
        return null;

      return cache.get(k);
    }
  def update(k: String, v: Any) {
    val ctx = HttpContext.Current;

    var cookie = ctx.Request.Cookies(HttpSessionState.SessionKey);

    if (cookie == null) {
      cookie = new HttpCookie(HttpSessionState.SessionKey, UUID.randomUUID().toString());
      cookie.HttpOnly = true;
      ctx.Response.Cookies.Add(cookie);
    }

    var cache = HttpSessionState.Sessions.getIfPresent(cookie.Value);
    if (cache == null) {
      cache = new HashMap[String, Any]();
      HttpSessionState.Sessions.put(cookie.Value, cache);
    }

    cache.put(k, v);

  }

  def Renew(ctx: HttpContext) {

    val cookie = ctx.Request.Cookies(HttpSessionState.SessionKey);

    if (cookie != null)
      HttpSessionState.Sessions.getIfPresent(cookie.Value)

  }

  def Remove(k: String) {
    val ctx = HttpContext.Current;

    val cookie = ctx.Request.Cookies(HttpSessionState.SessionKey);

    if (cookie != null) {
      val cache = HttpSessionState.Sessions.getIfPresent(cookie.Value);
      if (cache != null)
        cache.remove(k);
    }
  }
}