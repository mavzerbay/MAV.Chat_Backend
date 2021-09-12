using MAV.Chat.Core.Entities;
using System.Threading.Tasks;

namespace MAV.Chat.Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(MavUser user);
    }
}
