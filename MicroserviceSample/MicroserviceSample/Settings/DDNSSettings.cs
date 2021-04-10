using System.Collections.Generic;

namespace MicroserviceSample.Settings
{
    internal sealed class DDNSSettings
    {
        public string Name { get; set; }
        public string ZoneIdentifier { get; set; }
        public IEnumerable<Record> Records { get; set; }
    }
    internal sealed class Record
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Ttl { get; set; } = 1;
        public bool proxied { get; set; } = false;
    }
}
