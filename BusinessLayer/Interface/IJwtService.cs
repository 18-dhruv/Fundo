using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IJwtService
    {
        (string Token, DateTime ExpiresAt) GenerateToken(UserEntity user);
    }
}
