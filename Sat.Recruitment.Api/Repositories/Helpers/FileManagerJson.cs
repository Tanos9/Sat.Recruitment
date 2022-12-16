using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sat.Recruitment.Api.Models.User;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Repositories.Helpers
{
    public class FileManagerJson : IFileManagerJson
    {
        private readonly IConfiguration _configuration;

        public FileManagerJson(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<FileStream> ReadJsonFile()
        {
            var path = _configuration.GetValue<string>("JsonFilePath");

            var filestream = new FileStream(path, FileMode.Open);

            return await Task.FromResult(filestream);
        }

        public async Task<bool> WriteJsonFile(IEnumerable<User> users)
        {
            string json = JsonConvert.SerializeObject(users);
            var path = _configuration.GetValue<string>("JsonFilePath");

            File.WriteAllText(path, json);
            return await Task.FromResult(true);
        }
    }
}
