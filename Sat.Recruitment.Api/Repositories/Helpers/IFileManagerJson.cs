using Sat.Recruitment.Api.Models.User;
using System.Collections.Generic;
using System.IO;

namespace Sat.Recruitment.Api.Repositories.Helpers
{
    public interface IFileManagerJson :IFileManager<FileStream, IEnumerable<User>>
    {
    }
}
