using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace TK75Attractions.Monitoring
{
    internal class ActivitySourceProvider
    {
        private readonly string _prefix;
        ConcurrentDictionary<string, ActivitySource> _activitySource = new();

        public ActivitySourceProvider(string prefix)
        {
            _prefix = prefix;
        }

        public ActivitySource GetActivitySource(string key)
        {
            string normalized = $"{_prefix} + . +{key}";
            var result = _activitySource.GetOrAdd(normalized, k => new ActivitySource(k));
            return result;
        }   
    
        public void Dispose()
        {
            foreach (var source in _activitySource.Values)
            {
                source.Dispose();
            }
        }
    }
}