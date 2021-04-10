using System;
using System.Collections.Generic;

namespace MicroserviceSample.Models
{
    internal sealed class Data
    {
    }

    internal sealed class Meta
    {
        public bool auto_added { get; set; }
        public string source { get; set; }
    }

    internal sealed class Result
    {
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public bool proxiable { get; set; }
        public bool proxied { get; set; }
        public int ttl { get; set; }
        public bool locked { get; set; }
        public string zone_id { get; set; }
        public string zone_name { get; set; }
        public DateTime created_on { get; set; }
        public DateTime modified_on { get; set; }
        public Data data { get; set; }
        public Meta meta { get; set; }
    }

    internal sealed class DnsRecordCloudflareGetDto
    {
        public bool success { get; set; }
        public List<object> errors { get; set; }
        public List<object> messages { get; set; }
        public IEnumerable<Result> result { get; set; }
    }

    internal sealed class DnsRecordCloudflare
    {
        public bool success { get; set; }
        public List<object> errors { get; set; }
        public List<object> messages { get; set; }
        public Result result { get; set; }
    }
}
