﻿using System;

namespace Common.Security.Authorization
{
    public class CustomClaimTypes
    {
        public static readonly string userId = nameof(userId);
        public static readonly string userName = nameof(userName);
        public static readonly string nationalCode = nameof(nationalCode);

        public static Guid UserId { get; set; }

        public static string UserName { get; set; }

        public static string NationalCode { get; set; }
    }
}
