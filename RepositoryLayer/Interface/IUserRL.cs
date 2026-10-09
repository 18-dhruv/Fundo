using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{

    public interface IUserRL
    {
        Task<UserEntity> AddAsync(UserEntity user);

        Task<UserEntity?> GetByIdAsync(int userId);

        Task<UserEntity?> GetByEmailAsync(string email);

        Task<UserEntity?> GetByResetTokenHashAsync(string tokenHash);

        Task<List<UserEntity>> GetAllAsync();

        Task<bool> EmailExistsAsync(string email);

        Task UpdateAsync(UserEntity user);

        Task<bool> DeleteAsync(int userId);
    }
}
