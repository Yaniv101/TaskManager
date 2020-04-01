using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Matrix.TaskManager.Common.Configuration;
using Matrix.TaskManager.Common.Helpers;
using Matrix.TaskManager.Common.Model;
using Matrix.TaskManager.Interfaces;
using Matrix.TaskManager.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Matrix.TaskManager.Services
{
	public interface IAuthenticationService
	{
		UserInfo Authenticate(string username, string password);
		Task<IEnumerable<UserInfo>> GetAll();
	}

	public class AuthenticationService : IAuthenticationService
	{
		private readonly ITaskManagerRepository _dataRepository;

		// users hardcoded for simplicity, store in a db with hashed passwords in production applications

		private readonly AppSettings _appSettings;
		private readonly ILogger<AuthenticationService> _logger;
		public AuthenticationService(
			ILogger<AuthenticationService> logger,
			IOptions<AppSettings> appSettings,
			ITaskManagerRepository dataRepository)
		{
			_appSettings = appSettings.Value;
			_logger = logger;
			_dataRepository = dataRepository;
		}


		public UserInfo Authenticate(string username, string password)
		{
			_logger.LogDebug($"authenticate user {username}");
			var user = _dataRepository.FindUser(username, password);
			//_users.SingleOrDefault(x => x.UserName == username && x.Password == password);

			// return null if user not found
			if (user == null)
				return null;

			if (!user.IsActive)
			{
				_logger.LogError($"Authenticate: failed to authenticate. user({username}) is not active");
				return null;
			}

			// authentication successful so generate jwt token
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.UserId.ToString()),
					new Claim(ClaimTypes.Email, user.Email)
				}),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			user.Token = tokenHandler.WriteToken(token);

			return user.WithoutPassword();
		}

		public async Task<IEnumerable<UserInfo>> GetAll()
		{
			var users = await _dataRepository.GetAllUsers();
			return users.WithoutPasswords();
		}
	}
}