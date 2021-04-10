namespace MicroserviceSample.Settings
{
    internal sealed class ProcessSettings
    {
        public int TimerIntervalSecs { get; set; }
        public string ExternalIpApiEndpoint { get; set; }
        public bool EnableTimer { get; set; }
    }
}
