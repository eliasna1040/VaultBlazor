using DAL.Models;

namespace Services;

public interface IUserService
{
    Task<User?> CreateUser(User user);
    Task<User?> Login(User user);
}