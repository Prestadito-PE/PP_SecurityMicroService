using Amazon.SecurityToken.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prestadito.Security.Application.Dto.Email;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
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

        public async Task EnviarCorreoAsync<T>(EmailData<T> obj, RecuperarClaveEmail message, string templateKey, Dictionary<string, string> parameters)
        {
            try
            {


                string correoOrigen = configuration.GetSection("EmailSettings").GetSection("CorreoEnvio").Value;
                string key = configuration.GetSection("EmailSettings").GetSection("SecureKey").Value;
                string ruta = $@"{_pathRoot}{obj.HtmlTemplateName}";
                string html = System.IO.File.ReadAllText(ruta);
                string body = Engine.Razor.RunCompile(html, $"{templateKey}", typeof(T), message);
                string correoDestino = string.Join(',', obj.EmailList);
                string clave = key;
                MailMessage mail = new MailMessage(correoOrigen, correoDestino, parameters.First(x => x.Key == "Asunto").Value, body) { IsBodyHtml = true };
                SmtpClient SmtpServer = new SmtpClient(configuration.GetSection("EmailSettings").GetSection("Smtp").Value)
                {
                    EnableSsl = Convert.ToBoolean(configuration.GetSection("EmailSettings").GetSection("SSLEmail").Value),
                    Port = Convert.ToInt32(configuration.GetSection("EmailSettings").GetSection("Port").Value),
                    Credentials = new NetworkCredential(correoOrigen, key)
                };
                SmtpServer.Send(mail);
                SmtpServer.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
