using TK75Attractions.Monitoring;
using UnityEngine;
using System.Collections.Generic;

public class Bootstrap : MonoBehaviour
{
    static OtelBootstrap otelBootstrap;

    private async void Start()
    {
        List<string> list = new() { "Game.Core" };
        otelBootstrap = new();

        await otelBootstrap.Setup("Hoge", list);
        await otelBootstrap.Activate();

        using (var test = ActivityManager.StartActivity("Game.Core", "startGame"))
        {
            test.SetTag("aaa", 123);
        }
        Debug.Log("Hoge");
    }

    public void OnDestroy()
    {
        otelBootstrap.ForceFlush(10000);  // 10秒待機してデータを送信
        ActivityManager.DisposeSources();
        otelBootstrap.Dispose();
    }
}