using System;
using System.Collections.Generic;
using System.Text;

namespace MetroWebApi.Models.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public UserDto(string id, string email, string username)
        {
            Id = id;
            Email = email;
            UserName = username;
        }
    }
}
