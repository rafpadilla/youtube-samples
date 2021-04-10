using MicroserviceSample.Models;
using MicroserviceSample.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceSample.Services
{
    internal interface ICloudflareService
    {
        Task CheckIpAndUpdateDDNSIfNeeded(CancellationToken cancellationToken);
    }
    internal sealed class CloudflareService : ICloudflareService
    {
        private readonly ILogger<CloudflareService> _logger;
        private readonly ProcessSettings _settings;
        private readonly CloudflareSettings _cloudflareSettings;
        private readonly IEnumerable<DDNSSettings> _dDNSSettings;
        private readonly IPersistenceService _persistenceService;

        public CloudflareService(ILogger<CloudflareService> logger, IOptions<ProcessSettings> settings, IOptions<CloudflareSettings> cloudflareSettings,
            IEnumerable<DDNSSettings> ddnsConfig, IPersistenceService persistenceService)
        {
            _logger = logger;
            _settings = settings.Value;
            _cloudflareSettings = cloudflareSettings.Value;
            _dDNSSettings = ddnsConfig;
            _persistenceService = persistenceService;
        }
        public async Task CheckIpAndUpdateDDNSIfNeeded(CancellationToken cancellationToken)
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_cloudflareSettings.Token}");

            var ip = await GetExternalIpAddress(httpClient, cancellationToken);

            _logger.LogDebug($"External IP found: {ip}");

            var lastIp = await _persistenceService.GetLatestIp();
            if (string.IsNullOrEmpty(lastIp) || !lastIp.Equals(ip.ToString()))
            {
                _logger.LogInformation("Starting update on Cloudflare");
                await UpdateCloudflareDDNSIP(httpClient, ip, cancellationToken);

                _logger.LogInformation("Saving IP to file");
                await _persistenceService.SaveNewIp(ip.ToString());

                _logger.LogInformation("Update completed successfully");
            }
            else
            {
                _logger.LogInformation($"Last IP matches new one {ip}, skipping update.");
            }
        }
        private async Task<IPAddress> GetExternalIpAddress(HttpClient httpClient, CancellationToken cancellationToken)
        {
            var externalIp = await httpClient.GetStringAsync(_settings.ExternalIpApiEndpoint, cancellationToken);

            if (IPAddress.TryParse(externalIp, out IPAddress ip))
            {
                return ip;
            }
            _logger.LogError("Error on fetching external IP");

            throw new FormatException($"Error when parsing IP: '{externalIp}'");
        }
        private async Task UpdateCloudflareDDNSIP(HttpClient httpClient, IPAddress ip, CancellationToken cancellationToken)
        {
            foreach (var dns in _dDNSSettings)
            {
                _logger.LogInformation($"updating host: {dns.Name}");
                var identifiers = await GetDnsIdentifiers(httpClient, dns, cancellationToken);
                _logger.LogInformation($"DNS identifiers found. Updating {dns.Records.Count()} records");
                foreach (var ddns in dns.Records)
                {
                    var identifier = identifiers.FirstOrDefault(a => a.type.Equals(ddns.Type) && a.name.Equals(ddns.Name));

                    if (identifier == null)
                        _logger.LogError($"No identifier matches with DNS for record: [{ddns.Type}]-{ddns.Name}");

                    var url = $"https://api.cloudflare.com/client/v4/zones/{dns.ZoneIdentifier}/dns_records/{identifier.id}";
                    ddns.Content = ddns.Content.Replace("{ip}", ip.ToString());
                    var stringContent = new StringContent(JsonSerializer.Serialize(ddns), Encoding.UTF8, "application/json");

                    var response = await httpClient.PutAsync(url, stringContent, cancellationToken);
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync(cancellationToken);
                        var dnsInfo = JsonSerializer.Deserialize<DnsRecordCloudflare>(json);
                        if (dnsInfo.success)
                        {
                            _logger.LogInformation($"DNS record updated successfully [{ddns.Type}]-{ddns.Name}");
                            continue;
                        }
                    }
                    _logger.LogError($"Error updating DNS record [{ddns.Type}]-{ddns.Name}");

                    throw new InvalidOperationException("Error updating DNS records from Cloudflare");
                }
            }
        }
        private async Task<IEnumerable<Result>> GetDnsIdentifiers(HttpClient httpClient, DDNSSettings dns, CancellationToken cancellationToken)
        {
            var url = $"https://api.cloudflare.com/client/v4/zones/{dns.ZoneIdentifier}/dns_records";
            var response = await httpClient.GetAsync(url, cancellationToken);
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var dnsInfo = JsonSerializer.Deserialize<DnsRecordCloudflareGetDto>(json);
                return dnsInfo.result;
            }
            _logger.LogError($"Error trying to read DNS information for DNS: {dns.Name}, if this error oftenly appears, check configuration file");

            throw new InvalidOperationException("Error trying to read DNS records from Cloudflare");
        }
    }
}
