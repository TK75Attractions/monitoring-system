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
        public ISpan StartSpan(string instrucmentationName, string spanName, ISpan parent = null)
        {
            ActivitySource activitySource = sourceProvider.GetActivitySource(instrucmentationName);

            var parentContext = GetParentContext(parent);

            var activity = activitySource.StartActivity(spanName,ActivityKind.Internal,parentContext);

            return activity == null ? new NoOpSpan() : new Span(activity);
        }

        private ActivityContext GetParentContext(ISpan parent = null)
        {
            return parent?.ActivityContext ?? Activity.Current?.Context ?? default;
        }
    }
}