﻿namespace Offwind.WebApp.Models.Account
{
    public class SystemRole
    {
        public const string Admin = "Admin";
        public const string Partner = "Partner";
        public const string User = "User";
    }

    public enum SystemRoleType
    {
        Admin,
        Partner,
        User
    }
}
