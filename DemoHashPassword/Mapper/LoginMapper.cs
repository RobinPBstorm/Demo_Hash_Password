using DemoHashPassword.API.DTOs;
using DemoHashPassword.DL.Entities;

namespace DemoHashPassword.API.Mapper
{
    public static class LoginMapper
    {
        public static Login ToEntity(this LoginDTO dto)
        {
            return new Login
            {
                Username = dto.Username,
                password = dto.Password
            };
        }
    }
}
