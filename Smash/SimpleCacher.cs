using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Smash
{
    class SimpleCacher : ICacher
    {
        private const string CacheKey = "SmashStore";

        private Dictionary<Key, object> _store;

        public void Save(Key key, object value)
        {
            Store[key] = value;
        }

        public bool TryRetrieve(Key key, out object value)
        {
            return Store.TryGetValue(key, out value);
        }

        private Dictionary<Key, object> Store
        {
            get { return _store ?? (_store = GetStore()); }
        }

        private static Dictionary<Key, object> GetStore()
        {
            var newStore = new Dictionary<Key, object>();

            var savedStored = MemoryCache.Default.AddOrGetExisting(
                CacheKey,
                newStore,
                ObjectCache.InfiniteAbsoluteExpiration);

            return (savedStored as Dictionary<Key, object>) ?? newStore;
        }
    }
}
