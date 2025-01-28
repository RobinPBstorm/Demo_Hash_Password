using DemoHashPassword.DL.Entities;
using DemoHashPassword.DTOs;

namespace DemoHashPassword.API.Mapper
{
    public static class UserMapper
    {
        public static User ToEntity(this UserRegisterForm form)
        {
            return new User
            {
                Username = form.Username,
                Firstname = form.Firstname,
                Name = form.Name
            };
        }

        public static UserFullDTO ToFullDTO(this User entity)
        {
            return new UserFullDTO
            {
                Id = entity.Id,
                Username = entity.Username,
                Name = entity.Name,
                Firstname = entity.Firstname
            };
        }
    }
}
