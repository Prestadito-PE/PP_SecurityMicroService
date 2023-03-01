namespace Prestadito.Security.Infrastructure.Data.Constants
{
    public class Constants
    {
        #region System
        public class System
        {
            public const string SYSTEM_USER = "SYS";

        }
        #endregion
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
    }
}
