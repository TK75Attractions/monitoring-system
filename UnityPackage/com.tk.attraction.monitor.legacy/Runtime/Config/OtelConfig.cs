using UnityEngine;
using System.Collections.Generic;

namespace TK75Attractions.Monitoring
{
    [CreateAssetMenu(fileName = "OtelConfig", menuName = "Monitor/OtelConfig")]
    public class OtelConfig : ScriptableObject
    {
        public string GameName;
        public string IP;
        public List<string> ActivitySources;
        public Connection connection;

    }

    public enum Connection
    {
        HTTP,
        gRPC //未実装
    }
}