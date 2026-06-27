using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace TK75Attractions.Monitoring
{
    internal class Span : IDisposable, ISpan
    {
        private readonly Activity _activity;
        private bool isDisposed = false;
        public ActivityContext ActivityContext => _activity.Context;

        public Span(Activity activity)
        {
            _activity = activity;
        }
        public void AddTag(
            string name,
            object content
        )
        {
            
            _activity.SetTag(name, content);
        }

        public void AddEvent(
            string name,
            Dictionary<string, object> attributes = null
        )
        {
            ActivityTagsCollection collection = attributes == null ? new() : new(attributes);
            ActivityEvent activityEvent = new ActivityEvent(name,DateTime.Now, collection);

            _activity.AddEvent(activityEvent);
        }

        public void SetError(Exception exception) => SetStatus(ActivityStatusCode.Error, exception.Message);
        public void SetStatus(
            ActivityStatusCode statusCode,
            string description = null
        )
        {
            _activity.SetStatus(statusCode, description);
        }

        public void Dispose()
        {
            if(isDisposed) return;
            isDisposed = true;
            _activity.Dispose();
        }
    }
}