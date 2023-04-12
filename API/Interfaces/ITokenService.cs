
using API.Entities;

namespace API.Interfaces
{
    // Any class which inherits this interface will have to implement a method called CreateToken
    // and that method will return a string and it will take AppUser as an argument
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}