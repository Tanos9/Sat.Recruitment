using Newtonsoft.Json;
using Sat.Recruitment.Api.DataAccess.Interfaces;
using Sat.Recruitment.Api.Models.User;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.DataAccess
{
    public class UserJsonRepository : IUserJsonRepository
    {
        public async Task<IEnumerable<User>> GetAll()
        {
            return await ReadUsersFromFile();
        }

        public async Task Insert(User user)
        {
            var fileStream = ReadJsonFile();

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
            
            var userExists = users.Any(u =>
                u.Email.Equals(user.Email) ||
                u.Phone.Equals(user.Phone) ||
                (u.Name.Equals(user.Name) && u.Address.Equals(user.Address)));

            return userExists;
        }

        private Task<List<User>> ReadUsersFromFile()
        {
            List<User> users;

            using (var fileStream = ReadJsonFile())
            {
                using StreamReader reader = new StreamReader(fileStream);

                var jsonFile = reader.ReadToEnd();

                fileStream.Close();

                users = JsonConvert.DeserializeObject<List<User>>(jsonFile);
            }

            return Task.FromResult(users);
        }

        private FileStream ReadJsonFile()
        {
            var path = Directory.GetCurrentDirectory() + "./Files/Users_Refactor.json"; //TODO: Add to settings

            return  new FileStream(path, FileMode.Open);
        }

        private Task<bool> WriteUsersToJsonFile(IList<User> users)
        {
            string json = JsonConvert.SerializeObject(users);
            File.WriteAllText("./Files/Users_Refactor.json", json);

            return Task.FromResult(true);
        }
    }
}

