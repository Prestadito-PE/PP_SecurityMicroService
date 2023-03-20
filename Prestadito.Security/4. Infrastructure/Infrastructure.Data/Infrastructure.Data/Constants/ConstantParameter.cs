namespace Prestadito.Security.Infrastructure.Data.Constants
{
    public class ConstantSettings
    {
        #region Parametria
        public class Parameter
        {
            public class UserStatus
            {
                public const string STATUS_ACTIVE = "01";
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
