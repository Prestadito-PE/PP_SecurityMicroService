using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestadito.Security.Application.Dto.Email
{
    public class EmailViewModel
    {
        public string correo { get; set; }
        public string correocc { get; set; }
        public string correocco { get; set; }
        public Dictionary<string, string> parametros { get; set; }
        public string asunto { get; set; }
        public string plantilla { get; set; }
        public string correoUser { get; set; }
        public string correoPwd { get; set; }
        public string displayName { get; set; }
        public string host { get; set; }
        public string puerto { get; set; }
    }

    public class EmailResponseViewModel
    {
        public List<string> Destinatarios { get; set; }
        public List<string> DestinatariosCC { get; set; }
        public List<string> DestinatariosCCO { get; set; }
        public string Asunto { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string DisplayName { get; set; }
        public Dictionary<string, string> Parametros { get; set; }
        public Dictionary<string, List<string>> ListadoColumnas { get; set; }
        public Dictionary<string, List<List<string>>> ListadoFilas { get; set; }
        public string Plantilla { get; set; }
        public Dictionary<string, byte[]> Attachments { get; set; }
    }
}