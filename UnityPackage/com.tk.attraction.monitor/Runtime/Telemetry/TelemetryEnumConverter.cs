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
    internal static class TelemetryEnumConverter
    {
        internal static ActivityStatusCode GetStatus(SpanStatus status)
        {
            return status switch
            {
                SpanStatus.Unset => ActivityStatusCode.Unset,
                SpanStatus.Ok => ActivityStatusCode.Ok,
                SpanStatus.Error => ActivityStatusCode.Error,
                _ => throw new ArgumentOutOfRangeException(nameof(status))
            };
        }

        internal static ActivityKind GetKind(SpanKind kind)
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
    }
}