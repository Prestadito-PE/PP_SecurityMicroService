namespace Prestadito.Security.Infrastructure.Data.Constants
{
    public class ConstantMessages
    {
        public class Errors
        {
            public class Sessions
            {
                public const string USER_NOT_ACTIVE = "Usuario inactivo";
                public const string INCORRECT_CREDENTIALS = "Credenciales incorrectas";
                public const string USER_LOCKED_BY_MAX_ATTEMPS = "Usuario bloqueado por superar intentos máximos";
            }
            public class Users
            {
                public const string USER_NOT_FOUND = "Usuario no encontrado";
                public const string USER_FAILED_TO_CREATE = "Email no pudo ser creado";
                public const string USER_FAILED_TO_DISABLE = "Usuario no se pudo deshabilitar";
                public const string USER_FAILED_TO_DELETE = "Usuario no pudo eliminar";
                public const string USER_FAILED_TO_UPDATE = "Usuario no se pudo eliminar";
                public const string EMAIL_ALREADY_EXIST = "Email ya esta registrado";
            }

            public class Validator
            {
                public const string PROPERTY_NAME_IS_EMPTY = "{PropertyName} está vacio";
                public const string EMAIL_NOT_VAlID = "{PropertyName} no tiene formato válido";
            }
        }
    }
}
