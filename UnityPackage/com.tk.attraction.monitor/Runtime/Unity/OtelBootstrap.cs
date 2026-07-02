using Microsoft.Extensions.Logging;
using UnityEngine;
using OpenTelemetry;
using System;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using System.Diagnostics;
using System.Collections.Generic;

namespace TK75Attractions.Monitoring
{
    public class OtelBootstrap : IDisposable
    {
        private TracerProvider traceProvider;
        
        private List<string> _sources = new();
        private string _name = "HogeHoge";
        private ProviderCondition condition = ProviderCondition.BeforeSetup;

        public void Setup(
            string name,
            List<string> sources
        )
        {
            if (condition != ProviderCondition.BeforeSetup) return;

            _name = name;
            _sources = sources;
            condition = ProviderCondition.BeforeActivate;
        }

        public void Activate()
        {
            if(condition != ProviderCondition.BeforeActivate) return;
            UnityEngine.Debug.Log("aaa");
            TracerProviderBuilder builder = Sdk
                .CreateTracerProviderBuilder()
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                    .AddService(_name
                ))         // サービス情報
                .AddOtlpExporter(option =>
                {
                    option.Endpoint = new Uri("http://localhost:4317");
                });            // 送信先

            foreach (var source in _sources)
            {
                builder.AddSource(source);
            }
            
            traceProvider = builder.Build();

            condition = ProviderCondition.Activated;
        }

        public void Dispose()
        {
            traceProvider?.Dispose();
            condition = ProviderCondition.BeforeActivate;
        }
    }
}