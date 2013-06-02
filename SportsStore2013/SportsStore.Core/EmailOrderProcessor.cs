using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Core.Contracts;
using SportsStore.Core.Contracts.Models;

namespace SportsStore.Core
{
    public class EmailOrderProcessor :IOrderProcessor 
    {
        private readonly EmailSettings _settings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            _settings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = _settings.UseSsl;
                smtpClient.Host = _settings.ServerName;
                smtpClient.Port = _settings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_settings.Username, _settings.Password);

                if (_settings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = _settings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("---")
                    .AppendLine("Items:");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.LineSubTotal;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c})", line.Quantity, line.Product.Name, subtotal);
                    body.AppendLine();
                }

                body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? string.Empty)
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.State ?? string.Empty)
                    .AppendLine(shippingInfo.Country)
                    .AppendLine(shippingInfo.Zip)
                    .AppendLine("---")
                    .AppendFormat("Gift wrap {0}", shippingInfo.GiftWrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(_settings.From,
                    _settings.To, "New order submitted!", body.ToString());

                if (_settings.WriteAsFile)
                    mailMessage.BodyEncoding = Encoding.ASCII;

                smtpClient.Send(mailMessage);
            }
        }
    }

    public class EmailSettings
    {
        public string To = "orders@example.com";
        public string From = "sportsstore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"c:\email";
    }
}
