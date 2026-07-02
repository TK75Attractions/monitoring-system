using Microsoft.Extensions.Logging;
using OpenTelemetry;
using System;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TK75Attractions.Monitoring
{
    public static class ActivityManager
    {
        private static readonly ConcurrentDictionary<string, ActivitySource> _sources = new();
        public static Span StartActivity(string sourceName, string name, SpanKind kind = SpanKind.Internal)
        {
            Activity activity = GetSource(sourceName)
                .StartActivity(name, TelemetryEnumConverter.GetKind(kind))
                ?? throw new InvalidOperationException("No ActivityListener registered");
            return new Span(activity);
        }

        private static ActivitySource GetSource(string name) => _sources.GetOrAdd(name,static n => new ActivitySource(n));

        public static void DisposeSources()
        {
            foreach (var value in _sources.Values)
            {
                value.Dispose();
            }
            _sources.Clear();
        }
    }
}