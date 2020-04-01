using Matrix.TaskManager.Common.Configuration;
using Matrix.TaskManager.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Matrix.TaskManager.Repository
{
	public class EmailRepository : IEmailRepository
	{
		private readonly ILogger<EmailRepository> _logger;
		private readonly AppSettings _appSettings;

		public EmailRepository(ILogger<EmailRepository> logger,
			IOptions<AppSettings> appSettings)
		{
			_appSettings = appSettings.Value;
			_logger = logger;

		}
		public async Task<bool> SendEmail(string emailFrom, string emailTo, string title, string data)
		{
			_logger.LogDebug($"send email from {emailFrom} to {emailTo}");

			try
			{
				SmtpClient client = new SmtpClient(_appSettings.Email.Host);
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(_appSettings.Email.UserName, _appSettings.Email.Password);
				client.Port = _appSettings.Email.Port;
				client.EnableSsl = true;

				MailMessage mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(emailFrom);
				mailMessage.To.Add(emailTo);
				mailMessage.Body = data;
				mailMessage.Subject = title;
				await client.SendMailAsync(mailMessage);
				return true;

			}
			catch (Exception ex)
			{
				_logger.LogDebug($"SendEmail: " + ex.Message);
				return false;
			}
		}
	}
}
