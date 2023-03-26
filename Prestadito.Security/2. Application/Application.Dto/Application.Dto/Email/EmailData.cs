using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestadito.Security.Application.Dto.Email
{
    public class EmailData<T>
    {
        public int EmailType { get; set; }                  //1: Email Bienvenida       2: Email Reset pwd
        public T Model { get; set; }
        public List<string> SubjectData { get; set; }
        public List<string> EmailList { get; set; }
        public string HtmlTemplateName { get; set; }
        public List<string> AttachedFilePath { get; set; }
    }
}
