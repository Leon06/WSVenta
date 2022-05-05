using WSVenta.Models.Response;
using WSVenta.Models.Request;

namespace WSVenta.Services
{
    public interface IUserService
    {
        UserResponse Auth(AuthRequest model);
    }
}
