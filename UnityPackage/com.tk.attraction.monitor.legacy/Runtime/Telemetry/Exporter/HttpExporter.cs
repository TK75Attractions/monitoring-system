using System.Diagnostics;
using OpenTelemetry;
using System.Collections;

namespace TK75Attractions.Monitoring
{
    public class HttpExporter : BaseExporter<Activity>
    {
        public override ExportResult Export(in Batch<Activity> batch)
        {
            foreach (var activity in batch)
            {
                Send(activity);
            }

            return ExportResult.Success;
        }

        private void Send(Activity activity)
        {
            
        }
    }
}