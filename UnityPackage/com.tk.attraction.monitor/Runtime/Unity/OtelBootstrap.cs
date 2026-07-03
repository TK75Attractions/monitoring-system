using Microsoft.Extensions.Logging;
using UnityEngine;
using OpenTelemetry;
using System;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Exporter.OpenTelemetryProtocol;
using OpenTelemetry.Logs;
using System.Collections.Generic;
using OpenTelemetry.Exporter;
using System.Threading.Tasks;

namespace TK75Attractions.Monitoring
{
    public class OtelBootstrap : IDisposable
    {
        private TracerProvider traceProvider;
        
        private List<string> _sources = new();
        private string _ip = "localhost";
        private string _name = "HogeHoge";
        private ProviderCondition condition = ProviderCondition.BeforeSetup;

        public async Task Setup(
            string name,
            string ip,
            List<string> sources
        )
        {
            if (condition != ProviderCondition.BeforeSetup) return;

            _name = name;
            _ip = ip;
            _sources = sources;
            condition = ProviderCondition.BeforeActivate;
            return;
        }

        public async Task Activate()
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
                    option.Endpoint = new Uri($"http://{_ip}:4318/v1/traces");
                    option.Protocol = OtlpExportProtocol.HttpProtobuf;
                });            // 送信先

            var loggerFactory = LoggerFactory.Create(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();

                loggingBuilder.AddOpenTelemetry(logging =>
                {
                    logging.SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(_name)  // "your-service" → _name に変更
                    );

                    logging.IncludeFormattedMessage = true;
                    logging.IncludeScopes = true;

                    logging.AddOtlpExporter(otlp =>
                    {
                        otlp.Endpoint = new Uri($"http://{_ip}:4318/v1/logs");  // localhost → _ip に変更
                        otlp.Protocol = OtlpExportProtocol.HttpProtobuf;  // プロトコルを明示的に指定
                    });
                });
            });

            foreach (var source in _sources)
            {
                builder.AddSource(source);
            }

            try
            {
                traceProvider = builder.Build();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex);
            }

            ActivityManager.Initialize(traceProvider,loggerFactory ,_sources);

            condition = ProviderCondition.Activated;
            return;
        }

        public void ForceFlush(int timeoutMilliseconds = 10000)
        {
            if (traceProvider == null) return;
            traceProvider.ForceFlush(timeoutMilliseconds);
        }

        public void Dispose()
        {
            traceProvider?.Dispose();
            condition = ProviderCondition.BeforeActivate;
        }
    }
}