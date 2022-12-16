using Newtonsoft.Json;
using Sat.Recruitment.Api.DataAccess.Interfaces;
using Sat.Recruitment.Api.Models.User;
using Sat.Recruitment.Api.Repositories.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.DataAccess
{
    public class UserJsonRepository : IUserJsonRepository
    {
        private readonly IFileManagerJson _fileManager;

        public UserJsonRepository(IFileManagerJson fileManager)
        {
            _fileManager = fileManager;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await ReadUsersFromFile();
        }

        public async Task Insert(User user)
        {
            var fileStream = await _fileManager.ReadJsonFile();

            using StreamReader reader = new StreamReader(fileStream);

            var jsonFile = reader.ReadToEnd();

            fileStream.Close();

            var users = JsonConvert.DeserializeObject<List<User>>(jsonFile);

            users.Add(user);

            await WriteUsersToJsonFile(users);
        }

        public async Task<bool> UserExists(User user)
        {
            var users = await GetAll();
            
            return users.Any(u =>
                u.Email.Equals(user.Email) ||
                u.Phone.Equals(user.Phone) ||
                (u.Name.Equals(user.Name) && u.Address.Equals(user.Address)));
        }

        private async Task<List<User>> ReadUsersFromFile()
        {
            List<User> users;

            using (var fileStream = await _fileManager.ReadJsonFile())
            {
                using StreamReader reader = new StreamReader(fileStream);

                var jsonFile = reader.ReadToEnd();

                fileStream.Close();

                users = JsonConvert.DeserializeObject<List<User>>(jsonFile);
            }

            return await Task.FromResult(users);
        }

        private async Task<bool> WriteUsersToJsonFile(IList<User> users)
        {
            return await _fileManager.WriteJsonFile(users);
        }
    }
}

