using Amazon.SecurityToken.Model;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.IO;
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

        public async Task<bool> EnviarCorreoAsync(EmailViewModel oCorreo/*RecuperarClaveEmail message, string templateKey, Dictionary<string, string> parameters*/)
        {
            bool enviado = false;
            try
            {
                EmailResponseViewModel oEmailResponse = new EmailResponseViewModel();
                oEmailResponse.Destinatarios = new List<string>();
                oEmailResponse.DestinatariosCC = new List<string>();
                oEmailResponse.DestinatariosCCO = new List<string>();
                oEmailResponse.Parametros = new Dictionary<string, string>();
                oEmailResponse.ListadoColumnas = new Dictionary<string, List<string>>();
                oEmailResponse.ListadoFilas = new Dictionary<string, List<List<string>>>();

                string[] correos = oCorreo.correo.Split(';');
                string[] correosCC = oCorreo.correocc.Split(';');
                string[] correosCCO = oCorreo.correocco.Split(';');

                foreach (var item in correos)
                    oEmailResponse.Destinatarios.Add(item);

                foreach (var item in correosCC)
                    oEmailResponse.DestinatariosCC.Add(item);

                foreach (var item in correosCCO)
                    oEmailResponse.DestinatariosCCO.Add(item);

                oEmailResponse.Parametros = oCorreo.parametros;
                oEmailResponse.Plantilla = oCorreo.plantilla;

                oEmailResponse.Asunto = oCorreo.asunto;
                oEmailResponse.Correo = oCorreo.correoUser;
                oEmailResponse.Contrasena = oCorreo.correoPwd;

                try
                {
                    MailMessage oMail = new MailMessage();
                    SmtpClient oSMTP = new SmtpClient();
                    string error = string.Empty;
                    string resultado = string.Empty;
                    try
                    {
                        string cuerpo = string.Empty;

                        //Correos Destino
                        foreach (string item in oEmailResponse.Destinatarios)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    var oMailAddres = new MailAddress(item);
                                    oMail.To.Add(oMailAddres);
                                }
                            }
                            catch (Exception)
                            {
                                enviado = false;
                            }
                        }

                        //Correos CC
                        foreach (string item in oEmailResponse.DestinatariosCC)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    var oMailAddres = new MailAddress(item);
                                    oMail.CC.Add(oMailAddres);
                                }
                            }
                            catch (Exception)
                            {
                                enviado = false;
                            }
                        }

                        //Correos Ocultos
                        foreach (string item in oEmailResponse.DestinatariosCCO)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    var oMailAddres = new MailAddress(item);
                                    oMail.Bcc.Add(oMailAddres);
                                }
                            }
                            catch (Exception)
                            {
                                enviado = false;
                            }
                        }
                        string bodyMessage = string.Empty;
                        using (var client = new WebClient())
                        {
                            client.Encoding = System.Text.Encoding.UTF8;
                            bodyMessage = client.DownloadString($"{oCorreo.plantilla}");
                        }
                        string cMessage = string.Empty;
                        if (oCorreo.parametros != null)
                        {
                            bodyMessage = TagReplace(oCorreo.parametros, bodyMessage);
                        }

                        oMail.Body = bodyMessage;
                        oMail.IsBodyHtml = true;
                        oMail.Subject = oCorreo.asunto;
                        oMail.From = new MailAddress(oCorreo.correo, oCorreo.displayName);
                        oSMTP.Host = oCorreo.host;
                        oSMTP.Port = Convert.ToInt32(oCorreo.puerto);
                        oSMTP.Credentials = new NetworkCredential(oCorreo.correoUser, oCorreo.correoPwd);
                        oSMTP.EnableSsl = true;
                        oSMTP.Send(oMail);
                        oSMTP.Dispose();
                        enviado = true;
                    }
                    catch (Exception)
                    {
                        enviado = false;
                    }
                    finally
                    {
                        oMail = null;
                        oSMTP = null;
                    }
                }
                catch (Exception)
                {
                    enviado = false;
                }
            }
            catch(Exception)
            {
                enviado = false;
            }
            return enviado;
        }

        private static string TagReplace(Dictionary<string, string> parametros, string bodyMessage)
        {
            foreach (var replacement in parametros)
            {
                bodyMessage = bodyMessage.Replace($"{{{replacement.Key}}}", replacement.Value);
            }
            return bodyMessage;
        }
    }
}
