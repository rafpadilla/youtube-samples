using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicroserviceSample.Services
{
    internal interface IPersistenceService
    {
        Task<string> GetLatestIp();
        Task SaveNewIp(string ip);
    }
    internal sealed class PersistenceService : IPersistenceService
    {
        private string FilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "microserviceSample.db");
        private string HistoricFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "microserviceSampleHistoric.db");

        public PersistenceService()
        {
            if (!Directory.Exists("data"))
                Directory.CreateDirectory("data");
        }

        public async Task<string> GetLatestIp()
        {
            using var fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Read);
            using var sr = new StreamReader(fs, Encoding.UTF8);

            var ip = await sr.ReadToEndAsync();

            if (!string.IsNullOrEmpty(ip))
            {
                var lastIp = JsonSerializer.Deserialize<LastIp>(ip);
                return lastIp?.NewIp;
            }
            return null;
        }

        public async Task SaveNewIp(string ip)
        {
            using var fs = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite);
            using var sw = new StreamWriter(fs, Encoding.UTF8);

            if (!string.IsNullOrEmpty(ip))
            {
                var lastIp = JsonSerializer.Serialize(new LastIp() { NewIp = ip, Created = DateTime.Now });
                await sw.WriteLineAsync(lastIp);
            }
            await SaveToHistoricNewIp(ip);
        }

        private async Task SaveToHistoricNewIp(string ip)
        {
            using var fs = new FileStream(HistoricFilePath, FileMode.Append, FileAccess.Write);
            using var sw = new StreamWriter(fs, Encoding.UTF8);

            if (!string.IsNullOrEmpty(ip))
            {
                var lastIp = JsonSerializer.Serialize(new LastIp() { NewIp = ip, Created = DateTime.Now });
                await sw.WriteLineAsync(lastIp);
            }
        }
    }
}
