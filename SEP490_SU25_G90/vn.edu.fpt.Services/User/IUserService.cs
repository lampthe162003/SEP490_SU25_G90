﻿using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.User
{
    public interface IUserService
    {
        public IList<UserListInformationResponse> GetAllUsers(string? name, string? email);
    }
}
