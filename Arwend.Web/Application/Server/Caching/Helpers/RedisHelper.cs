using StackExchange.Redis;

namespace Arwend.Web.Application.Server.Caching.Helpers
{
    internal sealed class RedisHelper
    {
        private static readonly object AccessLocker = new object();
        private static volatile RedisHelper _Current;

        public ConnectionMultiplexer Redis { get; private set; }


        private RedisHelper()
        {
            if (Redis == null)
            {
                Redis = ConnectionMultiplexer.Connect(ConfigurationManager.GetParameter("RedisForCache") + ",connectTimeout=750");
            }
        }

        public static RedisHelper Current
        {
            get
            {
                if (_Current == null)
                {
                    lock (AccessLocker)
                    {
                        if (_Current == null)
                            _Current = new RedisHelper();
                    }
                }

                return _Current;
            }
        }
    }
}
