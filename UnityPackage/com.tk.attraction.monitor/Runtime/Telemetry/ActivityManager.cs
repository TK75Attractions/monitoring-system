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
        private static TracerProvider _tracerProvider;
        private static bool isInit = false;

        internal static void Initialize(TracerProvider tracerProvider, List<string> sourceNames)
        {
            if(isInit) return;
            _tracerProvider = tracerProvider;
            
            foreach (var source in sourceNames)
            {
                _ = GetSource(source);
            }
            isInit = true;
        }

        public static Span StartActivity(string sourceName, string name, SpanKind kind = SpanKind.Internal)
        {
            if (!isInit) throw new Exception("Called before Init");
            Activity activity = GetSource(sourceName)
                .StartActivity(name, TelemetryEnumConverter.GetKind(kind))
                ?? throw new InvalidOperationException("No ActivityListener registered");
            
            UnityEngine.Debug.Log($"Activity started: {activity.Id}");
            return new Span(activity);
        }

        private static ActivitySource GetSource(string name) => _sources.GetOrAdd(name,static n => new ActivitySource(n));
        
        public static void ForceFlush() => _tracerProvider.ForceFlush();
        public static void DisposeSources()
        {
            _tracerProvider.ForceFlush();
            
            foreach (var value in _sources.Values)
            {
                value.Dispose();
            }
            _sources.Clear();
        }
    }
}