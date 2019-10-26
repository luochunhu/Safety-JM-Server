using Basic.Cache;

namespace Sys.Safety.Cache.Person
{
    /// <summary>
    /// KJ237缓存
    /// </summary>
    public class KJ237CacheHelper
    {
        public static ICacheManager Cache = CacheFactory.Create(CacheType.Local);
        static KJ237CacheHelper()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["MongoDBEnable"].ToLower() == "true")
            {
                Cache = CacheFactory.Create(CacheType.MongoDb);
            }
            else
            {
                Cache = CacheFactory.Create(CacheType.Local);
            }
        }
    }
}
