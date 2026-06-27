using System.Diagnostics;
using System.Collections.Generic;

namespace TK75Attractions.Monitoring
{
    internal class Tracer
    {
        private ActivitySourceProvider sourceProvider;

        public Tracer(string prefix)
        {
            sourceProvider = new (prefix);
        }

        public ISpan StartSpan(string name, string spanName, Span parent = null)
        {
            ActivitySource activitySource = sourceProvider.GetActivitySource(name);

            var parentContext = GetParentSource(parent);

            var activity = activitySource.StartActivity(spanName,ActivityKind.Internal,parentContext);
            ISpan result;
            if(activity == null)
            {
                result = new NoOpSpan();
            }
            else
            {
                result = new Span(activity);
            }

            return result;
        }

        private ActivityContext GetParentSource(Span parent = null)
        {
            return parent?.ActivityContext ?? Activity.Current?.Context ?? default;
        }
    }
}