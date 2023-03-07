﻿namespace Prestadito.Security.Infrastructure.Data.Constants
{
    public class ConstantSettings
    {
        #region Parametria
        public class Parameter
        {
            public class UserStatus
            {
                public const string STATUS_ACTIVE = "01";
                public const string STATUS_INCOMPLETE_INFORMATION = "02";
                public const string STATUS_LOCK_ATTEMPTS = "03";
            }
        }
        #endregion

        #region Settings
        public class Settings
        {
            public class Parameter
            {
                public class LoginAttempts
                {
                    public const int LOGIN_MAX_ATTEMPTS = 3;
                }
            }
        }
        #endregion
    }
}