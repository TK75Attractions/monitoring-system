using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TK75Attractions.Monitoring
{
    internal interface ISpan
    {

        public ActivityContext ActivityContext { get; }
        
        public void AddTag(
            string name,
            object content
        );

        public void AddEvent(
            string name,
            Dictionary<string, object> attributes = null
        );

        public void SetError(Exception exception);

        public void SetStatus(
            ActivityStatusCode statusCode,
            string description = null
        );

        public void Dispose();
    }
}