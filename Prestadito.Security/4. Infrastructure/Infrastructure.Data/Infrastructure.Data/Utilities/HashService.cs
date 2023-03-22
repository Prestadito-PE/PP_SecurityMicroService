using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prestadito.Security.Application.Dto.Email;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Prestadito.Security.Infrastructure.Data.Utilities
{
    public class HashService
    {
        private readonly string _pathRoot;
        private readonly IConfiguration configuration;
        public HashService(IServiceProvider serviceProvider, IServiceScopeFactory factory)
        {
            var env = serviceProvider.GetService<IHostingEnvironment>();
            _pathRoot = $"{env.ContentRootPath}{Constantes.PathFinanciamientoTemplate}";
            configuration = factory.CreateScope().ServiceProvider.GetRequiredService<IConfiguration>();
        }

        public async Task EnviarCorreoAsync<T>(EmailData<T> obj, RecuperarClaveEmail message, string templateKey)
        {
            string smtp = configuration.GetSection("EmailSettings").GetSection("CorreoEnvio").Value;
            string key = configuration.GetSection("EmailSettings").GetSection("Key").Value;
            string ruta = "";
            ruta = $@"{_pathRoot}{obj.HtmlTemplateName}";
            string html = System.IO.File.ReadAllText(ruta);
            string body = Engine.Razor.RunCompile(html, $"{templateKey}", typeof(T), message);
            string correoDestino = string.Join(',', obj.EmailList);
            string correoSend = smtp;
            string clave = key;
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.UseDefaultCredentials = false;
            mail.From = new System.Net.Mail.MailAddress(correoSend);
            mail.To.Add(correoDestino);

            mail.Subject = "Test";
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpServer.Port = 587;
            
            SmtpServer.Credentials = new System.Net.NetworkCredential(correoSend, clave);
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }



    }
}
