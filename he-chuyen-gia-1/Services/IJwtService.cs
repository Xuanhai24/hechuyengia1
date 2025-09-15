using hechuyengia.Models;

namespace hechuyengia.Services
{
    public interface IJwtService
    {
        string CreateToken(User u);
    }
}
