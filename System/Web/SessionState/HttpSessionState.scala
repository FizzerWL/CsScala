package System.Web.SessionState;

import com.google.common.cache.CacheBuilder
import java.util.concurrent.TimeUnit
import System.Web.HttpContext
import System.Web.HttpCookie
import java.util.concurrent.ConcurrentHashMap
import com.google.common.cache.Cache
import java.util.UUID
import System.Net.Dns
import WarLight.Shared.Configuration
import WarLight.Shared.EnvironmentEnum

object HttpSessionState {
  final val Sessions = CacheBuilder.newBuilder().expireAfterAccess(20, TimeUnit.MINUTES).build().asInstanceOf[Cache[String, ConcurrentHashMap[String, Any]]];

  final val SessionKey = "jses_" + Dns.GetHostName();
}

class HttpSessionState(ctx:HttpContext) {

  def apply(k: String): Any =
    {
      val cookie = ctx.Request.Cookies(HttpSessionState.SessionKey);

      if (cookie == null)
        return null;

      val cache = HttpSessionState.Sessions.getIfPresent(cookie.Value);
      if (cache == null)
        return null;

      return cache.get(k);
    }
  def update(k: String, v: Any):Unit = {
    var cookie = ctx.Request.Cookies(HttpSessionState.SessionKey);

    if (cookie == null) {
      cookie = new HttpCookie(HttpSessionState.SessionKey, UUID.randomUUID().toString());
      cookie.HttpOnly = true;
      
      if (Configuration.Environment == EnvironmentEnum.Prod)
        cookie.Secure = true;
      
      ctx.Response.Cookies.Add(cookie);
    }

    var cache = HttpSessionState.Sessions.getIfPresent(cookie.Value);
    if (cache == null) {
      cache = new ConcurrentHashMap[String, Any]();
      HttpSessionState.Sessions.put(cookie.Value, cache);
    }

    cache.put(k, v);

  }

  def Renew(ctx: HttpContext):Unit = {

    val cookie = ctx.Request.Cookies(HttpSessionState.SessionKey);

    if (cookie != null)
      HttpSessionState.Sessions.getIfPresent(cookie.Value)

  }

  def Remove(k: String):Unit = {
    val cookie = ctx.Request.Cookies(HttpSessionState.SessionKey);

    if (cookie != null) {
      val cache = HttpSessionState.Sessions.getIfPresent(cookie.Value);
      if (cache != null)
        cache.remove(k);
    }
  }
}