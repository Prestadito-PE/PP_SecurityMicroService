namespace Prestadito.Security.Infrastructure.Data.Constants
{
    public class ConstantAPI
    {
        #region System
        public class System
        {
            public const string SYSTEM_USER = "SYS";
        }
        #endregion
        #region Parameters
        public class MicroSetting
        {
            public const string PARAMETER_MAX_ATTEMPS_CODE = "0201";
        }
        #endregion
        #region Endpoint
        public class Endpoint
        {
            public class Tag
            {
                public const string USERS = "USERS";
                public const string SESSIONS = "SESSIONS";
                public const string INTERSERVICE = "INTERSERVICE";
            }
        }
        #endregion
    }
}
