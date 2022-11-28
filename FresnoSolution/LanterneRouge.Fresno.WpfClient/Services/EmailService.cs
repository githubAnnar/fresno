using Autofac;
using LanterneRouge.Fresno.WpfClient.Services.Interfaces;
using LanterneRouge.Fresno.WpfClient.Utils;
using log4net;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows;

namespace LanterneRouge.Fresno.WpfClient.Services
{
    public class EmailService : IEmailService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EmailService));
        private IApplicationSettingsService _applicationSettingsService;

        private IApplicationSettingsService ApplicationSettingsService
        {
            get
            {
                if (_applicationSettingsService == null)
                {
                    var scope = ServiceLocator.Instance.BeginLifetimeScope();
                    _applicationSettingsService = scope.Resolve<IApplicationSettingsService>();
                }

                return _applicationSettingsService;
            }
        }

        private void SendCompleteCallback(object sender, AsyncCompletedEventArgs e)
        {
            var message = e.UserState as MailMessage;

            if (e.Cancelled)
            {
                var cancelledMessage = new StringBuilder();
                if (message != null)
                {
                    cancelledMessage.Append($"Send Message to {string.Join(", ", message.To.Select(m => m.Address))} is cancelled!");
                }

                else
                {
                    cancelledMessage.Append("Send Message is cancelled!");
                }

                Logger.Warn(cancelledMessage.ToString());
                MessageBox.Show(cancelledMessage.ToString(), "Sending Message Cancelled", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (e.Error != null)
            {
                var errorMessage = new StringBuilder();
                if (message != null)
                {
                    errorMessage.Append($"Send Message to {string.Join(", ", message.To.Select(m => m.Address))} failed!");
                }

                else
                {
                    errorMessage.Append("Send Message failed!");
                }

                Logger.Error(errorMessage.ToString(), e.Error);
                MessageBox.Show(errorMessage.ToString(), "Error Sending Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                var sentMessage = new StringBuilder();
                if (message != null)
                {
                    sentMessage.Append($"Message is sent to {string.Join(", ", message.To.Select(m => m.Address))}");
                }

                else
                {
                    sentMessage.Append("Message is sent");
                }

                Logger.Info(sentMessage.ToString());
                MessageBox.Show(sentMessage.ToString(), "Message Sent", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (message != null)
            {
                message.Dispose();
            }
        }

        public void SendEmail(MailMessage mailMessage)
        {
            var smtpClient = new SmtpClient(ApplicationSettingsService.EmailServer, ApplicationSettingsService.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(ApplicationSettingsService.Username, PasswordHelpers.DecryptString(ApplicationSettingsService.Password)),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };
            smtpClient.SendCompleted += SendCompleteCallback;

            try
            {
                smtpClient.SendAsync(mailMessage, mailMessage);
            }

            catch (Exception e)
            {
                Logger.Error("Error sending mail!", e);
            }
        }
    }
}
