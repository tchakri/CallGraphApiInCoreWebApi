using GraphiAPI.Models;
using System.Threading.Tasks;

namespace GraphiAPI.Services
{
    public interface IGraphService
    {
        Task<GraphUser> GetUserById(string id);

        Task<Chat> CreateOneOnOneChat();
    }
}
