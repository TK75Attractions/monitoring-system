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
        public static Activity StartActivity(string sourceName, string name, SpanKind kind = SpanKind.Internal)
        {
            return GetSource(sourceName)
                .StartActivity(name, GetKind(kind))
                ?? throw new InvalidOperationException("No ActivityListener registered");
        }

        private static ActivityKind GetKind(SpanKind kind)
        {
            return kind switch
            {
                SpanKind.Internal => ActivityKind.Internal,
                SpanKind.Client => ActivityKind.Client,
                SpanKind.Server => ActivityKind.Server,
                SpanKind.Producer => ActivityKind.Producer,
                SpanKind.Consumer => ActivityKind.Consumer,
                _ => throw new ArgumentOutOfRangeException(nameof(kind))
            };
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