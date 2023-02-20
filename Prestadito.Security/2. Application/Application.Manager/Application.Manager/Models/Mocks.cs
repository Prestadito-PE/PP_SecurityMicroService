namespace Prestadito.Security.Application.Manager.Models
{
    public class Mocks
    {
        public static List<ParameterEntity> Rols
        {
            get
            {
                return new List<ParameterEntity>()
                {
                    new ParameterEntity() {  StrCode = "ADM", StrDescription = "Administrador" },
                    new ParameterEntity() {  StrCode = "CLI", StrDescription = "Cliente" }
                };
            }
        }

        public static List<ParameterEntity> UserStatus
        {
            get
            {
                return new List<ParameterEntity>()
                {
                    new ParameterEntity() { StrCode = "0" , StrDescription = "Usuario activo" },
                    new ParameterEntity() { StrCode = "1" , StrDescription = "Usuario bloqueado, intentos de inicio de sesion superados" },
                    new ParameterEntity() { StrCode = "2" , StrDescription = "Usuario bloqueado, por incumplimiento de normas" }
                };
            }
        }

        public static ParameterEntity? GetRolByCode(string code)
        {
            return Rols.FirstOrDefault(r => r.StrCode == code);
        }

        public static ParameterEntity? GetUserStatus(string code)
        {
            return UserStatus.FirstOrDefault(r => r.StrCode == code);
        }
    }
}
