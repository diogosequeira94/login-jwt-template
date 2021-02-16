﻿using loginjwt.Model;
using loginjwt.Model.ValueObjects;

namespace loginjwt.Repository
{
    public interface IUserRepository
    {
        User ValidateCredentials(UserVO user);
        User ValidateCredentials(string username);
        User RefreshUserInfo(User user);

        
    }
}