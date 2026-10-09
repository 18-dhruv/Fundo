using ModelLayer.Models.Users;
using RepositoryLayer.Entity;

namespace BusinessLayer.Mappers
{
    /// <summary>
    /// Entity -> DTO. The one place that decides what the API is allowed to see about a user.
    /// </summary>
    public static class UserMapper
    {
        public static UserResponseModel ToResponse(this UserEntity user) => new()
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ContactNo = user.ContactNo
        };
    }
}
