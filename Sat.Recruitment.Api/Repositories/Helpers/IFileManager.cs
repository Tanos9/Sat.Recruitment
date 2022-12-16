using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Repositories.Helpers
{
    public interface IFileManager<T, W> where T : class where W : class
    {
        Task<T> ReadJsonFile();
        Task<bool> WriteJsonFile(W jsonObject);
    }
}
