using System.Diagnostics;
using System;
using OpenTelemetry.Trace;
#nullable enable

namespace TK75Attractions.Monitoring
{
    public class Span : IDisposable
    {
        private readonly Activity _activity;

        internal Span(
            Activity activity
        )
        {
            _activity = activity;
        }
        public void AddEvent(string eventName)
        {
            ActivityEvent activityEvent = new(eventName);
            _activity.AddEvent(activityEvent);
        }
        public void SetStatus(SpanStatus status) => _activity.SetStatus(GetStatus(status));
        public void SetTag(string key, object? content) => _activity.SetTag(key, content);
        public void RecordException(Exception ex) => _activity.RecordException(ex);
        public void Dispose() => _activity.Dispose();

        private ActivityStatusCode GetStatus(SpanStatus status)
        {
            return status switch
            {
                SpanStatus.Unset => ActivityStatusCode.Unset,
                SpanStatus.Ok => ActivityStatusCode.Ok,
                SpanStatus.Error => ActivityStatusCode.Error,
                _ => throw new ArgumentOutOfRangeException(nameof(status))
            };
        }
    }
}