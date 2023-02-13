namespace Prestadito.Security.Application.Manager.Models
{
    public class MockRol
    {
        public static List<Rol> GetRols()
        {
            return new List<Rol>()
            {
                new Rol() {  StrCode = "ADM", StrDescription="ADMINISTRATOR"},
                new Rol() {  StrCode = "CLI", StrDescription="CLIENT" }
            };
        }

        public static Rol? GetRolByCode(string code)
        {
            var rols = GetRols();

            return rols.FirstOrDefault(r => r.StrCode == code);
        }
    }
}
